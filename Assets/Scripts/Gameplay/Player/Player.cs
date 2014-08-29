using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	//Ground Rotation
	public Vector2 rayOffset = new Vector2(0.3f, 0.4f);
	private float rayDist = 2f;
	private float maxRotationDegrees = 4f; 
	private float positionOffsetY = 0f;
	private RaycastHit2D leftHitInfo;
	private RaycastHit2D rightHitInfo;
	private Vector3 averageNormalUp;
	private Vector3 rightNormal;
	private Vector3 leftNormal;
	private float rotationMax = 30f;  //rotation difference between center of planet and slop of terrain in degrees

	//Booster
	public bool isBoosting = false;
	public float fuel = 100;
	public float boosterForce;

	//Gravity
	public GravityAttractor strongestAttractor;
	private float strongestGravity;

	private Transform myTransform;
	public List<GravityAttractor> attractors;
	Animator Anim;

	void Start () {
		Anim = GetComponent<Animator>();
		myTransform = transform;

		int children = transform.childCount;
		GameObject planets = GameObject.Find("_Planets");
		for (int i = 0; i < children; ++i)
			attractors.AddRange(planets.GetComponentsInChildren<GravityAttractor>());
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
			//jumpDir = transform.up * JumpForce;
			jumpDir = (transform.position - strongestAttractor.transform.position).normalized * JumpForce;

			rigidbody2D.AddForce(jumpDir);
			//rigidbody2D.AddForce(new Vector2(0, JumpForce));
		}
		Debug.DrawLine(transform.position, transform.position + (new Vector3(jumpDir.x, jumpDir.y, 0)*0.5f), Color.blue);
		Vector2 moveDirectionRot = ((transform.right) * maxSpeed) * moveDirection.x + transform.up*0.2f;
		Debug.DrawLine(transform.position, transform.position + (new Vector3(moveDirectionRot.x, moveDirectionRot.y, 0) * 3f), Color.red);
		Debug.DrawLine(transform.position, transform.position + averageNormalUp * 50f, Color.magenta);
		//Debug.Log("averageNormalUp: " + averageNormalUp);
		Debug.DrawLine(transform.position, transform.position + leftNormal * 50f, Color.green);
		Debug.DrawLine(transform.position, transform.position + rightNormal * 50f, Color.green);
	}

	void Flip() {
		FacingRight = !FacingRight;
		Vector3 TheScale = transform.localScale;
		TheScale.x *= -1;
		transform.localScale = TheScale;
	}

	void Movement() {
		//Movement
		//float move = 0.0f;
		//if (Grounded) {
		if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0) {
			moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		} else {
			moveDirection = new Vector2(ControllerInput.LeftAnalog_X_Axis(id, 0.2f), ControllerInput.LeftAnalog_Y_Axis(id, 0.2f));
		}
		
		if ((moveDirection.x > 0 || moveDirection.x < 0) && Grounded) {
			rigidbody2D.drag = runDrag;
		} else if (!Grounded) {
			rigidbody2D.drag = airDrag;
		} else {
			rigidbody2D.drag = groundDrag;
		}
		
		Vector2 moveDirectionApplied = ((transform.right) * maxSpeed) * moveDirection.x + transform.up*0.2f;
		rigidbody2D.AddForce(moveDirectionApplied * 3);


		Anim.SetFloat("Speed", Mathf.Abs(moveDirection.x));

		if(moveDirection.x > 0 && !FacingRight) {
			Flip();
		}else if (moveDirection.x < 0 && FacingRight) {
			Flip();
		}
		
		Grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayerMask);
		Anim.SetBool ("Grounded", Grounded);
		Anim.SetFloat("VSpeed", rigidbody2D.velocity.y);

		//Booster
		if (Input.GetKey(KeyCode.LeftShift) && fuel > 5) {
			rigidbody2D.AddForce(boosterForce * moveDirection);
			fuel -= 2;
			isBoosting = true;
		} else if (ControllerInput.Left_Bumper_Button(id) && fuel > 5) {
			rigidbody2D.AddForce(boosterForce * moveDirection);
			fuel -= 2;
			isBoosting = true;
		} else {
			isBoosting = false;
		}
		if (fuel < 100)
			fuel += 0.3f;

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
		if (Grounded) {
			if (doubleRaycastDown(rayOffset, rayDist, out leftHitInfo, out rightHitInfo)) {
				//Debug.Log("LeftHitInfo: " + leftHitInfo.normal);
				positionOnTerrain(leftHitInfo, rightHitInfo, maxRotationDegrees, positionOffsetY);
			}
			if (Vector3.Angle(averageNormalUp, (transform.position - strongestAttractor.transform.position)) > rotationMax) {
				Quaternion targetRotation = strongestAttractor.Orientation(myTransform);
				transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,50f * Time.deltaTime );
			}
		} else {
			Quaternion targetRotation = strongestAttractor.Orientation(myTransform);
			transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,50f * Time.deltaTime );
		}
		
		//Gravity
		if (Grounded) {
			Vector3 gravitySingular = strongestAttractor.Attract(myTransform);
			rigidbody2D.AddForce(gravitySingular);
		} else {
			rigidbody2D.AddForce(gravityAverage);
		}
	}

	public bool doubleRaycastDown(Vector2 movementRay, float rayLength,
	                       out RaycastHit2D leftHitInfo, out RaycastHit2D rightHitInfo)
	{
		Vector2 transformUp = transform.up;
		Vector2 transformRight = transform.right;
//		Ray leftRay = new Ray(transform.position + movementRay.y * transformUp
//		                      + movementRay.x * transformRight, -transformUp);
//		Ray rightRay = new Ray(transform.position + movementRay.y * transformUp
//		                       - movementRay.x * transformRight, -transformUp);
		Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
		Vector2 leftRay = new Vector2(myPos.x + movementRay.x, + myPos.y + movementRay.y);
		Vector2 rightRay = new Vector2(myPos.x - movementRay.x, + myPos.y + movementRay.y);
		
		//		Debug.DrawLine(transform.position + movementRay.y * transformUp
//		               + movementRay.x * transformRight, transform.position - movementRay.y * transformUp + movementRay.x * transformRight, Color.green);
//		Debug.DrawLine(transform.position + movementRay.y * transformUp
//			- movementRay.x * transformRight, transform.position - movementRay.y * transformUp - movementRay.x * transformRight, Color.green);
		
		
		rightHitInfo = new RaycastHit2D();
		leftHitInfo = new RaycastHit2D();
		
		leftHitInfo = Physics2D.Raycast(leftRay, -transformUp, rayLength, GroundLayerMask);
		rightHitInfo = Physics2D.Raycast(rightRay, -transformUp, rayLength, GroundLayerMask);

		if (leftHitInfo != null && rightHitInfo != null) {
			return true;
		}else {
			return false;
		}
	}

	public void positionOnTerrain(RaycastHit2D leftHitInfo, RaycastHit2D rightHitInfo,
	                       float maxRotationDegrees, float positionOffsetY)
	{
		Vector2 averageNormal = (leftHitInfo.normal + rightHitInfo.normal) / 2;
		Vector2 averagePoint = (leftHitInfo.point + rightHitInfo.point) / 2;

		averageNormalUp = averageNormal;
		leftNormal = leftHitInfo.normal;
		rightNormal = rightHitInfo.normal;
		
		Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, averageNormal);
		Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
		                                                    maxRotationDegrees);
		transform.rotation = Quaternion.Euler(0, 0, finalRotation.eulerAngles.z);
		
		//transform.position = averagePoint + transform.up * positionOffsetY;
	}
}
