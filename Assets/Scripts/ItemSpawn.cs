using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour {

    /// <summary>
    /// Frankie's script for the spawn stuff for the Abandoned Room
    /// </summary>
    public GameObject Item;
    bool clue1 = false;
    bool clue2 = false;
    public GameObject Haunt;
    public AudioClip enemy;

    // Use this for initialization
    void Start () {
        Item.SetActive(false);
        Haunt.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "bloodsoakedLetter")
        {
            clue1 = true;
            CheckClues();
        }

        if (collision.gameObject.name == "Letter")
        {
            clue2 = true;
            CheckClues();
        }

        
    }

    public void CheckClues()
    {
        if (clue1 == true && clue2 == true)
        {
            Item.SetActive(true);
            Haunt.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(enemy);
        }
        else
        {
            Item.SetActive(false);
        }
    }
}
