using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TilePlacementDemo(){

		Application.LoadLevel ("game"); 
	}

	public void RoomDemo(){
		Application.LoadLevel ("Room1"); 
	}

	public void AbandonRoomDemo(){
		Application.LoadLevel ("AbandonRoom"); 
	}
}
