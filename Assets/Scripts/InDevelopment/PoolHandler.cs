using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PoolHandler : MonoBehaviour {

	private int defaultPoolInitSize = 5;

	private GenericDictionary<Prefab> enumPoolDictionary;
	private GenericDictionary<BaseBehaviour> specificObjectPoolDictionary;
	//private GenericDictionary<Type> typePoolDictionary;

	void Awake(){
		Init();
	}

	void Init(){
		enumPoolDictionary = new GenericDictionary<Prefab>();
		specificObjectPoolDictionary = new GenericDictionary<BaseBehaviour>();
		//typePoolDictionary = new GenericDictionary<Type>();
	}

	public Pool<T> GetPool<T>(Prefab prefabEnum) where T : BaseBehaviour{
		if(enumPoolDictionary.ContainsKey(prefabEnum)){
			return enumPoolDictionary.GetValue<Pool<T>>(prefabEnum);
		}
		else{
			Pool<T> newPool = new Pool<T>(PrefabHandler.GetPrefab<T>(prefabEnum), defaultPoolInitSize);
			enumPoolDictionary.Add<Pool<T>>(prefabEnum, newPool);
			return newPool;
		}
	}

	public Pool<T> GetPool<T>(T keyObject) where T : BaseBehaviour{
		if(specificObjectPoolDictionary.ContainsKey(keyObject)){
			return specificObjectPoolDictionary.GetValue<Pool<T>>(keyObject);
		}
		else{
			Pool<T> newPool = new Pool<T>(keyObject, defaultPoolInitSize);
			specificObjectPoolDictionary.Add<Pool<T>>(keyObject, newPool);
			return newPool;
		}
	}

//	public Pool<T> GetPool<T>(Type key) where T : BaseBehaviour{
//		if(typePoolDictionary.ContainsKey(key)){
//			return specificObjectPoolDictionary.GetValue<Pool<T>>(key);
//		}
//		else{
//			Pool<T> newPool = new Pool<T>(key, defaultPoolInitSize);
//			typePoolDictionary.Add<Pool<T>>(key, newPool);
//			return newPool;
//		}
//	}
	
}
