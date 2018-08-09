using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	/// <summary>
	/// Our Singleton instance
	/// </summary>
	public static GameManager instance;
	/// <summary>
	/// What's our current activity?
	/// </summary>
	//public Stack<string> activity
	public string activity;
	private bool settingUpBoard;
	public bool tilePlacementDebug;

	public GameObject[,] tiles;
	public Stack<GameObject> tileDrawPile;
	public List<GameObject> tileDrawPile2;
	private List<Coord> availableMoves = new List<Coord>();
	private List<Coord> thisMove = new List<Coord>();

	[Header("Tiles")]
	public List<GameObject> startingTilePile;
	public GameObject[] permanentTiles;
	public int entranceStartingX;
	public int entranceStartingZ;
	public int upstairsStartingX;
	public int upstairsStartingZ;
	public int basementStartingX;
	public int basementStartingZ;
	public float tilesStart;
	public float tileHeight;

	[Header("Camera")]
	public Camera cam;
	public float cameraSpeed;
	public float cameraHeight;
	/// <summary>
	/// Use 90 for straight vertical
	/// </summary>
	public float cameraPitchAngle;
	/// <summary>
	/// Set 0 for straight North/South
	/// </summary>
	public float cameraYawAngle;
	public float cameraOffset;
	public Vector3 lastCamPosition;
	public Quaternion lastCamRotation;

	[Header("Placement Tile")]
	private RaycastHit rcHit;
	private int curX;
	private int curZ;
	private int oldX;
	private int oldZ;
	private Ray ray;
	public GameObject tmpTile;
	private int orientation;
	private bool tmpTileChangeMade;
	public float transparentAlpha;
	public float floatingHeight;
	private bool placingTile;

	[Header("HUD")]
	//public Image UITilePreview;
	public RawImage TilePreview;
	public Toggle usableUpstairs;
	public Toggle usableOnGroundFloor;
	public Toggle usableInBasement;
	//public Toggle doorNorth;
	//public Toggle doorEast;
	//public Toggle doorSouth;
	//public Toggle doorWest;
	public Text playerName;
	public Text playerSpeed;
	public Text playerMight;
	public Text playerSanity;
	public Text playerWisdom;
	public Text currentActivity;
	//public Text clickCoords;

	public bool debugging = false;

	[Header("Outlines")]
	public GameObject outline;
	public float highlightY;
	private List<GameObject> outlines = new List<GameObject>();
	private bool outlinesSet;

	[Header("Players")]
	public GameObject[] availablePlayers;	// Holds available players that can be chosen.
	public GameObject[] players;            // Current players are saved here.
	private int currentPlayer;
	public float playerMoveSpeed;
	public float playerModelHeight;
	public float closenessThreshold;
	public int playerSpeedRemaining;
	public int currentMoveCount;
	public Dictionary<Coord, List<Coord>> dijkstra = new Dictionary<Coord, List<Coord>>();
	public bool playerTurn = true;
	public Vector3 oldPosition;

	[Header("Loading Stuff")]
	public GameObject hud;
	public GameObject board;
	//public GameObject cylinder;
	public GameObject mainLight;
	public string gameRoomTransition;
	public bool playing2D;
	public GameObject cam2D;

	private bool GoAheadAndZoom = false;

	public void LoadRoom(string sceneName)
	{
		if (sceneName != "")
		{
			if (GoAheadAndZoom)
				GoIntoRoom.instance.needToZoomIn = true;
			gameRoomTransition = sceneName;
			StartCoroutine(LoadScene());
		}
	}

	public IEnumerator LoadScene()
	{
		if (GoAheadAndZoom)
			yield return new WaitForSeconds(3.0f);
		playing2D = false;
		AsyncOperation ao = SceneManager.LoadSceneAsync(gameRoomTransition, LoadSceneMode.Additive);
		if (!ao.isDone)
			yield return null;
		enableObjects(false);
		if (GoAheadAndZoom)
		{
			GoIntoRoom.instance.needToZoomIn = false;
			GoIntoRoom.instance.needToZoomOut = false;
			GoIntoRoom.instance.inPuzzleRoom = true;
		}
	}

	public void ReturnFromScene()
	{
		enableObjects(true);
		AsyncOperation ao = SceneManager.UnloadSceneAsync(gameRoomTransition);
		playing2D = true;
		cam.transform.position = lastCamPosition;
		cam.transform.rotation = lastCamRotation;
	}

	/// <summary>
	/// Enable or Disable all on-screen objects
	/// </summary>
	/// <param name="setting"></param>
	public void enableObjects(bool setting)
	{
		hud.SetActive(setting);
		board.SetActive(setting);
		//mainLight.SetActive(setting);
		cam2D.SetActive(setting);
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int z = 0; z < tiles.GetLength(1); z++)
			{
				if (tiles[x, z] != null)
					tiles[x, z].SetActive(setting);
			}
		}
		foreach (GameObject go in outlines)
		{
			go.SetActive(setting);
		}
		foreach (GameObject player in players)
		{
			if (player != null)
				player.SetActive(setting);
		}
	}

	public class Path
	{
		public int moves;
		public List<Coord> path = new List<Coord>();
	}

	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Awake () {
		settingUpBoard = true;
		tiles = new GameObject[100, 100];
		//tilesForMovement = new GameObject[100, 100];
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if (instance != this)
		{
			Destroy(gameObject);
		}
		//activity = new Stack<string>();
	}
	/// <summary>
	/// Load starting tiles. Set initial camera position. Create the draw pile.
	/// </summary>
	private void Start()
	{
		
		// Load our Entrance
		PlaceTile(entranceStartingX, entranceStartingZ, permanentTiles[0]);											// Grand staircase
		PlaceTile(entranceStartingX, entranceStartingZ - 1, permanentTiles[1]);										// Foyer
		PlaceTile(entranceStartingX, entranceStartingZ - 2, permanentTiles[2]);										// Entrance Hall
		PlaceTile(entranceStartingX, entranceStartingZ - 3, permanentTiles[3], 0, new Vector3(0.5f, 0.0f, 0.91f));	// Stoop
		tiles[entranceStartingX, entranceStartingZ].GetComponent<Tile>().SetFloor(1);
		tiles[entranceStartingX, entranceStartingZ - 1].GetComponent<Tile>().SetFloor(1);
		tiles[entranceStartingX, entranceStartingZ - 2].GetComponent<Tile>().SetFloor(1);
		tiles[entranceStartingX, entranceStartingZ - 3].GetComponent<Tile>().SetFloor(1);

		// Load Upstairs Landing
		PlaceTile(upstairsStartingX, upstairsStartingZ, permanentTiles[4]);
		tiles[upstairsStartingX, upstairsStartingZ].GetComponent<Tile>().SetFloor(2);

		// Load Basement Landing
		PlaceTile(basementStartingX, basementStartingZ, permanentTiles[5]);
		tiles[basementStartingX, basementStartingZ].GetComponent<Tile>().SetFloor(0);

		// Camera starts at Entrance (for now)
		SendCamToLocation("entrance");

		tileDrawPile = new Stack<GameObject>();
		tileDrawPile2 = new List<GameObject>();

		//setTransparent(testTile, true);
		System.Random _rnd = new System.Random();
		int _index;
		while (startingTilePile.Count > 0)
		{
			_index = _rnd.Next(startingTilePile.Count);
			tileDrawPile.Push(startingTilePile[_index]);
			tileDrawPile2.Insert(0, startingTilePile[_index]);
			startingTilePile.RemoveAt(_index);
		}
		UpdateCamPositioning();
		settingUpBoard = false;
		//Highlight.highlightAvailableTiles = false;

		// Instantiate our player
		//players[1] = availablePlayers[0];	// Setting this up manually.  In the future, it can be chosen.
		players[1] = Instantiate(availablePlayers[0], new Vector3(entranceStartingX, playerModelHeight, entranceStartingZ - 2), Quaternion.Euler(Vector3.zero));
		players[1].GetComponent<Player>().setPosition(entranceStartingX, entranceStartingZ - 2);	// And now update the player's stored position.
		currentPlayer = 1;      // Our current player
		SetActivity("moving player");
		ResetPlayerForMoving();
		updateHUDInfo();
		playing2D = true;
	}
	/// <summary>
	/// Update().  Runs every frame.
	/// </summary>
	private void Update()
	{
		/*if (GoIntoRoom.instance.inPuzzleRoom == true)
		{
			Camera.main.fieldOfView = 60;
			Camera.main.transform.position = new Vector3(49.0f, 4.0f, 57.901f);
			Camera.main.transform.rotation = Quaternion.Euler(50.0f, 30.0f, 0.0f);
			Debug.Log("IN CAMERA ADJUSTMENT");
			GoIntoRoom.instance.inPuzzleRoom = false;
		}*/
		if (playing2D)
		{
			// Only need a single raycast
			ray = cam.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out rcHit))
			{
				// Get the variables in curX, curZ format
				Transform objectHit = rcHit.transform;
				curX = (int)Math.Floor(rcHit.point.x);
				curZ = (int)Math.Floor(rcHit.point.z);
			}

			if (SeeActivity("movement in progress"))
			{
				// Previously not doing anything?
				if (getCurrentPlayer().newPosition != Vector3.zero)
				{
					// New direction!
					float relativeSpeed = Vector3.Distance(oldPosition, getCurrentPlayer().newPosition) / 3;
					if (relativeSpeed <= 3)
						relativeSpeed = 1;
					float step = playerMoveSpeed * Time.deltaTime * relativeSpeed;
					players[currentPlayer].transform.position = Vector3.MoveTowards(players[currentPlayer].transform.position, getCurrentPlayer().newPosition, step);
				}

				if (Vector3.Distance(getCurrentPlayer().newPosition, players[currentPlayer].transform.position) < closenessThreshold)
				{
					// Meh...  close enough.
					SetActivity("idle");
					removeOutlines();
					players[currentPlayer].transform.position = getCurrentPlayer().newPosition;
					getCurrentPlayer().UpdateXZ();      // Update the X, Z based on newPosition (the target of a move)
					getCurrentPlayer().newPosition = Vector3.zero;
					lastCamPosition = cam.transform.position;
					lastCamRotation = cam.transform.rotation;
				}
				return;
			}

			// Not doing anything.  Maybe we should place a tile?
			if (SeeActivity("idle"))
			{
				int _x = getCurrentPlayer().x;
				int _z = getCurrentPlayer().z;
				if (tiles[_x, _z] == null)
				{
					SetActivity("placing tile");
					playerSpeedRemaining = 0;
				}
				else
				{
					if (tiles[getCurrentPlayer().x, getCurrentPlayer().z].GetComponent<Tile>().transitionTo != "")
					{
						if (!GoIntoRoom.instance.needToZoomIn && GoAheadAndZoom)
							GoIntoRoom.instance.needToZoomIn = true;
						LoadRoom(tiles[getCurrentPlayer().x, getCurrentPlayer().z].GetComponent<Tile>().transitionTo);
					}
					SelectNextPlayer();
				}
			}

			// Place a tile.
			if (SeeActivity("placing tile"))
			{
				if (!placingTile)
				{
					float _height = tilesStart + floatingHeight;
					//tmpTile = Instantiate(tileDrawPile.Peek(), new Vector3(curX + 0.5f, _height, curZ + 0.5f), Quaternion.Euler(0.0f, tileDrawPile.Peek().GetComponent<Tile>().direction, 0.0f));
					//tmpTile = Instantiate(tileDrawPile.Peek(), new Vector3(getCurrentPlayer().x + 0.5f, _height, getCurrentPlayer().z + 0.5f), Quaternion.Euler(0.0f, tileDrawPile.Peek().GetComponent<Tile>().direction, 0.0f));

					PushUsableTileToTop();

					tmpTile = Instantiate(tileDrawPile2[0], new Vector3(getCurrentPlayer().x + 0.5f, _height, getCurrentPlayer().z + 0.5f), Quaternion.Euler(0.0f, tileDrawPile2[0].GetComponent<Tile>().direction, 0.0f));
					tmpTile.GetComponent<Tile>().currentLocation = new Coord(getCurrentPlayer().x, getCurrentPlayer().z);
					UpdatePreview();
					tmpTileChangeMade = true;
					placingTile = true;
				}
				else
				{
					if (Input.GetMouseButtonDown(1))
					{
						tmpTile.GetComponent<Tile>().RotateCW();
						tmpTileChangeMade = true;
					}
					if (Input.GetMouseButtonDown(0))
					{
						if (CheckTilePlacement())
						{
							//PlaceTile(curX, curZ, tmpTile, tmpTile.GetComponent<Tile>().direction);
							PlaceTile(getCurrentPlayer().x, getCurrentPlayer().z, tmpTile, tmpTile.GetComponent<Tile>().direction);
							//tileDrawPile.Pop();
							tileDrawPile2.RemoveAt(0);
							Destroy(tmpTile);
							SelectNextPlayer();
							SetActivity("moving player");
						}
					}
				}
			}

			// Ready to move a player.  Move it!
			if (SeeActivity("moving player"))
			{
				if (Input.GetMouseButtonDown(0))
				{
					if (existsInList(new Coord(curX, curZ), availableMoves))
					{
						string _tmp = existsInList(new Coord(curX, curZ), availableMoves).ToString();
						//clickCoords.text = "(" + curX + ", " + curZ + ") " + _tmp;
						currentMoveCount = GetPathLength(new Coord(curX, curZ));
						Debug.Log("Requires " + currentMoveCount + " moves.");
						Debug.Log("You have " + playerSpeedRemaining);
						playerSpeedRemaining -= currentMoveCount;
						Debug.Log("Moves Remaining: " + playerSpeedRemaining);

						getCurrentPlayer().newPosition = new Vector3(curX, playerModelHeight, curZ);
						oldPosition = players[currentPlayer].transform.position;
						SetActivity("movement in progress");
					}
				}
			}
			if (SeeActivity("next player"))
			{
				SelectNextPlayer();
				if (outlinesSet == false)
				{
					outlinesSet = true;
					removeOutlines();
					getMovableTiles();
					outlineMovableTiles();
				}
			}
			// Move cam with keys
			bool camChangeMade = false;
			if (Input.GetKey(KeyCode.RightArrow))
			{
				camChangeMade = true;
				cam.transform.Translate(new Vector3(cameraSpeed * Time.deltaTime, 0, 0));
			}
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				camChangeMade = true;
				cam.transform.Translate(new Vector3(-cameraSpeed * Time.deltaTime, 0, 0));
			}
			if (Input.GetKey(KeyCode.UpArrow))
			{
				camChangeMade = true;
				cam.transform.Translate(new Vector3(0, cameraSpeed * Time.deltaTime, 0));
			}
			if (Input.GetKey(KeyCode.DownArrow))
			{
				camChangeMade = true;
				cam.transform.Translate(new Vector3(0, -cameraSpeed * Time.deltaTime, 0));
			}
			if (camChangeMade)
			{
				// Only hit this if a change has been made.  Saves on frame updates.
				UpdateCamPositioning();
			}

			/*if (Input.GetKeyDown(KeyCode.A))
			{
				AttachAllTilesToGameManager();
			}*/

			// Prints tiles[,] to file for debugging purposes.
			if (Input.GetKeyDown(KeyCode.D))
			{
				string output = "";
				Tile _tile;
				bool _N, _E, _S, _W;
				output = "Current tile: " + curX + ", " + curZ + System.Environment.NewLine;
				output += "     " + curX + "," + (curZ + 1) + System.Environment.NewLine;
				output += (curX - 1) + "," + curZ + "     " + (curX + 1) + "," + curZ + System.Environment.NewLine;
				output += "     " + curX + "," + (curZ - 1) + System.Environment.NewLine;
				for (int a = 0; a < tiles.GetLength(0); a++)
				{
					for (int b = 0; b < tiles.GetLength(1); b++)
					{
						if (tiles[a, b] != null)
						{
							_tile = tiles[a, b].GetComponent<Tile>();
							_N = _tile.doorN;
							_E = _tile.doorE;
							_S = _tile.doorS;
							_W = _tile.doorW;
							int _floor = _tile.currentFloor;
							output += "tiles[" + a + "," + b + "]:";
							output += " N:" + _N;
							output += " E:" + _E;
							output += " S:" + _S;
							output += " W:" + _W;
							output += " F:" + _floor + System.Environment.NewLine;
						}
					}
				}
				System.IO.File.WriteAllText(@"C:\Users\Public\debug.txt", output);
			}

			//go back to main menu
			if (Input.GetKeyDown(KeyCode.Q))
			{
				//Application.LoadLevel("MainMenu");
				SceneManager.LoadScene("MainMenu");
			}

			if (tmpTileChangeMade)
			{
				tmpTileChangeMade = false;
				if (CheckTilePlacement())
				{
					setRed(false);
					setTransparent(tmpTile);
				}
				else
				{
					setRed();
					setTransparent(tmpTile);
				}
				Tile _tile = tmpTile.GetComponent<Tile>();

				usableUpstairs.isOn = _tile.usableOnUpper;
				usableOnGroundFloor.isOn = _tile.usableOnGround;
				usableInBasement.isOn = _tile.usableInBasement;
			}
		}
	}
	public void PushUsableTileToTop()
	{
		if (TileMatchesFloor(tileDrawPile2[0].GetComponent<Tile>(), getCurrentPlayer().getCoords()))
		{
			return;
		}
		for (int a = 0; a < tileDrawPile2.Count; a++)
		{
			if (TileMatchesFloor(tileDrawPile2[a].GetComponent<Tile>(), getCurrentPlayer().getCoords()) && a > 0)
			{
				GameObject _tile = tileDrawPile2[a];
				tileDrawPile2.RemoveAt(a);
				tileDrawPile2.Insert(0, _tile);
				return;
			}
		}
		Debug.Log("Oh crap.  Out of tiles...  le sigh.");
	}

	public bool TileMatchesFloor(Tile tile, Coord c)
	{
		int floor = GetFloor(c);
		if (floor == 0 && tile.usableInBasement)
			return true;
		if (floor == 1 && tile.usableOnGround)
			return true;
		if (floor == 2 && tile.usableOnUpper)
			return true;
		return false;
	}
	public void SelectNextPlayer()
	{
		currentPlayer++;
		if (currentPlayer >= players.Length)
			currentPlayer = 0;
		while (players[currentPlayer] == null)
		{
			currentPlayer++;
			if (currentPlayer > players.Length)
				currentPlayer = 0;
		}
		ResetPlayerForMoving();
		SetActivity("moving player");
		outlinesSet = false;
		if (outlinesSet == false)
		{
			outlinesSet = true;
			removeOutlines();
			getMovableTiles();
			outlineMovableTiles();
		}
	}
	public void AttachAllTilesToGameManager()
	{
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int z = 0; z < tiles.GetLength(1); z++)
			{
				if (tiles[x, z] != null)
				{
					tiles[x, z].transform.parent = GameManager.instance.gameObject.transform;
					tiles[x, z].SetActive(false);
				}
			}
		}
	}
	public void ResetPlayerForMoving()
	{
		playerSpeedRemaining = getCurrentPlayer().getSpeed();
	}
	public void ShowPath(Coord c)
	{
			Dijkstra d = new Dijkstra();
			Player p = players[currentPlayer].GetComponent<Player>();
			List<Coord> path = d.GetCoordPath(new Coord(p.x, p.z), c);
			foreach (Coord _c in path)
			{
				Debug.Log(_c + " => ");
			}
	}
	public List<Coord> GetPath(Coord c)
	{
		Dijkstra d = new Dijkstra();
		Player p = players[currentPlayer].GetComponent<Player>();
		List<Coord> path = d.GetCoordPath(new Coord(p.x, p.z), c);
		return path;
	}
	public int GetPathLength(Coord c)
	{
		Dijkstra d = new Dijkstra();
		Player p = players[currentPlayer].GetComponent<Player>();
		List<Coord> path = d.GetCoordPath(new Coord(p.x, p.z), c);
		return path.Count - 1;
	}
	/// <summary>
	/// Get all possible tiles a player can move to.
	/// </summary>
	public void getMovableTiles()
	{
		Player _player = players[currentPlayer].GetComponent<Player>();			// Current player
		int speed = players[currentPlayer].GetComponent<Player>().getSpeed();	// Player's speed
		moveOne(new Coord(_player.x, _player.z), speed);						// Start our decent into madness
	}
	/// <summary>
	/// Outline all tiles found in "getMovableTiles()"
	/// </summary>
	public void outlineMovableTiles()
	{
		foreach (Coord coord in availableMoves)
		{
			makeOutline(coord.x, coord.z);
		}
	}
	/// <summary>
	/// Recursive function that will move to all possible tiles a player can move to.
	/// </summary>
	/// <param name="c"></param>
	/// <param name="speedRemaining"></param>
	public void moveOne(Coord c, int speedRemaining)
	{
		if (!existsInList(c, availableMoves))
		{
			availableMoves.Add(c);
		}
		int x = c.x;
		int z = c.z;
		if (tiles[x, z] == null)
			return;
		Tile thisTile = tiles[x, z].GetComponent<Tile>();
		foreach (string dir in new List<string> { "n", "e", "s", "w" })
		{
			int newX = x;
			int newZ = z;
			switch (dir)
			{
				case "n":
					if (!thisTile.doorN)
						continue;
					newZ++;
					break;
				case "e":
					if (!thisTile.doorE)
						continue;
					newX++;
					break;
				case "s":
					if (!thisTile.doorS)
						continue;
					newZ--;
					break;
				case "w":
					if (!thisTile.doorW)
						continue;
					newX--;
					break;
			}
			if (speedRemaining > 0)
				moveOne(new Coord(newX, newZ), speedRemaining - 1);
			else
				return;
		}
		foreach (GameObject go in thisTile.specialMoves)
		{
			string specialName = go.GetComponent<Tile>().roomName;
			for (int _x = 0; _x < 100; _x++)
			{
				for (int _z = 0; _z < 100; _z++)
				{
					if (tiles[_x, _z] != null && tiles[_x, _z].GetComponent<Tile>().roomName == specialName) {
						moveOne(new Coord(_x, _z), speedRemaining - 1);
					}
				}
			}
		}
		return;
	}
	/// <summary>
	/// Does the current tile already exist in the list?
	/// </summary>
	/// <param name="coord"></param>
	/// <param name="list"></param>
	/// <returns></returns>
	public bool existsInList(Coord coord, List<Coord> list)
	{
		foreach (Coord c in list)
		{
			if (coord.x == c.x && coord.z == c.z)
				return true;
		}
		return false;
	}
	/// <summary>
	/// Update the tile preview in the TL corner
	/// </summary>
	public void UpdatePreview()
	{
		TilePreview.texture = tmpTile.GetComponent<Renderer>().material.mainTexture;
	}

	/// <summary>
	/// Set transparency of a tile.
	/// </summary>
	/// <param name="tile">The tile to change alpha channel</param>
	/// <param name="transparent">boolean to enable/disable transparency</param>
	private void setTransparent(GameObject tile, bool transparent = true)
	{
		//Debug.Log("Setting transparency.");
		Renderer rend = tile.GetComponent<Renderer>();
		Color oldColor = rend.material.color;
		float newAlpha;
		if (transparent)
		{
			//Debug.Log("Transparent.");
			newAlpha = transparentAlpha;
		}
		else
		{
			//Debug.Log("Not transparent.");
			newAlpha = 1.0f;
		}
		//testTile.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
		//Debug.Log("New alpha: " + newAlpha.ToString());
		rend.material.color = new Color(oldColor.r, oldColor.b, oldColor.g, newAlpha);
	}
	/// <summary>
	/// Set the color of a tile Red (Can't move there)
	/// </summary>
	/// <param name="tile">The tile</param>
	/// <param name="makeRed">true/false for enable/disable respectively</param>
	private void setRed(GameObject tile, bool makeRed = true)
	{
		Color newColor;
		if (makeRed)
			newColor = Color.red;
		else
			newColor = Color.white;
		tile.GetComponent<Renderer>().material.SetColor("_Color", newColor);
	}
	private void setRed(bool makeRed = true)
	{
		Tile tile = tmpTile.GetComponent<Tile>();
		Color newColor;
		if (makeRed)
			newColor = Color.red;
		else
			newColor = Color.white;
		tile.GetComponent<Renderer>().material.SetColor("_Color", newColor);
	}
	public void UpdateCamPositioning()
	{
		// Reset height
		cam.transform.position = new Vector3(cam.transform.position.x, cameraHeight, cam.transform.position.z);

		// Reset pitch/yaw
		cam.transform.eulerAngles = new Vector3(cameraPitchAngle, cameraYawAngle, 0.0f);
	}
	/// <summary>
	/// Check to see if the current spot can accomodate the current tile in its position.
	/// </summary>
	/// <returns></returns>
	private bool CheckTilePlacement()
	{
		Tile tile = tmpTile.GetComponent<Tile>();       // Our currently held tile

		if (TileOccupied(tile.currentLocation))
		{
			if (tilePlacementDebug)
				Debug.Log("Can't place tile: occupied.");
			return false;
		}

		if (AllAdjacentTilesEmpty(tile.currentLocation))
		{
			if (tilePlacementDebug)
				Debug.Log("Can't place tile: Not adjacent to any tiles.");
			return false;
		}

		// Any non-matching doors?
		bool hasOneMatch = false;
		foreach (string dir in new List<string> { "n", "e", "s", "w" })
		{
			var _getdoor = tile.GetDoor(dir);
			var _tileIsNull = !TileIsNull(dir);
			var _tileHasDoor = TileHasDoor(dir);
			if (tile.GetDoor(dir) && !TileIsNull(dir) && !TileHasDoor(dir))
			{
				if (tilePlacementDebug)
					Debug.Log("Can't place tile: Directional door unavailable.");
				return false;
			}
			if (!tile.GetDoor(dir) && !TileIsNull(dir) && TileHasDoor(dir))
			{
				if (tilePlacementDebug)
					Debug.Log("Can't place tile: No door to match directional door.");
				return false;
			}
			if (tile.GetDoor(dir) && !TileIsNull(dir) && TileHasDoor(dir))
				hasOneMatch = true;
		}
		if (!hasOneMatch)
		{
			if (tilePlacementDebug)
				Debug.Log("Can't place tile: At least one door must match up.");
			return false;
		}

		if (!PlayableOnFloor(tile.currentLocation))
		{
			if (tilePlacementDebug)
				Debug.Log("Not playable on this floor.");
			return false;
		}

		if (tilePlacementDebug)
			Debug.Log("Tile may be placed here.");
		return true;
	}
	/// <summary>
	/// Is the tile playable on this floor?
	/// </summary>
	/// <returns></returns>
	private bool PlayableOnFloor()
	{
		int floor = GetFloor();
		if (floor == -1)
			Debug.Log("What in the hell?");
		Tile tile = tmpTile.GetComponent<Tile>();
		if (floor == 0 && tile.usableInBasement)
			return true;
		if (floor == 1 && tile.usableOnGround)
			return true;
		if (floor == 2 && tile.usableOnUpper)
			return true;
		return false;
	}
	private bool PlayableOnFloor(Coord c)
	{
		int floor = GetFloor(c);
		if (floor == -1)
			Debug.Log("What in the hell?");
		Tile tile = tmpTile.GetComponent<Tile>();
		if (floor == 0 && tile.usableInBasement)
			return true;
		if (floor == 1 && tile.usableOnGround)
			return true;
		if (floor == 2 && tile.usableOnUpper)
			return true;
		return false;
	}
	/// <summary>
	/// Is the current tile occupied?
	/// </summary>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	private bool TileOccupied(int x, int z)
	{
		if (tiles[x, z] != null)
			return true;
		return false;
	}
	private bool TileOccupied(Coord c)
	{
		if (tiles[c.x, c.z] != null)
			return true;
		return false;
	}
	/// <summary>
	/// Is the current tile occupied?
	/// </summary>
	/// <returns></returns>
	private bool TileOccupied()
	{
		return TileOccupied(curX, curZ);
	}
	/// <summary>
	/// Are all adjacent tiles empty?
	/// </summary>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	private bool AllAdjacentTilesEmpty(int x, int z)
	{
		GameObject tileN = tiles[x, z + 1];       // Tile to the NORTH
		GameObject tileS = tiles[x, z - 1];       // South
		GameObject tileE = tiles[x + 1, z];       // East
		GameObject tileW = tiles[x - 1, z];       // West
		if (tileN == null && tileS == null && tileE == null && tileW == null)   // Are adjacent tiles all empty?  If so, can't place there.
			return true;
		return false;
	}
	private bool AllAdjacentTilesEmpty(Coord c)
	{
		return AllAdjacentTilesEmpty(c.x, c.z);
	}
	/// <summary>
	/// Are all adjacent tiles empty?
	/// </summary>
	/// <returns></returns>
	private bool AllAdjacentTilesEmpty()
	{
		return AllAdjacentTilesEmpty(curX, curZ);
	}
	/// <summary>
	/// Does the direction tile have an appropriate door?
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	private bool TileHasDoor(string dir)
	{
		dir = dir.ToLower();
		if (debugging && dir == "w")
			Debug.Log("Break");
		// int x = curX;
		// int z = curZ;
		int x = tmpTile.GetComponent<Tile>().currentLocation.x;
		int z = tmpTile.GetComponent<Tile>().currentLocation.z;
		if (dir == "n")
			z++;
		if (dir == "e")
			x++;
		if (dir == "s")
			z--;
		if (dir == "w")
			x--;
		if (tiles[x, z] == null)
			return false;
		Tile testTile = tiles[x, z].GetComponent<Tile>();
		if (dir == "n")
			return testTile.doorS;
		if (dir == "e")
			return testTile.doorW;
		if (dir == "s")
			return testTile.doorN;
		if (dir == "w")
			return testTile.doorE;
		//Debug.Log("You should not EVER get here.");
		return false;
	}
	/// <summary>
	/// Is the directional tile null?
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	private bool TileIsNull(string dir)
	{
		return TileIsNull(dir, tmpTile.GetComponent<Tile>().currentLocation);
	}
	private bool TileIsNull(string dir, Coord c)
	{
		return TileIsNull(dir, c.x, c.z);
	}
	private bool TileIsNull(string dir, int x, int z)
	{
		dir = dir.ToLower();
		if (dir == "n")
			z++;
		if (dir == "e")
			x++;
		if (dir == "s")
			z--;
		if (dir == "w")
			x--;
		if (tiles[x, z] == null)
		{
			if (debugging)
				Debug.Log("Checking tile to " + dir + ": null");
			return true;
		}
		if (debugging)
			Debug.Log("Checking tile to " + dir + ": " + tiles[x, z].GetComponent<Tile>().roomName);
		return false;
	}
	/// <summary>
	/// Place selected tile onto the game board.
	/// </summary>
	/// <param name="x">X-Coordinate</param>
	/// <param name="z">Z-Coordinate</param>
	/// <param name="tile">Tile to place</param>
	public void PlaceTile(int x, int z, GameObject tile)
	{
		PlaceTile(x, z, tile, 180.0f, new Vector3(0.5f, 0.0f, 0.5f));
	}
	/// <summary>
	/// Place selected tile onto the game board.
	/// </summary>
	/// <param name="x">X-Coordinate</param>
	/// <param name="z">Z-Coordinate</param>
	/// <param name="tile">Tile to place</param>
	/// <param name="rotation">Direction to place the tile in degrees</param>
	public void PlaceTile(int x, int z, GameObject tile, int rotation)
	{
		PlaceTile(x, z, tile, (float)rotation);
	}
	/// <summary>
	/// Place selected tile onto the game board.
	/// </summary>
	/// <param name="x">X-Coordinate</param>
	/// <param name="z">Z-Coordinate</param>
	/// <param name="tile">Tile to place</param>
	/// <param name="rotation">Direction to place the tile in degrees</param>
	public void PlaceTile(int x, int z, GameObject tile, float rotation)
	{
		PlaceTile(x, z, tile, rotation, new Vector3(0.5f, 0.0f, 0.5f));
	}
	/// <summary>
	/// Place selected tile onto the game board.
	/// </summary>
	/// <param name="x">X-Coordinate</param>
	/// <param name="z">Z-Coordinate</param>
	/// <param name="tile">Tile to place</param>
	/// <param name="rotation">Direction to place the tile in degrees</param>
	/// <param name="offset">Tile offset</param>
	public void PlaceTile(int x, int z, GameObject tile, int rotation, Vector3 offset)
	{
		PlaceTile(x, z, tile, (float)rotation, offset);
	}
	/// <summary>
	/// Place selected tile onto the game board.
	/// </summary>
	/// <param name="x">X-Coordinate</param>
	/// <param name="z">Z-Coordinate</param>
	/// <param name="tile">Tile to place</param>
	/// <param name="rotation">Direction to place the tile in degrees</param>
	/// <param name="offset">Tile offset</param>
	public void PlaceTile(int x, int z, GameObject tile, float rotation, Vector3 offset)
	{
		float newX = x + offset.x;
		float newZ = z + offset.z;
		float height = tilesStart + tileHeight;
		tiles[x, z] = Instantiate(tile, new Vector3(newX, height, newZ), Quaternion.Euler(0.0f, rotation, 0.0f));

		//tiles[x, z].GetComponent<Tile>() = tmpTile.GetComponent<Tile>();
		if (!settingUpBoard)
		{
			tiles[x, z].GetComponent<Tile>().Import(tmpTile);
			tiles[x, z].GetComponent<Tile>().SetFloor(GetFloor());
		}
		tiles[x, z].GetComponent<Tile>().displayed = true;
		setRed(tiles[x, z], false);
		setTransparent(tiles[x, z], false);
		/*for (int a = (int)rotation; a >= 0; a -= 90)
		{
			tiles[x, z].GetComponent<Tile>().ShiftDoorsCW();
		}*/
	}
	/// <summary>
	/// Get the floor of the adjacent tiles.
	/// </summary>
	/// <returns></returns>
	private int GetFloor()
	{
		foreach (string dir in new List<string> { "n", "e", "s", "w" })
		{
			int x = curX;
			int z = curZ;
			if (dir == "n")
				z++;
			if (dir == "e")
				x++;
			if (dir == "s")
				z--;
			if (dir == "w")
				x--;
			if (tiles[x, z] != null)
			{
				Tile tile = tiles[x, z].GetComponent<Tile>();
				Tile _tmpTileScript = tmpTile.GetComponent<Tile>();

				if (_tmpTileScript.GetDoor(dir) && !TileIsNull(dir) && TileHasDoor(dir))
					return tile.currentFloor;
			}
		}
		return -1;
	}
	private int GetFloor(Coord c)
	{
		foreach (string dir in new List<string> { "n", "e", "s", "w" })
		{
			int x = c.x;
			int z = c.z;
			if (dir == "n")
				z++;
			if (dir == "e")
				x++;
			if (dir == "s")
				z--;
			if (dir == "w")
				x--;
			if (tiles[x, z] != null)
			{
				Tile tile = tiles[x, z].GetComponent<Tile>();
				return tile.currentFloor;
			}
		}
		return -1;
	}
	/// <summary>
	/// Set the current player activity
	/// </summary>
	/// <param name="act">The activity to set</param>
	public void SetActivity(string act)
	{
		act = act.ToLower();
		activity = act;
		if (act == "placing tiles")
		{
			oldX = -1;
			oldZ = -1;
		}
		else if (act == "moving player")
		{
			//Coord _playerCoord = players[currentPlayer].GetComponent<Player>().getCoords();
			getMovableTiles();
			outlineMovableTiles();
			placingTile = false;
		}
		currentActivity.text = SeeActivity();
	}
	/// <summary>
	/// Returns the Player script attached to the current player.
	/// </summary>
	/// <returns></returns>
	public Player getCurrentPlayer()
	{
		return players[currentPlayer].GetComponent<Player>();
	}
	/// <summary>
	/// See what the current activity is.  Uses Peek() and not Pop()
	/// </summary>
	/// <returns>The activity currently taking place</returns>
	public string SeeActivity()
	{
		return activity;
	}
	public bool SeeActivity(string s)
	{
		return SeeActivity().ToLower() == s.ToLower();
	}
	/// <summary>
	/// Roll the dice.
	/// </summary>
	/// <param name="numberOfDice"></param>
	/// <returns></returns>
	public int RollDice(int numberOfDice)
	{
		System.Random _rnd = new System.Random();
		int _total = 0;
		for (int a = 0; a < numberOfDice; a++)
		{
			_total += _rnd.Next(3);
		}

		return _total;
	}
	public int RollDice(Player player)
	{
		return RollDice(player.getSpeed());
	}
	/// <summary>
	/// Sends camera to one of the initial floor locations.
	/// </summary>
	/// <param name="location"></param>
	public void SendCamToLocation(string location)
	{
		//Debug.Log("Going " + location);
		//Debug.Log(upstairsStartingX + " " + upstairsStartingZ);
		location = location.ToLower();
		int x = 0;
		int z = 0;
		if (location == "player")
		{
			x = getCurrentPlayer().x;
			z = getCurrentPlayer().z;
			Debug.Log("Player at " + x + ", " + z);
		}
		if (location == "ground" || location == "entrance")
		{
			x = entranceStartingX;
			z = entranceStartingZ;
		} else if (location == "upper" || location == "upstairs")
		{
			x = upstairsStartingX;
			z = upstairsStartingZ;
		}else if (location == "basement")
		{
			x = basementStartingX;
			z = basementStartingZ;
		}
		//Debug.Log("New Camera X: " + x);
		//Debug.Log("New Camear Y: " + z);
		cam.transform.position = tiles[x, z].transform.position;
		cam.transform.Translate(new Vector3(0f, 0f, cameraOffset));
		UpdateCamPositioning();
	}
	/// <summary>
	/// Create an outline color around the specified tile.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="z"></param>
	public void makeOutline(int x, int z)
	{
		//GameObject highlight = Instantiate(outline, tiles[x, z].transform.position, Quaternion.Euler(Vector3.zero));
		float _x = x + 0.5f;
		float _y = highlightY;
		float _z = z + 0.5f;
		//GameObject highlight = Instantiate(outline, new Vector3(_x, _y, _z), Quaternion.Euler(Vector3.zero));
		outlines.Add(Instantiate(outline, new Vector3(_x, _y, _z), Quaternion.Euler(Vector3.zero)));
	}
	public void makeOutline(int x, int z, string color)
	{
		makeOutline(x, z);
		colorOutlines(color);
	}
	/// <summary>
	/// Set outline color
	/// </summary>
	/// <param name="color"></param>
	public void colorOutlines(Color color)
	{
		foreach (GameObject outline in outlines)
		{
			foreach (var child in outline.GetComponentsInChildren<Renderer>())
			{
				if (child.transform != transform)
				{
					child.material.color = color;
				}
			}
		}
	}
	public void colorOutlines(string color)
	{
		color = color.ToLower();
		Color c;
		c = Color.white;
		switch (color)
		{
			case "red":
				c = Color.red;
				break;
			case "white":
				c = Color.white;
				break;
			case "black":
				c = Color.black;
				break;
			case "blue":
				c = Color.blue;
				break;
			case "cyan":
				c = Color.cyan;
				break;
			case "grey":
			case "gray":
				c = Color.gray;
				break;
			case "magenta":
				c = Color.magenta;
				break;
			case "yellow":
				c = Color.yellow;
				break;
			default:
				c = Color.white;
				break;
		}
		colorOutlines(c);
	}
	public void removeOutlines()
	{
		while (outlines.Count > 0)
		{
			Destroy(outlines[0]);
			outlines.RemoveAt(0);
		}
	}
	/// <summary>
	/// Update the Heads-Up Display (HUD)
	/// </summary>
	public void updateHUDInfo()
	{
		Player _player = players[currentPlayer].GetComponent<Player>();
		playerName.text = _player.playerName;
		playerMight.text = _player.might.ToString();
		playerSpeed.text = _player.speed.ToString();
		playerSanity.text = _player.sanity.ToString();
		playerWisdom.text = _player.wisdom.ToString();
		currentActivity.text = SeeActivity();
	}
}