using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour {
	[Header("From The Card")]
	public int startingSpeed;
	public int startingMight;
	public int startingSanity;
	public int startingWisdom;
	public string playerName;
	public string height;
	public string[] hobbies;
	public string birthday;

	[Header("Behind The Scenes")]
	public int speed;
	public int might;
	public int sanity;
	public int wisdom;
	public int[] ladderSpeed;
	public int[] ladderMight;
	public int[] ladderSanity;
	public int[] ladderWisdom;
	public List<GameObject> items;
	public Vector3 newPosition;

	public int x;
	public int z;

	private void Awake()
	{
		speed = ladderSpeed[startingSpeed];
		might = ladderMight[startingMight];
		sanity = ladderSanity[startingSanity];
		wisdom = ladderWisdom[startingWisdom];
	}
	public void setPosition(int _x, int _z)
	{
		x = _x;
		z = _z;
	}
	public Coord getCoords()
	{
		return new Coord(x, z);
	}
	public int getSpeed()
	{
		return ladderSpeed[speed];
	}
	public int getMight()
	{
		return ladderMight[might];
	}
	public int getSanity()
	{
		return ladderSanity[sanity];
	}
	public int getWisdom()
	{
		return ladderWisdom[wisdom];
	}
	public void UpdateXZ()
	{
		x = (int)newPosition.x;
		z = (int)newPosition.z;
	}
}
