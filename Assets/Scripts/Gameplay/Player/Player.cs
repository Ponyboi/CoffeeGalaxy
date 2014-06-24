using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public int id;

	public float maxSpeed = 10f;
	bool FacingRight = true;
	bool Grounded = false;
	float GroundRadius = 0.2f;
	public Transform GroundCheck;
	public LayerMask GroundLayerMask;
	public float JumpForce = 700f;
	public float groundDrag = 5.0f;
	public float runDrag = 1.8f;
	public float airDrag = 2.5f;
	private Vector2 moveDirection;
	private Vector2 oldMoveDirection;
	private Vector2 jumpDir;

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
			jumpDir = transform.up * JumpForce;

			rigidbody2D.AddForce(jumpDir);
			//rigidbody2D.AddForce(new Vector2(0, JumpForce));
		}
		Debug.DrawLine(transform.position, jumpDir*10, Color.blue);
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

		Vector2 moveDirectionApplied = ((transform.right) * maxSpeed) * moveDirection.x;
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
}
