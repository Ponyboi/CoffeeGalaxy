using UnityEngine;
using System.Collections;

// Attach this script to objects that should persist between scenes, if they
// don't already take care of persistence themselves
public class DontDestroyOnLoad : MonoBehaviour {
	
	// Toggled in editor. Only one should be active at a time (destroySelf takes precedence)
	public bool destroySelfIfAnotherInstanceFound;
	public bool destroyOtherIfAnotherInstanceFound;
	
	// Should be assigned in editor to match the objects tag
	public GlobalData.Tags identificationTag;
	
	// Set in editor, defines scenes in which this object SHOULD ALWAYS be destroyed on load
	public Scenes[] exceptionScenes;
	
	public bool immuneOnAwake;
	private bool immune;
	
	private bool destroyed;

	// Use this for initialization
	void Start () {
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
	
	void Awake(){
		if(immuneOnAwake){
			immune = true;	
		}
	}
	
	void OnLevelWasLoaded(){
		
		if(!immune){
		
			destroyed = false;

			// Check for any of the exception levels, destroy self if found
			for(int i = 0; i < exceptionScenes.Length; i++){
				if(Application.loadedLevelName == exceptionScenes[i].ToString()){
					GameObject.DestroyImmediate(this.gameObject);
					destroyed = true;
					break;
				}
			}
				
			// If this object has been set up properly ...
			if(identificationTag != GlobalData.Tags.Void){
				// Loaded in a non-exception level
				if(!destroyed){
					// Changing tag in order to search for another instance
					this.tag = "";
					
					if(destroySelfIfAnotherInstanceFound){
						GameObject other = GameObject.FindGameObjectWithTag(identificationTag.ToString());
						if(other != null){
							GameObject.DestroyImmediate(this.gameObject);
						}
					}
					else if(destroyOtherIfAnotherInstanceFound){
						GameObject other = GameObject.FindGameObjectWithTag(identificationTag.ToString());
						if(other != null){
							GameObject.DestroyImmediate(other);
						}
					}
					
					if(this != null){
						this.tag = identificationTag.ToString();
					}
				}
			}
		}
		
		immune = false;
	}
}
