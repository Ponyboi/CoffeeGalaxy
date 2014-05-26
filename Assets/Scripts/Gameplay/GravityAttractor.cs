using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {
	public float gravity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 Attract(Transform body) {
//		Vector3 gravityUp = (body.position - transform.position).normalized;
//		Vector3 localUp = body.up;
//		
//		//body.rigidbody2D.AddForce(gravityUp * gravity);
//		Vector3 returnVec = gravityUp * gravity;
//		return returnVec;

		//This is how to get the distance vector between two objects.
		Vector3 dist = body.position - transform.position; 
		float r = dist.magnitude;
		dist /= r;
		
		//This is the Newton's equation
		//G = 6.67 * 10^-11 N.m².kg^-2
		//float G = 6.67f * Mathf.Pow(10, -11);
		float G = 5;
		float force = ((float)G * body.rigidbody2D.mass * rigidbody2D.mass) / (r * r);

		dist *= force;
		return -dist;
		
		//Then, just apply the forces
//		A.AddForce (dist * force);
//		B.AddForce (-dist * force);
	}

	public Quaternion Orientation(Transform body) {
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.up;

		Vector3 returnVec = gravityUp * gravity;
		
		Quaternion targetRotation = Quaternion.FromToRotation(localUp,gravityUp) * body.rotation;
		//body.rotation = Quaternion.Slerp(body.rotation,targetRotation,50f * Time.deltaTime );
		return targetRotation;
	}
}
