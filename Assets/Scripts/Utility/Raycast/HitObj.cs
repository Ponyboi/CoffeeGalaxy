using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class HitObj: IComparable<HitObj>{
	public Transform transform;
	public Vector3 point;
	public Vector3 normal;
	public float distance;
	
	public HitObj(Transform transformIn, Vector3 pointIn, Vector3 normalIn, float distanceIn){
		transform = transformIn;
		point = pointIn;
		normal = normalIn;
		distance = distanceIn;
	}
	
	public int CompareTo(HitObj other){
		if(other.distance > distance){
			return -1;	
		}
		else{
			return 1;	
		}
	}
}
