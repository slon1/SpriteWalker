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
using Unity.VisualScripting.FullSerializer;
using NUnit.Framework.Internal.Commands;

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
	public float playerSpeed = 10;
	public float playerRotationSpeed = 10;
	public string boardUrl;
	public bool useUrl;
	public Sprite board;
	public int boardPPU = 100;
	public Vector2Int playerStart;
	public Vector2Int cellsize = new Vector2Int(1, 1);
	public int maxWaypoints = 2048;
	public Color border = Color.black;
}

public class GameManager : MonoBehaviour {
	public enum GameState { Loading, MainMenu, Gameplay, Exit }
	private GameState currentState;
	private SaveSystem saveSystem;
	private UIManager uiManager;
	private Player player;
	private VirtualScreen view;
	private Grid grid;


	private GameConfig config;

	private Pathfinding pathfinding;
	private MouseOrTouchInput input;
	private PlayerData playerData;
	private GameData gameData;
	private ConfigLoader configLoader;
	private GameObject boardGo;
	private Cell lastWP;

	[Inject]
	private void Construct(SaveSystem saveLoad, UIManager uI, Player player, VirtualScreen screen, MouseOrTouchInput input, ConfigLoader config) {
		saveSystem = saveLoad;
		uiManager = uI;
		this.player = player;
		this.view = screen;
		this.input = input;
		this.configLoader = config;
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

	//public async Task<Sprite> LoadSprite(string path, int ppu) {		
	//	var texture = await server.HttpGetTextureAsync(path);
	//	Rect rect = new Rect(0, 0, texture.width, texture.height);
	//	Vector2 pivot = new Vector2(0.5f, 0.5f);		
	//	return Sprite.Create(texture, rect, pivot, ppu);		

	//}


	private async void StartGame() {		
		
		gameData = (config == null)? await configLoader.LoadConfig(): config.gameData ;	

		boardGo = new GameObject("Board");
		var sr = boardGo.AddComponent<SpriteRenderer>();
		sr.sortingOrder = -10;
		sr.sprite = gameData.board;
		grid = new Grid(gameData.board, gameData.cellsize, gameData.border);

		input.Enable = true;

		pathfinding = new Pathfinding(grid);

		view.Init(gameData.board, gameData.cellsize);

		player.Init(view.GetCellScreenPosition(gameData.playerStart));
		player.speed = gameData.playerSpeed;
		player.rotationSpeed = gameData.playerRotationSpeed;


		



	}
	public void Save() {
		playerData = new PlayerData();
		playerData.state = player.state;
		playerData.dir=player.dir;
		playerData.waypoints= player.waypoints;
		playerData.speed=player.speed;
		playerData.rotationSpeed=player.rotationSpeed;
		
		saveSystem.SaveGame(JsonUtility.ToJson(playerData));
	}
	public async void Load() {
		var t= JsonUtility.FromJson<PlayerData> (await saveSystem.LoadGame());
		player.dir = t.dir;
		player.waypoints = t.waypoints;
		player.speed = t.speed;
		player.rotationSpeed = t.rotationSpeed;
		player.state=t.state;



	}
	//private void OnDrawGizmos1() {
	//	grid?.DrawGizmos(config.gameData.cellsize);
	//}
	private void OnEnable() {
		input.OnInputReceived += Input_OnInputReceived;
	}
	
	private void Input_OnInputReceived(Vector2 screenXY) {
		

		var start = (lastWP == null) ? grid.GetCell(view.GetCell(Camera.main.WorldToScreenPoint(player.go.position))) : lastWP;
		var path = pathfinding.FindPath(start, grid.GetCell(view.GetCell(screenXY)));
		if (path != null) {
			var list = new List<Vector2>();
			foreach (var item in path) {
				var position = view.GetCellScreenPosition(item.x, item.y);
				list.Add(position);
			}
			player.SetWayPoints(list);
			lastWP = path[path.Count - 1];
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
		Destroy(boardGo);		
		grid?.Dispose();
		pathfinding?.Dispose();
		StopAllCoroutines();
		input.Enable = true;	

	}
}
