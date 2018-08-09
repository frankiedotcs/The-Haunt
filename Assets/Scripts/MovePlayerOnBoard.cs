using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerOnBoard : MonoBehaviour {
	//Click to Move script
	//Moves the object towards the mouse position on left mouse click

	private int smooth = 2; //Determines how quickly object moves towards position

	public Vector3 targetPosition; //Determines the location of where the object will move

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

		//can't move player unless the "M" key has been pressed
		//if (GameManager.instance.SeeActivity ().ToLower () == "placing tile") {
	
		

			//mouse click to move the player
			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("in here");
				Plane playerPlane = new Plane (Vector3.up, transform.position); //the plane of which the player will be moving about
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 	//get the position of the mouse to move the player to
				float hitdist = 0.0f; 
				Debug.Log ("down here"); 
				if (playerPlane.Raycast (ray, out hitdist)) {

					Vector3 targetPoint = ray.GetPoint (hitdist); 													//the targetpoint for the object
					targetPosition = ray.GetPoint (hitdist); 														//the targetposition for the object
					Quaternion targetRotation = Quaternion.LookRotation (targetPoint - transform.position); 		//the targetrotation for the object so it remains at the right rotation
					transform.rotation = targetRotation; 															//assign the transform.rotation to the targetRotation
				}
			}
			transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * smooth); 	//move the object
		//}
	}
}
