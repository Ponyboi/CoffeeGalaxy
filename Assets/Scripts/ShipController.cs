using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {
	
	private int id = 1;
	public float MaxSpeed = 10f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float MoveX = ControllerInput.RightAnalog_X_Axis(id, 0.2f);
		float MoveY = ControllerInput.RightAnalog_Y_Axis(id, 0.2f);
		//rigidbody2D.velocity = new Vector2(Move * MaxSpeed, rigidbody2D.velocity.y);
		transform.position = new Vector2(transform.position.x+MaxSpeed*MoveX, transform.position.y+MaxSpeed*MoveY);
	}
}
