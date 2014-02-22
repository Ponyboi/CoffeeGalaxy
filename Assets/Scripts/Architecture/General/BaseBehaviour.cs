using UnityEngine;
using System.Collections;

public class BaseBehaviour : MonoBehaviour {

	// Use this for initialization
	public virtual void Start () {

	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}

	public virtual void SetActive(bool state){
		gameObject.SetActive(state);
	}

	public virtual bool IsActive(){
		return gameObject.activeSelf;
	}

	public virtual void SetRenderers(bool state){
		renderer.enabled = state;
	}

	public virtual void SetColliders(bool state){
		collider.enabled = state;
	}

	public virtual Color GetColor(){
		return renderer.material.color;
	}

	public virtual void SetColor(Color color){
		renderer.material.color = color;
	}

	public virtual void SetAlpha(float normalizedAlpha){
		Color color = GetColor();
		color.a = normalizedAlpha;
		SetColor(color);
	}

	public virtual void SaveToXml(string path){
		XmlUtil xmlUtil = new XmlUtil();
		xmlUtil.OpenXml(path);
		xmlUtil.WriteGameObject(gameObject);
		xmlUtil.Close();
	}

	public virtual BaseBehaviour Duplicate(){
		BaseBehaviour result = (GameObject.Instantiate(this.gameObject) as GameObject).GetComponent<BaseBehaviour>();
		return result;
	}

	// Calling this prevents the object from being destroyed during
	// scene transitions
	public void MakePersistent(){
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
}
