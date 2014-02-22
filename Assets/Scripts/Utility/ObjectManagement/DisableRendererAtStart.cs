using UnityEngine;
using System.Collections;

public class DisableRendererAtStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.renderer.enabled = false;
	}
}
