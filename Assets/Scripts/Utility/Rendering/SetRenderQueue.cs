using UnityEngine;
using System.Collections;

public class SetRenderQueue : MonoBehaviour {
	// 0 is earliest (think skybox)
	// 4000 + is latest (like gui)
	public int renderOrder = 2000;

	// Use this for initialization
	void Start () {
		this.renderer.material.renderQueue = renderOrder;
	}
}
