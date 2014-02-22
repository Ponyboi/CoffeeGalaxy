using UnityEngine;
using System.Collections;

[NotConverted]
[NotRenamed]
public class BrowserInterface{

    [NotRenamed]
    public static void mouseEnterGameArena(){
		GlobalData.ApplicationFocused();
    }
	
    [NotRenamed]
    public static void mouseLeaveGameArena(){
		GlobalData.ApplicationLostFocus();
    }
	
	[NotRenamed]
	public static void EstablishCallbacks(){
		;
	}
	
	[NotRenamed]
	public static void ConsoleLog(string message){
		#if UNITY_WEBPLAYER
		Application.ExternalCall("ConsoleLog2", message);
		#endif
	}

}

