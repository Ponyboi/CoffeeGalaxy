using UnityEngine;
using System.Collections;

public class BezierPath {
	
	Vector3 P0, P1, P2;
	
	public BezierPath(Vector3 P0, Vector3 P1, Vector3 P2){
		this.P0 = P0;
		this.P1 = P1;
		this.P2 = P2;
	}
	
	public Vector3 Interpolate(float time, float limit){
		time = Elastic(Mathf.Clamp(time/limit, 0, 1));
		return Interpolate(time);
	}
	
	public Vector3 Interpolate(float time){
		return Interpolate(P0, P1, P2, time);	
	}
	
	public static Vector3 Interpolate(Vector3 P0, Vector3 P1, Vector3 P2, float t){
		float temp = (1 - t);
		return (temp * temp) * P0 + 2 * temp * t * P1 + (t * t) * P2;
	}
	
	public static float Elastic(float x){
		return 	(x * x * (3 - (2 * x)));
	}
	
}
