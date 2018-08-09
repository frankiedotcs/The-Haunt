using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class getPrefab : MonoBehaviour {
    /// <summary>
    /// Frankie's Prefab Script
    /// </summary>
	// Use this for initialization
	void Start () {
        RoomPrefab.LoadAll("Prefabs");
	}
	
	// Update is called once per frame
	void Update () {

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "AbandonRoom") {

            Instantiate(RoomPrefab.getPrefab("AbandonRoom"));
        }
	}
}
