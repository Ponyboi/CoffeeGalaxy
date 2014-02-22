using UnityEngine;
using System.Collections;

public class PoolTest : MonoBehaviour {

	public Pool<BaseBehaviour> pool;
	public BaseBehaviour item;

	// Use this for initialization
	void Start () {
		pool = new Pool<BaseBehaviour>(item, 10);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			pool.Pull();
		}
	}
}
