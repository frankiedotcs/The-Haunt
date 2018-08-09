using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBox : MonoBehaviour, IInventoryItem {


	public string Name {
		get{
			return "Puzzle Box";
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


}
