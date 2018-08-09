using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour {
	public string roomName;
	public bool startDoorN;
	public bool startDoorE;
	public bool startDoorS;
	public bool startDoorW;
	[HideInInspector]
	public bool doorN;
	[HideInInspector]
	public bool doorE;
	[HideInInspector]
	public bool doorS;
	[HideInInspector]
	public bool doorW;

	public int direction;
	public bool hasOmen;
	public bool hasEvent;
	public bool hasItem;
	public bool hasItem2;
	public bool usableOnUpper;
	public bool usableOnGround;
	public bool usableInBasement;
	[HideInInspector]
	public int currentFloor;
	public bool hasWindow;
	public bool openAir;
	[HideInInspector]
	public bool displayed;
	public float offsetX;
	public float offsetZ;
	public string transitionTo;

	public Coord currentLocation;

	public GameObject[] specialMoves;

	private void Awake()
	{
		doorN = startDoorN;
		doorE = startDoorE;
		doorS = startDoorS;
		doorW = startDoorW;
	}
	/// <summary>
	/// Rotate the tile and all doors clockwise
	/// </summary>
	public void RotateCW()
	{
		ShiftDoorsCW();
		direction += 90;
		if (direction >= 360)
			direction = 0;
		gameObject.transform.eulerAngles = new Vector3(gameObject.transform.rotation.x, direction, gameObject.transform.rotation.z);
	}
	/// <summary>
	/// Rotate the tile and all doors counter-clockwise
	/// </summary>
	public void RotateCCW()
	{
		ShiftDoorsCCW();
		direction -= 90;
		if (direction < 0)
			direction = 0;
	}
	/// <summary>
	/// Shift doors clockwise
	/// </summary>
	public void ShiftDoorsCW()
	{
		bool _doorN, _doorE, _doorS, _doorW;
		_doorN = doorW;
		_doorE = doorN;
		_doorS = doorE;
		_doorW = doorS;
		doorN = _doorN;
		doorE = _doorE;
		doorS = _doorS;
		doorW = _doorW;
	}
	/// <summary>
	/// Shift doors counter-clockwise
	/// </summary>
	public void ShiftDoorsCCW()
	{
		bool _doorN, _doorE, _doorS, _doorW;
		_doorN = doorE;
		_doorE = doorS;
		_doorS = doorW;
		_doorW = doorN;
		doorN = _doorN;
		doorE = _doorE;
		doorS = _doorS;
		doorW = _doorW;
	}
	/// <summary>
	/// Is there a door in <dir> direction?
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public bool GetDoor(string dir)
	{
		dir = dir.ToLower();
		if (dir == "n")
			return doorN;
		if (dir == "e")
			return doorE;
		if (dir == "s")
			return doorS;
		if (dir == "w")
			return doorW;
		return false;
	}
	/// <summary>
	/// Set the current floor.
	/// </summary>
	/// <param name="floor"></param>
	public void SetFloor(int floor)
	{
		currentFloor = floor;
	}
	/// <summary>
	/// Import settings from another tile (since Unity does not support C# Copy constructors
	/// </summary>
	/// <param name="obj"></param>
	public void Import(GameObject obj)
	{
		Import(obj.GetComponent<Tile>());
	}
	/// <summary>
	/// Import settings from another tile (since Unity does not support C# Copy constructors
	/// </summary>
	/// <param name="obj"></param>
	public void Import(Tile t)
	{
		roomName = t.roomName;
		startDoorN = t.startDoorN;
		startDoorE = t.startDoorE;
		startDoorS = t.startDoorS;
		startDoorW = t.startDoorW;
		doorN = t.doorN;
		doorE = t.doorE;
		doorS = t.doorS;
		doorW = t.doorW;
		direction = t.direction;
		hasOmen = t.hasOmen;
		hasEvent = t.hasEvent;
		hasItem = t.hasItem;
		hasItem2 = t.hasItem2;
		usableOnUpper = t.usableOnUpper;
		usableOnGround = t.usableOnGround;
		usableInBasement = t.usableInBasement;
		currentFloor = t.currentFloor;
		hasWindow = t.hasWindow;
		openAir = t.openAir;
		displayed = t.displayed;
		offsetX = t.offsetX;
		offsetZ = t.offsetZ;
		currentLocation = t.currentLocation;
	}
}
