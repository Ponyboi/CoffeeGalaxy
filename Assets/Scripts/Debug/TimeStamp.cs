using UnityEngine;
using System.Collections;

public static class TimeStamp{

	/// <summary>
	/// Stamps time in seconds since game start, including TimeScale affects and ignoring editor pauses
	/// </summary>
	public static float StampTime(string tag){
		Debug.Log(tag + " " + Time.time);
		return Time.time;
	}
	
	/// <summary>
	/// Stamps time in seconds since game start, ignoring TimeScale affects and including editor
	/// </summary>
	public static float StampRealTime(string tag){
		Debug.Log(tag + " " + Time.realtimeSinceStartup);
		return Time.realtimeSinceStartup;
	}
	
	/// <summary>
	/// Stamps time in seconds since last level load
	/// </summary>
	public static float StampLevelTime(string tag){
		Debug.Log(tag + " " + Time.timeSinceLevelLoad);
		return Time.timeSinceLevelLoad;
	}
	
	/// <summary>
	/// Stamps the difference in seconds between two given times
	/// </summary>
	/// <returns>
	/// Difference between times in seconds as a float
	/// </returns>
	/// <param name='tag'>
	/// string tag to stamp the time difference with
	/// </param>
	/// <param name='startTime'>
	/// Start time.
	/// </param>
	/// <param name='endTime'>
	/// End time.
	/// </param>
	public static float StampDiff(string tag, float startTime, float endTime){
		Debug.Log(tag + " " + (endTime - startTime));
		return (endTime - startTime);
	}
}
