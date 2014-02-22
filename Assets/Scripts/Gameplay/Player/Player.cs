using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int id;

	public float MaxSpeed = 10f;
	bool FacingRight = true;
	bool Grounded = false;
	float GroundRadius = 0.2f;
	public Transform GroundCheck;
	public LayerMask GroundLayerMask;
	public float JumpForce = 700f;

	Animator Anim;

	void Start () {
		Anim = GetComponent<Animator>();
	}

	void FixedUpdate () {

		//Movement
		float Move = ControllerInput.LeftAnalog_X_Axis(id);
		//float Move = Input.GetAxis("Horizontal")
		rigidbody2D.velocity = new Vector2(Move * MaxSpeed, rigidbody2D.velocity.y);
		Debug.Log("Move Value: " + Move);
			Anim.SetFloat("Speed", Mathf.Abs(Move));

		if(Move > 0 && !FacingRight) {
			Flip();
		}else if (Move < 0 && FacingRight) {
			Flip();
		}

		Grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayerMask);
		Anim.SetBool ("Grounded", Grounded);
		Anim.SetFloat("VSpeed", rigidbody2D.velocity.y);

	}

	void Update() {
		if (Grounded && ControllerInput.A_ButtonDown(id)) {
			Debug.Log ("in jump");
			Anim.SetBool("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, JumpForce));
		}
	}

	void Flip() {
		FacingRight = !FacingRight;
		Vector3 TheScale = transform.localScale;
		TheScale.x *= -1;
		transform.localScale = TheScale;
	}
}
