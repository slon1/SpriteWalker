using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using View;
using Zenject;
using Grid = Model.Grid;
/// <summary>
/// Manages the game state, initialization, and interactions between different components.
/// </summary>
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

		player.Init(view.GetCellScreenPosition(gameData.playerStart), gameData.playerSpeed, gameData.playerRotationSpeed);


	}
	public void Save() {
		
		saveSystem.SaveGame(JsonUtility.ToJson(player.GetGameData()));
	}
	public async void Load() {
		var data= JsonUtility.FromJson<PlayerData> (await saveSystem.LoadGame());
		player.LoadFromGameData(data);



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
		while (elapsedTime < 3) {
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
