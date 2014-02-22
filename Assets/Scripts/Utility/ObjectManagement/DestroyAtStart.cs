using UnityEngine;
using System.Collections;

public class DestroyAtStart : MonoBehaviour {
	
	// Generally turn this on for actual builds, or
	// if you just don't need some object when the game is actually running
	public bool destructionEnabled;

	// Use this for initialization
	void Start () {
		if(destructionEnabled){
			GameObject.Destroy(this.gameObject);
		}
	}
	
}
