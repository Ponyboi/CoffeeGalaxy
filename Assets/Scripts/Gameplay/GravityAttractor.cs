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
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.up;
		
		body.rigidbody2D.AddForce(gravityUp * gravity);
		Vector3 returnVec = gravityUp * gravity;
		
		Quaternion targetRotation = Quaternion.FromToRotation(localUp,gravityUp) * body.rotation;
		body.rotation = Quaternion.Slerp(body.rotation,targetRotation,50f * Time.deltaTime );	
		
		return returnVec;
	}
}
