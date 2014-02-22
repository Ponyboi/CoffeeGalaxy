using UnityEngine;
using System.Collections;

/// <summary>
/// Global data is used to store generally useful data or provide an interface for highly generalized functions
/// </summary>
[NotRenamed]
public static class GlobalData{
	
	// State
	// Is the game paused
	public static bool paused = false;
	// Is the game window focused
	public static bool focus = true;
	
	// Audio
	public static bool mute = false;
	public static float masterVolume = 1;
	
	// Gui
	public static Rect screenRect;
	// Color assigned at the beginning of every OnGUI call
	// Used to modify transparency of the gui
	public static Color globalGuiColor;
	
	/// <summary>
	/// Enumaration mirroring the set of Tags being used in the game.
	/// </summary>
	public enum Tags{
		Void = 0,
		HandlerRoot,
		SceneHandler,
		DebugHandler,
		Player,
		AudioHandler
	}
	
	public static void Init(){
		globalGuiColor = new Color(1,1,1,1);	
		screenRect = new Rect(0,0, Screen.width, Screen.height);
	}
	
	/// <summary>
	/// Finds and returns the GameObject with the specifified tag.
	/// </summary>
	public static GameObject Get(Tags objectTag){
		GameObject obj = GameObject.FindGameObjectWithTag(objectTag.ToString());
		
		if(obj == null && objectTag != Tags.DebugHandler){
			Debug.Log("Could not find object with tag '" + objectTag +"'. Did you " +
										"forget to tag an object in the scene?");	
		}
		
		return obj;
	}
	
	[NotRenamed]
	public static void ApplicationFocused(){
		focus = true;
	}
	[NotRenamed]
	public static void ApplicationLostFocus(){
		focus = false;
	}
}
