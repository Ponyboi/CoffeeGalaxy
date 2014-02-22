using UnityEngine;
using System.Collections;

/// <summary>
/// Attach to "one-shot" particle systems to have them destroy themselves
/// once they have finished playing
/// </summary>
public class ParticleSystemKiller : MonoBehaviour {
	
	ParticleSystem particleGen;
	
	// Use this for initialization
	void Start () {
		particleGen = this.GetComponent<ParticleSystem>();
	}
	
	void Update(){
		if(particleGen.isStopped){
			GameObject.Destroy(this.gameObject);
		}
	}
}
