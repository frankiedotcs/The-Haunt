using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {

	Rigidbody lamp; 						//the lamp game object that the player needs to tip over to destroy the phone booth
	Transform newLamp; 						//the new position the lamp will be in once the player has knocked it over
	private float angle; 					//the angle of which the lamp will fall over
	private float targetAngle; 				//the end angle of which the lamp will have fallen to
	private float startTime; 				//just need this to calculate the angle
	public GameObject brokenWood; 			//the broken wood game object that will get instantiated when the phone booth is destroyed
	public GameObject phone; 				//the phone booth game object
	public GameObject puzzleBox; 			//the puzzle box pick up when the player destroys the phone booth
	private int count; 						//the count for how many times the player hits the lamp
	public AudioSource phoneAudio; 			//the ringing of the phone
	public AudioClip breakingWoodAudio; 	//the breaking wood sound
	public AudioClip hittingLampAudio; 		//hitting the lamp sound
	public GameObject inventoryPanel; 
	public Animation lampAnim; 
	private bool playAnim; 
	private bool lampHitSound; 


	// Use this for initialization
	void Start () {
		lamp = GetComponent<Rigidbody> (); 						//get the Rigidbody of the lamp
		newLamp = GetComponent<Transform> (); 					//get the transform of the new lamp position
		angle = transform.eulerAngles.y; 						//our angle is going to be the y-axis angle
		count = 0; 												//set our count to 0 initially
		phoneAudio = GetComponent<AudioSource> (); 				//get the audio source of the phone booth
		lampAnim.gameObject.GetComponent<Animator> ().enabled = false; 
		playAnim = true; 
		lampHitSound = true; 
	}

	// Update is called once per frame
	void Update () {

		lampAnim.Stop ();
	}


	private void OnCollisionEnter(Collision collision)
	{
		//if the player hits the lamp
		if (collision.gameObject.name == "Player")
		{
			
			if (playAnim == true) {
				lampAnim.gameObject.GetComponent<Animator> ().enabled = true; 
				StartCoroutine (WaitToPlayAnimation ()); 
			}
			if (playAnim == false) {
				lampAnim.gameObject.GetComponent<Animator> ().enabled = false;
			}

			if (lampHitSound == true) {
				//play hitting the lamp sound
				GetComponent<AudioSource> ().PlayOneShot (hittingLampAudio); 

			}

			//start the count
			count++; 
			Debug.Log ("count: " + count); 

			//start our time
			startTime = Time.time; 

			//move the lamp slightly towards the phone booth every time the player hits it
			//lamp.transform.Translate (new Vector3 (0, -8, 0) * Time.deltaTime); 

			//once our player has hit the lamp 15 times
			if (count == 25) {
				playAnim = false; 
				lampHitSound = false; 
				lampAnim.gameObject.GetComponent<Animator> ().enabled = false; 
				//stop the phone ringing
				phoneAudio.Stop();


				//knock the lamp over
				angle = Mathf.LerpAngle ((float)Time.time, (float)90.0, (float)((Time.time - startTime) / 10.0)); 

				//the final fall over of the lamp
				transform.rotation = Quaternion.Euler ((float)0, (float)0, (float)(angle) * 10); 


				//play breaking wood sound
				GetComponent<AudioSource> ().PlayOneShot (breakingWoodAudio); 

				//destroy the phonebooth
				Destroy (phone); 



				//instantiate piles of broken wood
				Instantiate (brokenWood, new Vector3 (-15.0f, 0.5f, 25.0f), Quaternion.identity); 
				Instantiate (brokenWood, new Vector3 (-17.0f, 0.5f, 27.0f), Quaternion.identity); 
				Instantiate (brokenWood, new Vector3 (-13.0f, 0.5f, 29.0f), Quaternion.identity); 
				Instantiate (brokenWood, new Vector3 (-15.0f, 0.5f, 29.0f), Quaternion.identity); 
			
				inventoryPanel.SetActive (true); 
				//instantiate our pick up, which will be the puzzle box
				Instantiate (puzzleBox, new Vector3 (-15.0f, 6.0f, 18.0f), Quaternion.identity); 

			} 
		
		}
	}

	IEnumerator WaitToPlayAnimation(){

		yield return new WaitForSeconds (1); 
		lampAnim.gameObject.GetComponent<Animator> ().enabled = false; 
	}
}
