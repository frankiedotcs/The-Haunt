using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidicCharm : MonoBehaviour, IInventoryItem {


	public string Name {
		get{
			return "Druidic Charm";
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
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
