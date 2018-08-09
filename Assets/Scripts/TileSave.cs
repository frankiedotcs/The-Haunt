using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileSave {
	public string name;
	public Vector3 position;
	public Quaternion rotation;
	public TileSave(GameObject go)
	{
		Tile tile = go.GetComponent<Tile>();
		position = go.transform.position;
		rotation = go.transform.rotation;
	}
}
