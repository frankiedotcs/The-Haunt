using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKey : MonoBehaviour, IInventoryItem {
	//use to move the doors back to their original position once the skeleton key has been picked up
	public GameObject door1; //(0.26, 0.2242, -4.632)
	public GameObject door2; //(-4.9991, 0.1957, -0.123)
	public GameObject door3; //(0.23, 0.2728, 4.5857)
	public GameObject door4; //(4.795, 0.1957, -0.123)
	public AudioClip skeletonKey; 		//when the player comes in contact with the Skeleton Key sound



	public string Name {
		get{
			return "Skeleton Key";
		}
	}

	public Sprite _Image = null; 

	public Sprite Image{

		get{
			return _Image; 
		}
	}

	public void OnPickup(){
		gameObject.SetActive (false); 
		//play breaking wood sound


	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Player") {
			
			door1.transform.position = new Vector3 (15.0f, 0.6f, -30.0f);
			door2.transform.position = new Vector3 (-30.0f, 0.6f, -30.0f);
			door3.transform.position = new Vector3 (15.0f, 0.6f, 30.0f);
			door4.transform.position = new Vector3 (30.0f, 0.6f, -30.0f);

			//play Skeleton Key sound
			GetComponent<AudioSource> ().PlayOneShot (skeletonKey); 
		}
	}
}
