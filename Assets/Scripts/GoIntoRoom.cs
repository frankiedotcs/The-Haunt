using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoIntoRoom : MonoBehaviour {

	public int zoom; 				//for how much we zoom in to
	public int normal; 				//the normal zoom of the camera position, our current camera position
	public float smooth; 			//how smooth the camera zooms
	public static GoIntoRoom instance;
	public bool needToZoomIn;
	public bool needToZoomOut;
	public bool isZoomed;
	GameManager gameManager; 
	public bool inPuzzleRoom; 
	private int count;
	public bool amZoomingRightNow;

	void Awake(){
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	//use this for initialization
	void Start(){
		gameManager = GameManager.instance; 		//get the GameManager script
		inPuzzleRoom = false; 
		count = 0; 
	}

	//Update is called once per frame
	void Update(){
		//if the "Space bar" is pressed, then the player can zoom into the 3D room
		/*
		if (Input.GetKeyDown (KeyCode.Space) && !needToZoomIn && !needToZoomOut) {
			count += 1; 
			if (count == 1) {
				if (!isZoomed) {
					DontDestroyOnLoad (gameObject); 
					needToZoomIn = true;
					StartCoroutine (LoadPuzzleRoom1 ()); 	//this starts the LoadPuzzleRoom1() function to load in to the 3D room
				} else {
					needToZoomOut = true;
				}
			}

			if (count == 2) {
				if (!isZoomed) {
					DontDestroyOnLoad (gameObject); 
					needToZoomIn = true;
					StartCoroutine (LoadPuzzleRoom2 ()); 	//this starts the LoadPuzzleRoom1() function to load in to the 3D room
				} else {
					needToZoomOut = true;
				}
			}
		}*/

		//once the "Space bar" has been selected, isZoomed will go to true, and the zooming begins
		if (needToZoomIn) {
			isZoomed = true;
			//the camera zoom will follow the player
			transform.LookAt (gameManager.getCurrentPlayer().transform.position + new Vector3(0.5f, 0.0f, 0.5f)); 																		//use the LookAt() method in order to zoom in to the room the player is currently on						
			transform.position += Vector3.up * 0.1f; 														//this is used to set the camer from a top-down perspective, otherwise, it zooms in from the side
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoom, Time.deltaTime * smooth);	//this allows for a smooth zoom
																		
		}
		else if (needToZoomOut)
		{
			isZoomed = false;
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, normal, Time.deltaTime * smooth); 	//leave the camera at it's normal position, no zooming in to room
		}
	}

	IEnumerator LoadPuzzleRoom1(){
		
		yield return new WaitForSeconds (3.0f);     //wait a few seconds for the zoom to complete
		// Application.LoadLevel ("PuzzleRoom1"); 			//load the 3D room scene
		//SceneManager.LoadScene("PuzzleRoom1"); 
		GameManager.instance.LoadRoom("PuzzleRoom1");
		needToZoomIn = false;
		needToZoomOut = false;
		inPuzzleRoom = true; 
	
	}

	IEnumerator LoadPuzzleRoom2(){

		yield return new WaitForSeconds (3.0f);     //wait a few seconds for the zoom to complete
		// Application.LoadLevel ("PuzzleRoom1"); 			//load the 3D room scene
		//SceneManager.LoadScene("PuzzleRoom2");  
		GameManager.instance.LoadRoom("PuzzleRooom2");
		needToZoomIn = false; 
		needToZoomOut = false;
		inPuzzleRoom = true; 

	}


}