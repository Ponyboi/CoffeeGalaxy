using UnityEngine;
using System.Collections;

public static class Settings{

	private static VoidDelegate ResolutionChange;
	private static VoidDelegate FrameRateChange;
	private static VoidDelegate WindowModeChange;

	public static void ChangeResolution(int resIndex){
		Screen.SetResolution(Screen.resolutions[resIndex].width, Screen.resolutions[resIndex].height, Screen.fullScreen);
		if(ResolutionChange != null){ ResolutionChange();}
	}

	public static void SetFullscreen(bool state){
		Screen.fullScreen = state;
		if(WindowModeChange != null){ WindowModeChange();}
	}

	public static Vector2 GetCurrentResolution(){
		return new Vector2(Screen.width, Screen.height);
	}

	public static Vector2[] GetSupportedResolutions(){

		Vector2[] result = new Vector2[Screen.resolutions.Length];

		for(int i = 0; i < Screen.resolutions.Length; i++){
			result[i] = new Vector2(Screen.resolutions[i].width, Screen.resolutions[i].height);
		}

		return result;
	}

	public static int GetHighestSupportedFrameRate(){
		return Screen.currentResolution.refreshRate;
	}

	public static void SetDefaultFrameRate(){
		Application.targetFrameRate = -1;
		if(FrameRateChange != null){ FrameRateChange();}
	}

	public static void SetTargetFrameRate(int hz){
		Application.targetFrameRate = Mathf.Clamp(hz, 0, 1000);
		if(FrameRateChange != null){ FrameRateChange();}
	}

	public static void VsyncOff(){
		QualitySettings.vSyncCount = 0;
		if(FrameRateChange != null){ FrameRateChange();}
	}

	public static void VsyncOn(int count){
		QualitySettings.vSyncCount = Mathf.Clamp(count, 1, 3);
		if(FrameRateChange != null){ FrameRateChange();}
	}
	// 0 is off, 1 is default fps, 2 is 1/2 monitor fps
	public static int GetVsyncState(){
		return Mathf.Clamp(QualitySettings.vSyncCount, 0, 3);
	}

	public static void RegisterResolutionListener(VoidDelegate listener){
		ResolutionChange += listener;
	}

	public static void RemoveResolutionListener(VoidDelegate listener){
		ResolutionChange -= listener;
	}

	public static void RegisterFrameRateListener(VoidDelegate listener){
		FrameRateChange += listener;
	}

	public static void RemoveFrameRateListener(VoidDelegate listener){
		FrameRateChange -= listener;
	}

	public static void RegisterWindowModeListener(VoidDelegate listener){
		WindowModeChange += listener;
	}

	public static void RemoveWindowModeListener(VoidDelegate listener){
		WindowModeChange -= listener;
	}
}
