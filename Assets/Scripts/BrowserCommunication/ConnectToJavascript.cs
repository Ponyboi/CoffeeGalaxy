using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnectToJavascript : MonoBehaviour {
	
	static string parameters;
	static Dictionary<string,string> paramTable;

	// Use this for initialization
	void Start () {
		BrowserInterface.EstablishCallbacks();
	}
	
	static void RetrieveFlashvars(){
		BrowserInterface.ConsoleLog("Unity: Language code retrieval attempted.");
		#if UNITY_FLASH && !UNITY_EDITOR
		parameters = UnityEngine.Flash.ActionScript.Expression<string>("FlashVars.Join('|')");
		string[] list = parameters.Split('|'); 
		paramTable = new Dictionary<string, string>();
	
		foreach (string parameter in list) {
			string key = parameter.Substring(0, parameter.IndexOf('='));
			string val = parameter.Substring(parameter.IndexOf('=') + 1);
			paramTable.Add(key, val);
		}
		#endif
		
		#if UNITY_WEBPLAYER
		string[] paramArray = Application.absoluteURL.Split('?');
		if(paramArray.Length > 1){
			// Get the last split chunk, in case there were multiple ?s in the url
			string[] list = paramArray[paramArray.Length - 1].Split('&');
			paramTable = new Dictionary<string, string>();
		
			foreach (string parameter in list) {
				string key = parameter.Substring(0, parameter.IndexOf('='));
				string val = parameter.Substring(parameter.IndexOf('=') + 1);
				paramTable.Add(key, val);
			}
		}
		#endif
	}
	
	public static string GetLanguageCode(){
		
		string result = "";
		
		if(paramTable == null){
			
			RetrieveFlashvars();
			
			if(paramTable == null){
				Debug.Log("Flash vars were not retrieved. Returning default 'US' language code.");
				BrowserInterface.ConsoleLog("Unity: Language code not found.");
				result = "US";
			}
			else{
				result = CheckLangCode();
			}	
		}
		else{
			result = CheckLangCode();
		}
		
		return result;
	}
	
	private static string CheckLangCode(){
		
		string result = "US";
		
		if(paramTable.ContainsKey("languageCode")){
			result = paramTable["languageCode"];
			BrowserInterface.ConsoleLog("Unity: Confirms languageCode " + result + " retrieved.");
		}
		else{
			Debug.Log("Flashvars did not contain a languageCode attribute. Returning default 'US'.");
		}
		
		return result;
	}
}
