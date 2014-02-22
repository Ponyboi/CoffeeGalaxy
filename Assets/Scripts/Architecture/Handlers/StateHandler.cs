using UnityEngine;
using System.Collections;

/// <summary>
/// State handler is the most global handler. It monitors all handlers and input and directs the game flow.
/// </summary>
public class StateHandler : BaseBehaviour {

	private static StateHandler Instance;
	
	// Custom Handlers Here
	
	// Monitored Gui Objects Here

	// A delegate is being used to switch the method being performed during Update() based on
	// the current game state. It basically acts as a mutually exclusive boolean/method.
	private delegate void UpdateDelegate();
	private UpdateDelegate UpdateFunction;

	private VoidDelegate[] SceneLoadFunctions;
	private VoidDelegate[] SceneTransitionFunctions;
	
	void Start(){
		Begin();
	}
	
	// Retrieve all needed Handlers
	void Init(){

		Instance = this;

		MakePersistent();

		SetupStateTransitionFunctions();
		
		SceneHandler.Instance.Init();
		
		AudioHandler.Instance.Init();

		DebugHandler.Instance.Init();
		
		GlobalDataInit();
	}

	void SetupStateTransitionFunctions(){

		SceneLoadFunctions = new VoidDelegate[(int) Scenes.Length];
		SceneTransitionFunctions = new VoidDelegate[(int) SceneTransition.Length];

		SceneLoadFunctions[(int) Scenes.Initialization] = Begin;
		SceneTransitionFunctions[(int) Scenes.Initialization] = Begin;

		SceneLoadFunctions[(int) Scenes.DevScene] = DevSceneInit;
	}
	
	void Begin(){
		// Init self and set up needed handlers
		Init();
		// Assign update delegate
		UpdateFunction = InitUpdate;
		// Set up the title scene
		//ExampleSceneInit();
		
		DebugStartOptions();
	}
	
	void GlobalDataInit(){
		GlobalData.Init();
	}
	
	/// <summary>
	/// Used to set debug options that override the normal startup of the game
	/// </summary>
	void DebugStartOptions(){
		
		// Example of using debug handler to modify game flow when needed
		//if(DebugHandler.skipTitle){
			//sceneHandler.OpenSomeScene();	
		//}
	}
	
	/// <summary>
	/// StateHandler's update uses a delegate function to adapt at runtime based on the current state of
	/// the game. See individual Update functions to know what happens in each state.
	/// </summary>
	void Update () {
		// UpdateFunction changes at runtime based on the current state
		UpdateFunction();
	}
	
	#region UpdateDelegates

	void InitUpdate(){
		SceneHandler.LoadScene(Scenes.DevScene);
	}
	
	void ExampleSceneUpdate(){
		// Monitor gui input or
		// a handler flag
		//Ex: if(whateverHandler.complete){
			// sceneHandler.OpenSomeScene();
		//}
	}

	void DevSceneUpdate(){

	}
	
	#endregion
	
	#region SceneInits
	

	void ExampleSceneInit(){
		// Set up high level objects for the scene, like gui
		// Assign UpdateFunction
	}

	void DevSceneInit(){
		UpdateFunction = DevSceneUpdate;
	}
	
	#endregion

	public static void SceneLoaded(Scenes scene){
		Instance.SceneLoadFunctions[(int) scene]();
	}

	void TransitionComplete(SceneTransition transition){
		SceneTransitionFunctions[(int) transition]();
	}

	public static bool Initialized(){
		return !(Instance == null);
	}

}
