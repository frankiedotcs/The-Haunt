using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : Object
{
	public int[,] graph;
	public List<Coord> coordList = new List<Coord>();
	public int GetIndex(Coord c)
	{
		for (int a = 0; a < coordList.Count; a++)
		{
			if (c == coordList[a])
				return a;
		}
		return -1;
	}
	public Dijkstra()
	{
		GameObject[,] tiles = GameManager.instance.tiles;
		for (int x = 0; x <  tiles.GetLength(0); x++)
		{
			for (int z = 0; z < tiles.GetLength(1); z++)
			{
				if (tiles[x, z] != null)
				{
					if (GetIndex(new Coord(x, z)) == -1)
						coordList.Add(new Coord(x, z));
					foreach (string dir in new string[] { "n", "e", "s", "w" })
					{
						int _x = x;
						int _z = z;
						Tile _t = tiles[x, z].GetComponent<Tile>();

						if (dir == "n" && _t.doorN)
							_z = z + 1;
						if (dir == "e" && _t.doorE)
							_x = x + 1;
						if (dir == "s" && _t.doorS)
							_z = z - 1;
						if (dir == "w" && _t.doorW)
							_x = x - 1;
						if (GetIndex(new Coord(_x, _z)) == -1)
							coordList.Add(new Coord(_x, _z));
					}
					Tile thisTile = tiles[x, z].GetComponent<Tile>();
					foreach (GameObject go in thisTile.specialMoves)
					{
						string specialName = go.GetComponent<Tile>().roomName;
						for (int _x = 0; _x < 100; _x++)
						{
							for (int _z = 0; _z < 100; _z++)
							{
								if (tiles[_x, _z] != null && tiles[_x, _z].GetComponent<Tile>().roomName == specialName)
								{
									coordList.Add(new Coord(_x, _z));
								}
							}
						}
					}
				}
			}
		}
		graph = new int[coordList.Count, coordList.Count];
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int z = 0; z < tiles.GetLength(1); z++)
			{
				if (tiles[x, z] != null)
				{
					int index = GetIndex(new Coord(x, z));
					//Debug.Log("Index:  " + index);
					foreach (string dir in new string[] { "n", "e", "s", "w" })
					{
						int _x = x;
						int _z = z;
						Tile _t = tiles[x, z].GetComponent<Tile>();

						if (dir == "n" && _t.doorN)
							_z = z + 1;
						if (dir == "e" && _t.doorE)
							_x = x + 1;
						if (dir == "s" && _t.doorS)
							_z = z - 1;
						if (dir == "w" && _t.doorW)
							_x = x - 1;
						int index2 = GetIndex(new Coord(_x, _z));
						//Debug.Log("Index2: " + index2);
						graph[index, index2] = 1;
					}
					Tile thisTile = tiles[x, z].GetComponent<Tile>();
					foreach (GameObject go in thisTile.specialMoves)
					{
						string specialName = go.GetComponent<Tile>().roomName;
						for (int _x = 0; _x < 100; _x++)
						{
							for (int _z = 0; _z < 100; _z++)
							{
								if (tiles[_x, _z] != null && tiles[_x, _z].GetComponent<Tile>().roomName == specialName)
								{
									int index2 = GetIndex(new Coord(_x, _z));
									graph[index, index2] = 1;
								}
							}
						}
					}
				}
			}
		}
	}
	public List<Coord> GetCoordPath(Coord startCoord, Coord targetCoord)
	{
		List<Coord> path = new List<Coord>();
		foreach (int a in GetPath(startCoord, targetCoord))
		{
			path.Add(coordList[a]);
		}
		return path;
	}
	public List<int> GetPath(Coord startCoord, Coord targetCoord)
	{
		LinkedList<int> path = new LinkedList<int>();

		int graphSize = graph.GetLength(0);
		int[] distance = new int[graphSize];
		for (int a = 0; a < graphSize; a++)
			distance[a] = int.MaxValue;

		distance[GetIndex(startCoord)] = 0;		// Distance from starting TO starting is 0

		bool[] used = new bool[graphSize];
		int?[] previous = new int?[graphSize];

		while (true)
		{
			int minDistance = int.MaxValue;
			int minNode = 0;
			for (int a = 0; a < graphSize; a++)
			{
				if (!used[a] && distance[a] < minDistance)
				{
					minDistance = distance[a];
					minNode = a;
				}
			}

			if (minDistance == int.MaxValue)
				break;

			used[minNode] = true;

			for (int a=0; a<graphSize; a++)
			{
				if (graph[minNode, a] > 0)
				{
					int shortestToMinNode = distance[minNode];
					int distanceToNextNode = graph[minNode, a];

					int totalDistance = shortestToMinNode + distanceToNextNode;

					if (totalDistance < distance[a])
					{
						distance[a] = totalDistance;
						previous[a] = minNode;
					}
				}
			}
		}

		int index = GetIndex(targetCoord);

		//Debug.Log("index: " + index);
		//Debug.Log("dl: " + distance.Length);
		if (index >= distance.Length || index == -1 || distance[index] == int.MaxValue)
			return null;

		path = new LinkedList<int>();

		int? currentNode = GetIndex(targetCoord);
		 while (currentNode != null)
		{
			path.AddFirst(currentNode.Value);
			currentNode = previous[currentNode.Value];
		}
		return ToList(path);
	}
	public void PrintList()
	{
		foreach (Coord c in coordList)
		{
			Debug.Log(c.ToString());
		}
	}
	public void PrintGraph()
	{
		for (int a = 0; a < graph.GetLength(0); a++)
		{
			for (int b = 0; b < graph.GetLength(1); b++)
			{
				if (graph[a, b] != 0)
				{
					Debug.Log("Path: " + coordList[a] + "=>" + coordList[b] + " w:" + graph[a, b]);
				}
			}
		}
	}
	public void PrintPath(Coord startCoord, Coord finishCoord)
	{
		//Debug.Log("Shortest Path: " + startCoord + " -> " + finishCoord);
		List<int> path = GetPath(startCoord, finishCoord);
		if (path == null)
		{
			Debug.Log("No Path");
			return;
		}
		else
		{
			List<Coord> coordPath = new List<Coord>();
			foreach (int a in path)
				coordPath.Add(coordList[a]);

			int pathLength = 0;
			for (int a=0; a<path.Count - 1; a++)
			{
				pathLength += graph[path[a], path[a + 1]];
			}

			string formattedPath = "";
			bool isFirst = true;
			foreach (int pos in path)
			{
				if (!isFirst)
					formattedPath += "->";
				isFirst = false;
				formattedPath += coordList[pos];
			}

			Debug.Log(formattedPath + " (length " + pathLength + ")");
		}
	}
	public List<int> ToList(LinkedList<int> inList)
	{
		List<int> outList = new List<int>();
		foreach (int num in inList)
		{
			outList.Add(num);
		}
		return outList;
	}
}
