using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class Inventory : MonoBehaviour {

	private const int SLOTS = 12;

	private List<IInventoryItem> mItems = new List<IInventoryItem> (); 

	public static event EventHandler<InventoryEventArgs> ItemAdded; 

	public void AddItem(IInventoryItem item){

		if (mItems.Count < SLOTS) {

			Collider collider = (item as MonoBehaviour).GetComponent<Collider> (); 

			if (collider.enabled) {
				collider.enabled = false; 
				mItems.Add (item); 
				item.OnPickup (); 

				if (ItemAdded != null) {
					ItemAdded (this, new InventoryEventArgs (item)); 
				}
			}
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
