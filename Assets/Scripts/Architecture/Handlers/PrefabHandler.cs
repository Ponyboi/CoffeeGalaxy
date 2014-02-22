using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Prefab{
	TestPrefab,
	BaseBehaviourExtender,
	Length
}

public class PrefabHandler : MonoBehaviour {

	private static PrefabHandler Instance;
	[SerializeField] [HideInInspector]
	private bool initialized;
	private BaseBehaviour[] prefabs;
	[SerializeField] [HideInInspector]
	private bool[] prefabsAssigned;
	[SerializeField] [HideInInspector]
	private bool[] preload;

	public void Init(){
		if(!initialized){
			initialized = true;
			prefabsAssigned = new bool[(int) Prefab.Length];
			preload = new bool[(int) Prefab.Length];
		}
	}

	public void Refresh(){
		RefreshBoolArray(ref prefabsAssigned);
		RefreshBoolArray(ref preload);
		CheckPrefabExistence();
	}

	private void CheckPrefabExistence(){
		for(int i = 0; i < (int) Prefab.Length; i++){
			if(prefabsAssigned[i]){
				UnityEngine.Object obj = Resources.Load(((Prefab) i).ToString());
				if(obj == null){
					prefabsAssigned[i] = false;
					preload[i] = false;
				}
			}
		}
	}

	void Awake(){
		if(Instance == null){

			Instance = this;
			prefabs = new BaseBehaviour[(int) Prefab.Length];

			for(int i = 0; i < (int) Prefab.Length; i++){
				if(preload[i]){
					LoadPrefab(i);
				}
			}
		}
		else{
			GameObject.Destroy(this.gameObject);
		}
	}

	public static T GetPrefab<T>(Prefab prefabEnum) where T : UnityEngine.Component{
		if(!Instance.PrefabLoaded(prefabEnum)){
			Instance.LoadPrefab((int) prefabEnum);
		}
		return Instance.prefabs[(int) prefabEnum].GetComponent<T>();
	}

	private void LoadPrefab(int index){
		prefabs[index] = Resources.Load<BaseBehaviour>(((Prefab) index).ToString());
	}

	private bool PrefabLoaded(Prefab prefabEnum){
		return !(prefabs[(int) prefabEnum] == null);
	}

	public void AddPrefab(int enumIndex){
		prefabsAssigned[enumIndex] = true;
	}

	public void RemovePrefab(int index){
		prefabsAssigned[index] = false;
		preload[index] = false;
	}

	public void SetPreload(int index, bool state){
		preload[index] = state;
	}

	public bool GetPreload(int index){
		return preload[index];
	}

	public bool Contains(Prefab prefab){
		return (prefabsAssigned[(int) prefab]);
	}

	private void RefreshBoolArray(ref bool[] array){
		if(array.Length != (int) Prefab.Length){
			bool[] newArray = new bool[(int) Prefab.Length];
			if((int) Prefab.Length > array.Length){
				array.CopyTo(newArray, 0);
			}
			else{
				for(int i = 0; i < (int) Prefab.Length; i++){
					newArray[i] = array[i];
				}
			}
			array = newArray;
		}
	}
	
}
