using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public Camera mainCamera;
	public float cameraZoom;
	public GameObject ship;
	public GameObject[] players;
	public int id;
	public cameraModes cameraMode;

	//Camera Mode Enum
	public enum cameraModes {Coop, Ship, Player};

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch(cameraMode){
			case cameraModes.Coop:
				driveCam();
				break;
			case cameraModes.Ship:
				bothCam();
				break;
			case cameraModes.Player:
				playerCam(id);
				break;
		}
	}
	//follows both players
	void bothCam(){
		Vector2 p1pos = players[1].transform.position;//get players positions
		Vector2 p2pos = players[2].transform.position;
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

	//follow particular player
	void playerCam(int id) {
		Vector3 playerPos = players[id].transform.position;
		mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(playerPos.x, playerPos.y, cameraZoom), Time.deltaTime * 10);
		mainCamera.orthographicSize = 26;
	}
	
}
