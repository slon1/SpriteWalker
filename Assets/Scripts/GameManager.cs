using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using View;
using Model;
using Zenject;
using Grid = Model.Grid;
using System;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.Windows;
using Zenject.SpaceFighter;
using Server;
using System.Threading.Tasks;

[Serializable]
public class PlayerData {
	public Vector2 dir;
	public float speed;
	public float rotationSpeed;	
	public PlayerState state;
	public List<Vector2> waypoints;
}
[Serializable]
public class GameData {
	//public PlayerData playerData;
	public float playerSpeed=10;
	public float playerRotationSpeed=10;
	public string boardUrl;
	public bool useUrl;
	public Sprite board;
	public int boardPPU = 100;
	public Vector2Int playerStart;
	public Vector2Int cellsize=new Vector2Int(1,1);
	public int maxWaypoints=2048;
	public Color border=Color.black;
}

public class GameManager : MonoBehaviour {
	public enum GameState { Loading, MainMenu, Gameplay, Exit }
	private GameState currentState;
	private SaveSystem saveSystem;
	private UIManager uiManager;
	private Player player;
	private VirtualScreen view;
	private Grid grid;
	
	[SerializeField]
	GameConfig config;

	private Pathfinding pathfinding;
	private MouseOrTouchInput input;
	private PlayerData gameData;
	

	[Inject]
	private void Construct(SaveSystem saveLoad, UIManager uI, Player player, VirtualScreen screen, MouseOrTouchInput input) {
		saveSystem = saveLoad;
		uiManager = uI;
		this.player = player;
		this.view = screen;
		this.input = input;
	}

	//void OnDrawGizmos() {
	//	grid?.DrawGizmos(cellsize);
	//}

	void Start() {
		
		ChangeState(GameState.Loading);
	}

	public void ChangeState(GameState newState) {
		currentState = newState;
		switch (newState) {
			case GameState.Loading:
				StartCoroutine(LoadingRoutine());
				break;
			case GameState.MainMenu:				
				uiManager.ShowMainMenu();
				break;
			case GameState.Gameplay:
				uiManager.HideMainMenu();
				StartGame();
				//saveSystem.LoadGame();
				break;
			case GameState.Exit:
			//	saveSystem.SaveGame(null);
				break;
		}
	}
	public ServerClient server;
	private GameObject boardGo;
	public async Task<Sprite> LoadSprite(string path, int ppu) {		
		var texture = await server.HttpGetTextureAsync(path);
		Rect rect = new Rect(0, 0, texture.width, texture.height);
		Vector2 pivot = new Vector2(0.5f, 0.5f);		
		return Sprite.Create(texture, rect, pivot, ppu);		
		
	}
	private Sprite level;
	private async void StartGame() {
		var ttt= await server.HttpGetAsync("https://drive.google.com/uc?export=download&id=15XcCpG8ajLuvbFCtdaem2YOo2-TZ3ZUy");
		GameData gd = JsonUtility.FromJson<GameData>(ttt);
		config.gameData= gd;
		Sprite level;
		if (config.gameData.useUrl) {
			var path = "file://" + Environment.CurrentDirectory + config.gameData.boardUrl;
			level = await LoadSprite(config.gameData.boardUrl, config.gameData.boardPPU);
		}else {
			level = config.gameData.board;
		}

		boardGo = new GameObject("Board");
		var sr = boardGo.AddComponent<SpriteRenderer>();
		sr.sortingOrder = -10;
		sr.sprite = level;

		grid = new Grid(level, config.gameData.cellsize, config.gameData.border);

		input.Enable = true;
		
		pathfinding = new Pathfinding(grid);
		
		view.Init(level, config.gameData.cellsize);
		
		player.Init(view.GetCellScreenPosition(config.gameData.playerStart));
		player.speed = config.gameData.playerSpeed;
		player.rotationSpeed = config.gameData.playerRotationSpeed;
		

		//saveSystem.SaveGame(JsonUtility.ToJson(player, true));
		
		print(JsonUtility.ToJson(config.gameData, true));

	}
	private void OnDrawGizmos1() {
		grid?.DrawGizmos(config.gameData.cellsize);
	}
	private void OnEnable() {
		input.OnInputReceived += Input_OnInputReceived;
	}
	Cell lastWP;
	private void Input_OnInputReceived(Vector2 screenXY) {
		//var tt = view.GetCell(screenXY);
		//print(grid.GetCell(screen.GetCell(Input.mousePosition)).isUnwalkable);
		
			var start = (lastWP == null)?grid.GetCell(view.GetCell(Camera.main.WorldToScreenPoint(player.go.position))):lastWP;
			var path = pathfinding.FindPath(start, grid.GetCell(view.GetCell(screenXY)));
			if (path != null) {
				var list = new List<Vector2>();
				foreach (var item in path) {
					var position = view.GetCellScreenPosition(item.x, item.y);
					list.Add(position);
				}
				player.SetWayPoints(list);
				lastWP=path[path.Count-1];
			}
		
		
	}

	private void OnDisable() {
		input.OnInputReceived -= Input_OnInputReceived;
	}

	private IEnumerator LoadingRoutine() {
		float elapsedTime = 0f;
		while (elapsedTime < 1) {
			uiManager.Spin.transform.Rotate(-Vector3.forward, 500 * Time.deltaTime);
			elapsedTime += Time.deltaTime;
			yield return null; 
		}		
		ChangeState(GameState.MainMenu);
	}
	private void OnDestroy() {
		Destroy(level);
		Destroy(boardGo);
		grid?.Dispose();
		pathfinding?.Dispose();
		StopAllCoroutines();
		input.Enable = true;

	}
}
