using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public Camera mainCamera;
	public GameObject player1, player2, ship;
	
	private string cameraMode = "both";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch(cameraMode){
			case "both":
				driveCam();
				break;
		}
	}
	//follows both players
	void bothCam(){
		Vector2 p1pos = player1.transform.position;//get players positions
		Vector2 p2pos = player2.transform.position;
		float camX = (p1pos.x + p2pos.x)/2;//average their position to set camera position
		float camY = (p1pos.y + p2pos.y)/2;
		mainCamera.transform.position = new Vector3(camX, camY,-10);
	}
	//in ship driving mode
	void driveCam(){
		Vector3 shipPos = ship.transform.position;
		mainCamera.transform.position = new Vector3(shipPos.x, shipPos.y, -10);
		mainCamera.orthographicSize = 26;
	}
	
}
