using UnityEngine;
using System.Collections;

public class Timer{
	
	private float startTime;
	private float stopwatchTime;
	
	public Timer(){
		Reset();	
	}
	
	public Timer(float time){
		NewStopwatch(time);	
	}
	
	public void NewStopwatch(float time){
		Reset();
		stopwatchTime = time;
	}
	
	public bool StopwatchDone(){
		return (GetTime() > stopwatchTime);	
	}
	
	public float GetTime(){
		return Time.time - startTime;	
	}
	
	public float GetNormalizedTime(){
		return GetNormalizedTime(stopwatchTime);	
	}
	
	public float GetNormalizedTime(float timePeriod){
		if (timePeriod != 0f){
			return Mathf.Clamp(GetTime() / timePeriod, 0, 1.0f);	
		}
		else{
			return 0;
		}
	}
	
	public float GetRawNormalizedTime(){
		return GetRawNormalizedTime(stopwatchTime);	
	}
	
	public float GetRawNormalizedTime(float timePeriod){
		return GetTime() / timePeriod;
	}
	
	public void Reset(){
		startTime = Time.time;	
	}
	
	public void ResetWithOffset(float offset){
		startTime = Time.time + offset;
	}
	
	public void ResetPercentOffset(float percentOffset){
		startTime = Time.time + (stopwatchTime * percentOffset);
	}
	
	public float GetStopwatchLength(){
		return stopwatchTime;	
	}
}
