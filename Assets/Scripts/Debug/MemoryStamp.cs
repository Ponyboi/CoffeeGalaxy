using UnityEngine;
using System;

public static class MemoryStamp{
	
	/// <summary>
	///	Prints the available memory, in bytes, along with the passed tag.
	/// </summary>
	public static long StampMemory(string tag){
		long memory = GC.GetTotalMemory(false);
		Debug.Log(tag + " " + memory);
		return memory;
	}
	
	/// <summary>
	/// Prints the difference between two passed memory counts (longs), along with the passed tag.
	/// </summary>
	public static long StampDiff(string tag, long startMem, long endMem){
		Debug.Log(tag + " " + (endMem - startMem));
		return (endMem - startMem);
	}
	
}
