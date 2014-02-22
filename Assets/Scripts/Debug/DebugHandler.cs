using UnityEngine;
using System.Collections;

// This script acts as a static debug reference
// Add debugging booleans to modify in editor

// You can add this script in any scene and set up the vars
// specifically for that scene. However, if this script
// finds another Debug object on scene load, it overwrites
// it.

public enum DebugLog{
	Global = 0,
}

public class DebugHandler : BaseBehaviour {

	public static DebugHandler Instance;
	
	public DebugLogOption[] logOptions;
	
	public bool skipTitle;
	public bool debugExample;

	public void Init(){
		if(Instance == null){
			Instance = this;
		}
		else if(Instance != this){
			GameObject.Destroy(this.gameObject);
		}
	}

	void Awake(){
		Init();
	}

	public static void Log(string message){
		Log(DebugLog.Global, message);
	}

	public static void Log(DebugLog log, string message){
		if(Instance.logOptions[(int) DebugLog.Global].enabled && Instance.logOptions[(int) log].enabled){
			Debug.Log("Log " + log + ": " + message);
		}
	}

	[System.Serializable]
	public class DebugLogOption{
		public DebugLog log;
		public bool enabled;
	}
}