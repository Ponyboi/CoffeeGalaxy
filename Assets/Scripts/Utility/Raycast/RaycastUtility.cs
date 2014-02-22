using UnityEngine;
using System.Collections;
using System;

public static class RaycastUtility{
	
	private static Ray ray;
	private static Vector3 line;
	private static RaycastHit hitInfo;
	private static RaycastHit[] hitInfoArray;
	private static LayerMask layerMask;
	private static HitObj[] hitObjArray;

	#region Raycast
	
	public static HitObj RaycastFirst(Vector3 startPos, Vector3 endPos, params int[] layers){

		SetRay(startPos, endPos);
		SetLayerMask(ref layers);
		
		if(Physics.Raycast(ray, out hitInfo, line.magnitude, layerMask)){;
			return new HitObj(hitInfo.transform, hitInfo.point, hitInfo.normal, hitInfo.distance);
		}
		
		// Only reached if raycast does not find anything
		return null;
	}
	
	public static HitObj RaycastLast(Vector3 startPos, Vector3 endPos, params int[] layers){
		return RaycastFirst(endPos, startPos, layers);	
	}
	
	public static HitObj[] RaycastAllUnordered(Vector3 startPos, Vector3 endPos, params int[] layers){

		SetRay(startPos, endPos);
		SetLayerMask(ref layers);

		hitInfoArray = Physics.RaycastAll(ray, line.magnitude, layerMask);
		
		UpdateHitObjArray();
		
		return hitObjArray;
	}
	
	public static HitObj[] RaycastAllOrdered(Vector3 startPos, Vector3 endPos, params int[] layers){

		SetRay(startPos, endPos);
		SetLayerMask(ref layers);

		hitInfoArray = Physics.RaycastAll(ray, line.magnitude, layerMask);
		
		UpdateHitObjArray();
		
		return hitObjArray;
	}

	#endregion

	#region SphereCast

	public static HitObj SphereCastFirst(Vector3 startPos, Vector3 endPos, float radius, params int[] layers){

		SetRay(startPos, endPos);
		SetLayerMask(ref layers);

		if(Physics.SphereCast(ray, radius, out hitInfo, line.magnitude, layerMask)){
			return new HitObj(hitInfo.transform, hitInfo.point, hitInfo.normal, hitInfo.distance);
		}
		
		// Only reached if raycast does not find anything
		return null;
	}

	public static HitObj SphereCastLast(Vector3 startPos, Vector3 endPos, float radius, params int[] layers){
		return SphereCastFirst(endPos, startPos, radius, layers);	
	}
	
	public static HitObj[] SphereCastAllUnordered(Vector3 startPos, Vector3 endPos, float radius, params int[] layers){

		SetRay(startPos, endPos);
		SetLayerMask(ref layers);

		hitInfoArray = Physics.SphereCastAll(ray, radius, line.magnitude, layerMask);
		
		UpdateHitObjArray();
		
		return hitObjArray;
	}
	
	public static HitObj[] SphereCastAllOrdered(Vector3 startPos, Vector3 endPos, float radius, params int[] layers){

		SetRay(startPos, endPos);
		SetLayerMask(ref layers);

		hitInfoArray = Physics.SphereCastAll(ray, line.magnitude, radius, layerMask);
		
		UpdateHitObjArray();
		
		return hitObjArray;
	}

	#endregion

	#region CapsuleCast

	public static HitObj CapsuleCastFirst(Vector3 startPos, Vector3 endPos, float capsuleLength, float radius, params int[] layers){
		return CapsuleCastFirst(startPos, endPos, (endPos - startPos), capsuleLength, radius, layers);
	}

	public static HitObj CapsuleCastFirst(Vector3 startPos, Vector3 endPos, Vector3 capsuleOrientation, float capsuleLength, float radius, params int[] layers){

		SetRay(startPos, endPos);
		SetLayerMask(ref layers);
		
		float halfLength = capsuleLength / 2.0f;
		Vector3 offset = capsuleOrientation.normalized * halfLength;
		Vector3 capsuleEndOne = startPos - offset;
		Vector3 capsuleEndTwo = startPos + offset;
		
		if(Physics.CapsuleCast(capsuleEndOne, capsuleEndTwo, radius, line.normalized, out hitInfo, line.magnitude, layerMask)){
			return new HitObj(hitInfo.transform, hitInfo.point, hitInfo.normal, hitInfo.distance);
		}
		
		// Only reached if raycast does not find anything
		return null;
	}


	public static HitObj CapsuleCastLast(Vector3 startPos, Vector3 endPos, float capsuleLength, float radius, params int[] layers){
		return CapsuleCastLast(startPos, endPos, (endPos - startPos), capsuleLength, radius, layers);	
	}

	public static HitObj CapsuleCastLast(Vector3 startPos, Vector3 endPos, Vector3 capsuleOrientation, float capsuleLength, float radius, params int[] layers){
		return CapsuleCastFirst(endPos, startPos, capsuleOrientation, capsuleLength, radius, layers);
	}

	public static HitObj[] CapsuleCastAllUnordered(Vector3 startPos, Vector3 endPos, float capsuleLength, float radius, params int[] layers){
		return CapsuleCastAllUnordered(startPos, endPos, (endPos - startPos), capsuleLength, radius, layers);
	}
	
	public static HitObj[] CapsuleCastAllUnordered(Vector3 startPos, Vector3 endPos, Vector3 capsuleOrientation, float capsuleLength, float radius, params int[] layers){

		SetRay(startPos, endPos);
		SetLayerMask(ref layers);

		CapsuleCastAll(startPos, endPos, capsuleOrientation, capsuleLength, radius, layers);
		
		UpdateHitObjArray();
		
		return hitObjArray;
	}

	public static HitObj[] CapsuleCastAllOrdered(Vector3 startPos, Vector3 endPos, float capsuleLength, float radius, params int[] layers){
		return CapsuleCastAllOrdered(startPos, endPos, (endPos - startPos), capsuleLength, radius, layers);
	}
	
	public static HitObj[] CapsuleCastAllOrdered(Vector3 startPos, Vector3 endPos, Vector3 capsuleOrientation, float capsuleLength, float radius, params int[] layers){

		SetRay(startPos, endPos);
		SetLayerMask(ref layers);

		CapsuleCastAll(startPos, endPos, capsuleOrientation, capsuleLength, radius, layers);
		
		UpdateHitObjArray();

		return hitObjArray;
	}

	private static void CapsuleCastAll(Vector3 startPos, Vector3 endPos, Vector3 capsuleOrientation, float capsuleLength, float radius, params int[] layers){

			float halfLength = capsuleLength / 2.0f;
			Vector3 offset = capsuleOrientation.normalized * halfLength;
			Vector3 capsuleEndOne = startPos - offset;
			Vector3 capsuleEndTwo = startPos + offset;
			
			hitInfoArray = Physics.CapsuleCastAll(capsuleEndOne, capsuleEndTwo, radius, line.normalized, line.magnitude, layerMask);
	}

	#endregion

	#region Utility
	
	static void SetRay(Vector3 startPos, Vector3 endPos){
		line = endPos - startPos;
		ray.origin = startPos;
		ray.direction = line;
	}
		
	static void SetLayerMask(ref int[] layers){
		layerMask = 0;
		for(int i = 0; i < layers.Length; i++){
			layerMask = (layerMask | 1 << layers[i]);
		}
	}

	static void UpdateHitObjArray(){
		if(hitInfoArray.Length > 0){
			hitObjArray = new HitObj[hitInfoArray.Length];
			for(int i = 0; i < hitInfoArray.Length; i++){
				hitObjArray[i] = new HitObj(hitInfoArray[i].transform, hitInfoArray[i].point, hitInfoArray[i].normal, hitInfoArray[i].distance);	
			}
			Array.Sort(hitObjArray);
		}
		else{
			hitObjArray = null;
		}
	}

	#endregion
}
