using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Coord {
	public int x;
	public int z;
	public Coord(int _x, int _z)
	{
		x = _x;
		z = _z;
	}
	public override string ToString()
	{
		return "(" + x.ToString() + "," + z.ToString() + ")";
	}
	public static bool operator==(Coord a, Coord b)
	{
		if (a.x == b.x && a.z == b.z)
			return true;
		return false;
	}
	public static bool operator!=(Coord a, Coord b)
	{
		if (a.x != b.x || a.z != b.z)
			return true;
		return false;
	}
}
