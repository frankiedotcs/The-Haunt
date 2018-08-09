using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logging : Object {
	public class Level : Object
	{
		public string levelName = "";
		public int debugLevel = 0;
		public Level(string name, int level)
		{
			debugLevel = level;
			levelName = name;
		}
	}
	List<string> levels = new List<string>();
	public Logging()
	{
		levels.Add("DEBUG3");
		levels.Add("DEBUG2");
		levels.Add("DEBUG");
		levels.Add("INFO");
		levels.Add("INFO");
	}
	public void SetLevel(int level)
	{

	}
}
