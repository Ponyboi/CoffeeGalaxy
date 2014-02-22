using UnityEngine;
using System.Collections;
using System;

public class GeneralTesting : MonoBehaviour {

	public BaseBehaviourExtender thing;
	public Pool<BaseBehaviour> pool;

	public UnityEngine.Component[] objects;

	public GameObject anchor;
	public float moveDelta;

	public int frameRate;
	public int v;

	bool set;
	bool set2;
	bool off;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

//		if(Input.GetKeyDown(KeyCode.Space)){
//			thing = PrefabHandler.GetPrefab<BaseBehaviourExtender>(Prefab.BaseBehaviourExtender);
//			pool = new Pool<BaseBehaviour>(thing, 5);
//		}
//
//		if(Input.GetKeyDown(KeyCode.RightArrow)){
//			BaseBehaviour obj = pool.Pull();
//			obj.transform.position = anchor.transform.position;
//			anchor.transform.position += Vector3.right * moveDelta;
//		}

		if(set){
			Settings.SetTargetFrameRate(frameRate);
		}

		if(set2){
			Settings.VsyncOn(v);
		}

		if(off){
			Settings.VsyncOff();
		}

		if(Input.GetKeyDown(KeyCode.RightArrow)){
			frameRate++;
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			frameRate--;
		}

		if(Input.GetKeyDown(KeyCode.W)){
			v++;
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			v--;
		}

	}

	void OnGUI(){
		GUILayout.BeginVertical();

		bool switchF = GUILayout.Button("Toggle Fullscreen");
		if(switchF){
			Settings.SetFullscreen(!Screen.fullScreen);
		}

		//GUILayout.Space(20);

		Vector2[] resolutions = Settings.GetSupportedResolutions();
		for(int i = 0; i < resolutions.Length; i++){
			GUILayout.BeginHorizontal();

			GUILayout.Label("X: " + resolutions[i].x);
			GUILayout.Label("Y: " + resolutions[i].y);

			if(resolutions[i] != Settings.GetCurrentResolution()){
				bool change = GUILayout.Button("Change Resolution");
				if(change){
					Settings.ChangeResolution(i);
				}
			}
			else{
				GUILayout.Label("Current Resolution");
			}

			GUILayout.EndHorizontal();
		}

		GUILayout.Space(20);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Framerate: " + frameRate);
		set = GUILayout.Button("Set");
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Vsync: " + Settings.GetVsyncState());
		GUILayout.Label("V: " + v);
		set2 = GUILayout.Button("Set");
		GUILayout.EndHorizontal();

		off = GUILayout.Button("Vsync Off");

		GUILayout.EndVertical();
	}
}
