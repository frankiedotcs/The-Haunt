using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	public AudioSource puzzleBoxAudio; 


	// Use this for initialization
	void Start () {
		puzzleBoxAudio = GetComponent<AudioSource> (); 
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 1, 0); 
	}

	void OnCollisionEnter(Collision collision){

		if (collision.gameObject.name == "Player") {
			puzzleBoxAudio.Play (); 
			Debug.Log ("in here"); 
		}
	}
}
