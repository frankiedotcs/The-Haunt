using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightFlashing : MonoBehaviour {



	//ARRAYS OF OBjECTS
	public GameObject[] boxes; 
	public GameObject[] barrels; 
	public GameObject[] pallets; 
	public GameObject[] restOfObjects; 
	public GameObject[] secondSetOfObjects; 


	public GameObject key; 
	public GameObject enemy; 
	public GameObject player; 


	//PANELS
	public GameObject hurtPanel; 
	public GameObject introPanel; 
	public Image image1; 

	//BOOLEANS
	private bool needToAttack; 
	private bool dontNeedToAttack; 
	private bool attackPlayer; 

	//OTHER
	private float x; 
	private float y; 
	private float z; 
	private int randomAttack = 2; 
	private int counter; 
	private int secondCount; 
	Vector3 pos; 

	// Use this for initialization
	void Start () {
		
		counter = 0; 					//start our counter at 0
		image1.enabled = false; 		//set our dark-out image to false initially

		x = Random.Range(-25, 26);		//get random ranges for the position of our objects
		y = 0.5f;
		z = Random.Range(-25, 26);
		pos = new Vector3 (x, y, z);	//set as a position

		attackPlayer = false; 			//set our attackPlayer to false initially, we don't want to attack the player yet
		needToAttack = false; 			//set our needToAttack to false initially, we don't want to attack the player yet
		dontNeedToAttack = true; 

		hurtPanel.SetActive (false);	//set our panels to false initially
		introPanel.SetActive (false); 

		secondCount = 0; 

	}

	// Update is called once per frame
	void Update () {
		
		counter++; 

		if(counter >= 250) {

			//start our darkout image
			image1.enabled = true; 

			//turn off our hit panel
			hurtPanel.SetActive (false);
		
			if (counter == 411) {

				int attackNumber = Random.Range (1, 4); 	//create a random set of number and if it becomes 2, then the enemy in the room will atack
		
				//attack the player
				if (randomAttack == attackNumber) {
					needToAttack = true; 
				} else {
					dontNeedToAttack = false; 
				}


				//create a new position
				x = Random.Range (-25, 26);
				y = 4.6f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if (checkIfPosEmpty (pos)) {

					if (needToAttack.Equals (true)) {
						attackPlayer = true; 

					} else if (needToAttack.Equals (false)) {
						enemy.transform.position = pos;		//set to random position
						attackPlayer = false; 
						hurtPanel.SetActive (false);

					}

					if (attackPlayer.Equals (true)) {
						enemy.transform.position = player.transform.position; 	//set to the players position
						needToAttack = false; 
						dontNeedToAttack = true; 

						hurtPanel.SetActive (true);		//activate hurt panel

					}
				}
			
			
			}



			//****************** MOVE OBJECTS RANDOMLY ***************************//
			if (counter == 307) {
				x = Random.Range (-25, 26);
				y = 4.6f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					key.transform.position = pos;
				}

			}
		
			if (counter == 400) {
				x = Random.Range (-25, 26);
				y = 0.5f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					boxes [0].transform.position = pos;
					boxes [1].transform.position = pos; 
					boxes [2].transform.position = pos;
				}

			}
			if (counter == 410) {
				x = Random.Range (-25, 26);
				y = 0.5f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					pallets [0].transform.position = pos;
					pallets [1].transform.position = pos; 
					pallets [2].transform.position = pos;
				}
			}
			if (counter == 415) {
				x = Random.Range (-25, 26);
				y = 2.13f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					barrels [0].transform.position = pos;
				}

			}
			if (counter == 417) {
				x = Random.Range (-25, 26);
				y = 0.36f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					barrels [1].transform.position = pos;
				}

			}
			if (counter == 419) {
				x = Random.Range (-25, 26);
				y = 2.05f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					barrels [3].transform.position = pos;
				}

			}
			if (counter == 422) {
				x = Random.Range (-25, 26);
				y = 1.32f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					barrels [3].transform.position = pos;
				}

			}
			if (counter == 424) {
				x = Random.Range (-25, 26);
				y = 0.36f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				barrels[4].transform.position = pos;

			}


			if (counter == 430) {
				x = Random.Range (-25, 26);
				y = 0.5f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					boxes [3].transform.position = pos;
					boxes [4].transform.position = pos; 
					boxes [5].transform.position = pos;
				}
			}
			if (counter == 440) {
				x = Random.Range (-25, 26);
				y = 0.5f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					pallets [3].transform.position = pos;
					pallets [4].transform.position = pos; 
				}
			}

			if (counter == 443) {
				x = Random.Range (-25, 26);
				y = 0.5f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [0].transform.position = pos; 
				}
			}
			if (counter == 446) {
				x = Random.Range (-25, 26);
				y = 1.23f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [1].transform.position = pos; 
				}
			}
			if (counter == 450) {
				x = Random.Range (-25, 26);
				y = 2.78f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [2].transform.position = pos; 
				}

			}
			if (counter == 353) {
				x = Random.Range (-25, 26);
				y = 2.86f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [3].transform.position = pos; 
				}
			}
			if (counter == 356) {
				x = Random.Range (-25, 26);
				y = 1.09f; 
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [4].transform.position = pos; 
				}
			}
			if (counter == 360) {
				x = Random.Range (-25, 26);
				y = 0.5f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [5].transform.position = pos; 
				}
			}
			if (counter == 363) {
				x = Random.Range (-25, 26);
				y = 0.5f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [6].transform.position = pos; 
				}
			}
			if (counter == 366) {
				x = Random.Range (-25, 26);
				y = 0.5f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [7].transform.position = pos; 
				}
			}
			if (counter == 370) {
				x = Random.Range (-25, 26);
				y = 1.4f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [8].transform.position = pos; 
				}
			}
			if (counter == 373) {
				x = Random.Range (-25, 26);
				y = 1.26f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [9].transform.position = pos; 
				}
			}
			if (counter == 376) {
				x = Random.Range (-25, 26);
				y = 0.91f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [10].transform.position = pos; 
				}
			}
			if (counter == 380) {
				x = Random.Range (-25, 26);
				y = 0.96f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [11].transform.position = pos; 
				}
			}
			if (counter == 383) {
				x = Random.Range (-25, 26);
				y = 0.86f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [12].transform.position = pos; 
				}
			}
			if (counter == 386) {
				x = Random.Range (-25, 26);
				y = 0.5f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [13].transform.position = pos; 
				}
			}
			if (counter == 390) {
				x = Random.Range (-25, 26);
				y = 1.21f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [14].transform.position = pos; 
				}
			}
			if (counter == 393) {
				x = Random.Range (-25, 26);
				y = 1.01f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					restOfObjects[15].transform.position = pos; 
				}
			}

			if (counter == 243) {
				x = Random.Range (-25, 26);
				y = 0.5f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [0].transform.position = pos; 
				}
			}
			if (counter == 246) {
				x = Random.Range (-25, 26);
				y = 2.9f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [1].transform.position = pos; 
				}
			}
			if (counter == 250) {
				x = Random.Range (-25, 26);
				y = 0.2f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [2].transform.position = pos; 
				}

			}
			if (counter == 253) {
				x = Random.Range (-25, 26);
				y = 0.43f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [3].transform.position = pos; 
				}
			}
			if (counter == 256) {
				x = Random.Range (-25, 26);
				y = 0.43f; 
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [4].transform.position = pos; 
				}
			}
			if (counter == 260) {
				x = Random.Range (-25, 26);
				y = 0.43f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [5].transform.position = pos; 
				}
			}
			if (counter == 263) {
				x = Random.Range (-25, 26);
				y = 2.13f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [6].transform.position = pos; 
				}
			}
			if (counter == 266) {
				x = Random.Range (-25, 26);
				y = 0.43f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [7].transform.position = pos; 
				}
			}
			if (counter == 270) {
				x = Random.Range (-25, 26);
				y = 0.43f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [8].transform.position = pos; 
				}
			}
			if (counter == 273) {
				x = Random.Range (-25, 26);
				y = 2.78f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [9].transform.position = pos; 
				}
			}
			if (counter == 276) {
				x = Random.Range (-25, 26);
				y = 0.36f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [10].transform.position = pos; 
				}
			}
			if (counter == 280) {
				x = Random.Range (-25, 26);
				y = 2.05f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [11].transform.position = pos; 
				}
			}
			if (counter == 583) {
				x = Random.Range (-25, 26);
				y = 0.36f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [12].transform.position = pos; 
				}
			}
			if (counter == 286) {
				x = Random.Range (-25, 26);
				y = 0.36f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					secondSetOfObjects [13].transform.position = pos; 
				}
			}
			if (counter == 290) {
				x = Random.Range (-25, 26);
				y = 0.43f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);
				if (checkIfPosEmpty (pos)) {
					restOfObjects [14].transform.position = pos; 
				}
			}
			if (counter == 293) {
				x = Random.Range (-25, 26);
				y = 0.43f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					secondSetOfObjects[15].transform.position = pos; 
				}
			}
			if (counter == 300) {
				x = Random.Range (-25, 26);
				y = 0.43f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					secondSetOfObjects[16].transform.position = pos; 
				}
			}
			if (counter == 320) {
				x = Random.Range (-25, 26);
				y = 1.4f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					secondSetOfObjects[17].transform.position = pos; 
				}
			}
			if (counter == 324) {
				x = Random.Range (-25, 26);
				y = 1.26f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					secondSetOfObjects[18].transform.position = pos; 
				}
			}
			if (counter == 325) {
				x = Random.Range (-25, 26);
				y = 0.91f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					secondSetOfObjects[19].transform.position = pos; 
				}
			}
			if (counter == 330) {
				x = Random.Range (-25, 26);
				y = 0.96f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					secondSetOfObjects[20].transform.position = pos; 
				}
			}
			if (counter == 335) {
				x = Random.Range (-25, 26);
				y = 1.21f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					secondSetOfObjects[21].transform.position = pos; 
				}
			}
			if (counter == 340) {
				x = Random.Range (-25, 26);
				y = 1.21f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					secondSetOfObjects[22].transform.position = pos; 
				}
			}
			if (counter == 344) {
				x = Random.Range (-25, 26);
				y = 0.4f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					secondSetOfObjects[23].transform.position = pos; 
				}
			}
			if (counter == 346) {
				x = Random.Range (-25, 26);
				y = 0.4f;
				z = Random.Range (-25, 26);
				pos = new Vector3 (x, y, z);

				if(checkIfPosEmpty(pos)){
					secondSetOfObjects[24].transform.position = pos; 
				}
			}

		}
		if (counter >= 450) {
			 
			image1.enabled = false; 
			if (secondCount == 0) {
				introPanel.SetActive (true); 
				Time.timeScale = 0; 
			}
			if (secondCount >= 1) {
				
				introPanel.SetActive (false); 
			}
				

			if (attackPlayer.Equals (true)) {
				hurtPanel.SetActive (true);
			}
			counter = 0; 
		
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			introPanel.SetActive (false); 
			Time.timeScale = 1; 
			secondCount += 1; 
		}



	}

	//check if position has already been taken
	public bool checkIfPosEmpty(Vector3 targetPos)
	{
		GameObject[] allMovableThings = GameObject.FindGameObjectsWithTag("move");
		foreach(GameObject current in allMovableThings)
		{
			if(current.transform.position == targetPos)
				return false;
		}
		return true;
	}
}
