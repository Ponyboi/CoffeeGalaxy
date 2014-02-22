using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericDictionary<G>{

	private Dictionary<G, object> _dict = new Dictionary<G, object>();
	
	public void Add<T>(G key, T value) where T : class
	{
		_dict.Add(key, value);
	}

	public T GetValue<T>(G key) where T : class
	{
		return _dict[key] as T;
	}

	public bool ContainsKey(G key){
		return _dict.ContainsKey(key);
	}
}