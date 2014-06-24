using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int id;

	public float maxSpeed = 10f;
	bool FacingRight = true;
	bool Grounded = false;
	float GroundRadius = 0.4f;
	public Transform GroundCheck;
	public LayerMask GroundLayerMask;
	public float JumpForce = 700f;
	public float groundDrag = 5.0f;
	public float runDrag = 1.8f;
	public float airDrag = 2.5f;
	private Vector2 moveDirection;
	private Vector2 oldMoveDirection;
	private Vector2 jumpDir;

	//Gravity
	public GravityAttractor strongestAttractor;
	private float strongestGravity;
	public GravityAttractor[] attractors;
	private Transform myTransform;

	Animator Anim;

	void Start () {
		Anim = GetComponent<Animator>();
		myTransform = transform;
	}

	void FixedUpdate () {
		
		//Movement
		Movement();

		//Gravity
		ApplyGravity();

	

		
	}

	void Update() {
		if ((Grounded && ControllerInput.A_ButtonDown(id)) || (Grounded && Input.GetKeyDown(KeyCode.W))) {
			Anim.SetBool("Ground", false);
			jumpDir = transform.up * JumpForce;

			rigidbody2D.AddForce(jumpDir);
			//rigidbody2D.AddForce(new Vector2(0, JumpForce));
		}
		Debug.DrawLine(transform.position, transform.position + (new Vector3(jumpDir.x, jumpDir.y, 0)*0.5f), Color.blue);
		Debug.DrawLine(transform.position, transform.position + (new Vector3(moveDirection.x, moveDirection.y, 0) * 0.5f), Color.red);
	}

	void Flip() {
		FacingRight = !FacingRight;
		Vector3 TheScale = transform.localScale;
		TheScale.x *= -1;
		transform.localScale = TheScale;
	}

	void Movement() {
		float move = 0.0f;
		//if (Grounded) {
		if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0) {
			move = Input.GetAxis("Horizontal");
		} else {
			move = ControllerInput.LeftAnalog_X_Axis(id, 0.2f);
		}
		
		if ((move > 0 || move < 0) && Grounded) {
			rigidbody2D.drag = runDrag;
		} else if (!Grounded) {
			rigidbody2D.drag = airDrag;
		} else {
			rigidbody2D.drag = groundDrag;
		}

		//Vector2 oldBodyForce =  rigidbody2D.velocity - oldMoveDirection;
		moveDirection = (transform.right * (maxSpeed * move)) + transform.up*0.2f;
		//rigidbody2D.velocity = (moveDirection); // + oldBodyForce);
		rigidbody2D.AddForce(moveDirection * 3);
		//oldMoveDirection = moveDirection;

		//		Debug.Log("Move Value: " + Move);
		Anim.SetFloat("Speed", Mathf.Abs(move));
		
		if(move > 0 && !FacingRight) {
			Flip();
		}else if (move < 0 && FacingRight) {
			Flip();
		}
		
		Grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayerMask);
		Anim.SetBool ("Grounded", Grounded);
		Anim.SetFloat("VSpeed", rigidbody2D.velocity.y);
	}

	void ApplyGravity() {
		Vector3 gravityAverage = new Vector3(0,0,0);
		foreach (GravityAttractor planet in attractors) {
			if (planet.Attract(myTransform).magnitude > strongestGravity) {
				strongestGravity = (planet.Attract(myTransform)).magnitude;
				strongestAttractor = planet;
			}
			gravityAverage += planet.Attract(myTransform);
		}
		//Rotation
		Quaternion targetRotation = strongestAttractor.Orientation(myTransform);
		transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,50f * Time.deltaTime );
		
		//Gravity
		if (Grounded) {
			Vector3 gravitySingular = strongestAttractor.Attract(myTransform);
			rigidbody2D.AddForce(gravitySingular);
		} else {
			rigidbody2D.AddForce(gravityAverage);
		}
	}

//	bool doubleRaycastDown(TerrainMovementRayProperties movementRay, float rayLength,
//	                       out RaycastHit leftHitInfo, out RaycastHit rightHitInfo)
//	{
//		Vector3 transformUp = transform.up;
//		Vector3 transformRight = transform.right;
//		Ray leftRay = new Ray(transform.position + movementRay.originOffsetY * transformUp
//		                      + movementRay.distanceFromCenter * transformRight, -transformUp);
//		Ray rightRay = new Ray(transform.position + movementRay.originOffsetY * transformUp
//		                       - movementRay.distanceFromCenter * transformRight, -transformUp);
//		
//		return Physics.Raycast(leftRay, out leftHitInfo, rayLength, DefaultTerrainLayerMask)
//			&& Physics.Raycast(rightRay, out rightHitInfo, rayLength, DefaultTerrainLayerMask);
//	}

	void positionOnTerrain(RaycastHit leftHitInfo, RaycastHit rightHitInfo,
	                       float maxRotationDegrees, float positionOffsetY)
	{
		Vector3 averageNormal = (leftHitInfo.normal + rightHitInfo.normal) / 2;
		Vector3 averagePoint = (leftHitInfo.point + rightHitInfo.point) / 2;
		
		Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, averageNormal);
		Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
		                                                    maxRotationDegrees);
		transform.rotation = Quaternion.Euler(0, 0, finalRotation.eulerAngles.z);
		
		transform.position = averagePoint + transform.up * positionOffsetY;
	}
}
