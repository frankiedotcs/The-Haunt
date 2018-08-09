using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Inventory inventory; 


	// Use this for initialization
	void Start () {
		 Inventory.ItemAdded += InventoryScript_ItemAdded; 

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e){
		 Transform inventoryPanel = transform.Find ("Inventory"); 

		foreach (Transform slot in inventoryPanel) {

			//Image image = slot.GetChild (0).GetChild (0).GetComponent<Image> (); 
			Image image = slot.GetComponent<Image> (); 
			 
			//if (!image.enabled) {
				image.enabled = true; 
				image.sprite = e.Item.Image; 
				Debug.Log ("image: " + image.enabled);
				break; 
			//}
		}
	}
}
