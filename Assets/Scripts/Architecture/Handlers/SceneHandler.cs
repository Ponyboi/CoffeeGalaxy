using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Scenes{
	Initialization = 0,
	DevScene,
	Length
}

public enum SceneTransition{
	Temp = 0,
	Length
}

/// <summary>
/// Scene handler manages the transitions between scenes, and provides specific methods for each possible transition.
/// </summary>
public class SceneHandler : BaseBehaviour {
	
	public static SceneHandler Instance;
	
	public int levelIndex = 0;
	public Scenes currentScene;
	public string currentSceneName = "";
	private bool levelLoadUpdateComplete;
	
	private bool transitioning;
	private Scenes queuedScene;

	private Transition transition;
	private Dictionary<Scenes, SceneTransitions> transitionsDict;

	// Notes: You can cast enums to ints, ex: (int) Scenes.SceneOne
	// Likewise, ints can be cast to enum values, ex: ((Scenes) 1) == Scenes.SceneTwo
	// And, wonderfully, enumerated values, when treated or cast to strings, return their variable
	// name, ex: Scenes.SceneOne.ToString() == "SceneOne"

	public void Init() {
		MakePersistent();
	}
	
	// Awake is called once in the lifetime of a script instance, the first time it is loaded
	void Awake(){
		SetupSingleton();
		SetupTransitions();
		UpdateLevelInfo();
	}
	
	void SetupSingleton(){
		if(Instance == null){
			Instance = this;	
		}
		else{
			GameObject.Destroy(this.gameObject);	
		}
	}

	void SetupTransitions(){

		transitionsDict = new Dictionary<Scenes, SceneTransitions>();

		// Initialization
		Transition nullTransition = new Transition(Scenes.Initialization, null, null);
		SceneTransitions initializationTransitions = new SceneTransitions(nullTransition);

		transitionsDict.Add(Scenes.Initialization, initializationTransitions);
	}
	
	public static bool SceneLoaded(Scenes scene){
		return (Instance.currentSceneName.Equals(scene.ToString()));	
	}

	// This function will only be called on persistent objects, and not on the first scene
	// in which they are instantiated. Ie, first time a level is loaded with an object
	// this will not be called (Use Awake instead).
	void OnLevelWasLoaded(){
		UpdateLevelInfo();
	}

	public static void SceneTransition(Scenes scene){
		SceneTransition(scene, 0);
	}

	public static void SceneTransition(Scenes scene, int version){
		Instance.transition = Instance.transitionsDict[scene].GetTransition(version);
		Instance.transition.Exit();
		LoadScene(scene);
	}
	
	public static void LoadScene(Scenes scene){
		Instance.LoadScene((int) scene);	
	}
	
	public static void LoadScene(string sceneName){
		Debug.Log("Attempting to load scene: " + sceneName);
		Application.LoadLevel(sceneName);
	}
	
	void LoadScene(int sceneIndex){
		levelLoadUpdateComplete = false;
		Debug.Log("Attempting to load scene with index: " + sceneIndex + ", name: " + ((Scenes) sceneIndex));
		if(sceneIndex < Application.levelCount){
			Application.LoadLevel(sceneIndex);
		}
		else{
			Debug.Log("Scene Load for scene with index: " + sceneIndex + ", name: " + ((Scenes) sceneIndex) +
				" failed. Scene index is invalid.");
		}
	}
	
	// Updates info on current loaded level from SceneName object.
	void UpdateLevelInfo(){
		
		string loadedSceneName = Application.loadedLevelName;
		currentScene = (Scenes) Application.loadedLevel;
		
		if(loadedSceneName == currentSceneName){
			// This gets called any time title scene is loaded, but otherwise it's likely an error
			Debug.Log("Potential Error: Loaded same scene or another scene load failed: " + currentSceneName);
		}
		else{
			Debug.Log("Scene " + loadedSceneName + " loaded.");
		}
		
		currentSceneName = loadedSceneName;

		TransitionEnterCheck();
		
		levelLoadUpdateComplete = true;
	}

	void TransitionEnterCheck(){
		if(transition != null){
			transition.Enter();
		}
		else if(StateHandler.Initialized()){
			StateHandler.SceneLoaded(currentScene);
		}
	}
	
	public static bool LevelLoadCheckComplete(){
		return Instance.levelLoadUpdateComplete;
	}

	private class SceneTransitions{

		private Transition[] transitions;

		public SceneTransitions(params Transition[] transitions){
			this.transitions = transitions;
		}

		public Transition GetTransition(int version){
			return transitions[version];
		}
	}

	private class Transition{

		SceneHandler parent;
		Scenes scene;
		VoidDelegate SceneExit;
		VoidDelegate SceneEnter;

		public Transition(Scenes scene, VoidDelegate SceneExit, VoidDelegate SceneEnter){
			this.parent = SceneHandler.Instance;
			this.scene = scene;
			this.SceneExit = SceneExit;
			this.SceneEnter = SceneEnter;
		}

		public void Exit(){
			if(SceneExit != null){SceneExit();}
		}

		public void Enter(){
			if(SceneEnter != null){SceneEnter();}
		}
	}
}
