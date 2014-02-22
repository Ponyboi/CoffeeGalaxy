using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool<T> where T : BaseBehaviour {

	private Queue<T> pool;
	private Queue<T> activePool;
	private T model;
	private int capacity;
	private bool recycle;

	public Pool(T item, int size) : this(item, size, false){

	}

	public Pool(T item, int size, bool recycle){

		this.recycle = recycle;

		capacity = 1;

		pool = new Queue<T>(size);

		model = item;
		model.SetActive(false);

		T copy = (T) model.Duplicate();
		copy.SetActive(false);
		pool.Enqueue(copy);

		if(recycle){
			activePool = new Queue<T>(size);
		}

		EnlargePool(size);
	}

	public T Pull(){

		if(pool.Count == 1){
			if(recycle){
				RecycleActiveQueue();
			}
			else{
				EnlargePool();
			}
		}

		T result = pool.Dequeue();
		result.SetActive(true);

		if(recycle){
			activePool.Enqueue(result);
		}

		return result;
	}

	public void Return(T item){
		item.SetActive(false);
		if(!recycle){ pool.Enqueue(item); }
	}

	private void EnlargePool(){
		EnlargePool(capacity);
	}

	private void EnlargePool(int count){

		T copy;

		for(int i = 0; i < count; i++){
			copy = (T) model.Duplicate();
			copy.SetActive(false);
			pool.Enqueue(copy);
		}

		capacity += count;
	}

	private void RecycleActiveQueue(){

		T item;
		int count = activePool.Count;

		for(int i = 0; i < count; i++){
			item = activePool.Dequeue();
			if(item.IsActive()){
				activePool.Enqueue(item);
			}
			else{
				pool.Enqueue(item);
			}
		}

		if(pool.Count == 1){
			item = activePool.Dequeue();
			item.SetActive(false);
			pool.Enqueue(item);
		}
	}
}
