using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerTileHighlight : MonoBehaviour
{
	
		private Material material;											//used to make the highlight color green
		public  Color originalColor; 										//used to make the material back to the original material at the start
		public List<Tile> newListToHighlight = new List<Tile>(); 			//used to highlight all the selected tiles within the range of the player to highlight
		public List<Tile> newListToNotHighlight = new List<Tile>(); 		//used to NOT highlight all the selected tiles NOT within the range of the player to move
		

	void Start()
	{
		
		//Fetch the Material from the Renderer of the GameObject
		material = GetComponent<Renderer>().material;

		//Fetch the originalColor of the materials at the start
		originalColor = GetComponent<Renderer> ().material.color;  



	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A)){
			//Destroy GameObject
			Destroy(gameObject);
		}
		/*
		//if our flag was set to true
		if (GameManager.Highlight.highlightAvailableTiles == true) {

			//variable that stores our tilesToSend list
			newListToHighlight = GameManager.Highlight.tilesToHighlight;


			//variable that stores our tiles that we are not going to highlight
			//newListToNotHighlight = GameManager.Highlight.tilesToNotHighlight;
		

			foreach( Tile newTile in newListToHighlight){
				//highlight every tile in this list to green, because this is a moveable place for the player

				newTile.GetComponent<MeshRenderer> ().material.color = Color.green; 
				//Debug.Log("Tiles we've just highlighted: " + newTile); 
				 

			}


				//foreach (Tile notTile in newListToNotHighlight) {
					//notTile.GetComponent<MeshRenderer> ().material.color = Color.white; 
					///Debug.Log("Tiles have NOT just highlighted: " + notTile); 
				//}


				
		}*/

		/*
		//if our flag was set to true
		if (GameManager.Highlight.highlightAvailableTiles == false) {

			//variable that stores our tilesToSend list
			newListToHighlight = GameManager.Highlight.tilesToHighlight;

			foreach( Tile newTile in newListToHighlight){
				
				//highlight every tile in this list to green, because this is a moveable place for the player
				newTile.GetComponent<MeshRenderer> ().material.color = Color.white; 

			}


		}*/
			
	}
		

	void OnCollisionEnter (Collision collider){

		//when the player moves the player piece on to the tile, then highlight the tile to yellow to let the player know they are on that tile
		if (collider.gameObject.tag == "Player") {
			material.color = Color.yellow;				//highlight tile to yellow

		} else {
			material.color = originalColor;		//otherwise, go back to the original material/color

		}

	}

	void OnCollisionExit (Collision collider){

		//once the player leaves the tile, go back to the original material/color
		if (collider.gameObject.tag == "Player") {
			material.color = originalColor;  
		}
	}

	void OnDestroy()
	{
		//Destroy the instance
		Destroy(material);

	}
}