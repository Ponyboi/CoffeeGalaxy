using UnityEngine;
using System.Collections;

public class GravityBody : MonoBehaviour {

	public GravityAttractor strongestAttractor;
	private float strongestGravity;
	public GravityAttractor[] attractors;
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
	//	rigidbody2D.constraint = RigidbodyConstraints2D.FreezeRotation;
	//	rigidbody2D.useGravity = false;
		myTransform = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log("player pos: " + transform.position);
		Vector3 gavityAverage = new Vector3(0,0,0);
		foreach (GravityAttractor planet in attractors) {
			if (planet.Attract(myTransform).magnitude > strongestGravity) {
				strongestGravity = (planet.Attract(myTransform)).magnitude;
				//strongestAttractor = planet;
			}
			gavityAverage += planet.Attract(myTransform);
		}
		//Rotation
		Quaternion targetRotation = strongestAttractor.Orientation(myTransform);
		transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,50f * Time.deltaTime );

		//Gravity
		rigidbody2D.AddForce(gavityAverage);
	}
}
