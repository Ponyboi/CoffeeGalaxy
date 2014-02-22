using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NetworkView))]
public class NetworkHandler : BaseBehaviour {

	public static NetworkHandler Instance;

	// All receiving end messages are stored in these delegate arrays
	// Ie, these are the messages other objects can call on this client
	private IntParamDelegate[] intDels;
	private StringParamDelegate[] stringDels;
	private FloatParamDelegate[] floatDels;

	private int intEnd;
	private int stringEnd;
	private int floatEnd;

	// Use this for initialization
	void Awake () {
		if(Instance == null){
			Instance = this;
		}
		else{
			GameObject.Destroy(this.gameObject);
		}
	}

	public static int AddNetworkedMessage(IntParamDelegate function){

		if(Instance.intEnd == Instance.intDels.Length){
			IntParamDelegate[] newIntDels = new IntParamDelegate[Instance.intDels.Length * 2];
			Instance.intDels.CopyTo(newIntDels, 0);
			Instance.intDels = newIntDels;
		}

		Instance.intDels[Instance.intEnd] = function;
		Instance.intEnd++;

		return Instance.intEnd - 1;
	}

	public static void RegisterNetworkedMessage(IntParamDelegate function, int messageId){
		if(messageId < Instance.intEnd){
			Instance.intDels[messageId] += function;
		}
	}

	public static void IntMessage(int messageId, int param){
		Instance.networkView.RPC("IntMessageInternal", RPCMode.Others, messageId, param);
	}

	[RPC]
	private void IntMessageInternal(int messageId, int param){
		intDels[messageId](param);
	}
}
