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

[Serializable]
public class PlayerData {
	public Vector2 dir;
	public float speed;
	public float rotationSpeed;
	public PlayerState state;
	public List<Vector2> waypoints;
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
	private Sprite level;
	[SerializeField]
	private Vector2 cellsize;
	[SerializeField]
	private Color border= Color.black;
	[SerializeField]
	private Vector2Int playerPos;

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

	private void StartGame() {		
		grid = new Grid(level, cellsize, border);
		input.Enable = true;
		pathfinding = new Pathfinding(grid);
		view.Init(level, cellsize);		
		player.Init(view.GetCellScreenPosition(playerPos));



		//print(JsonUtility.ToJson(player,true));

	}
	private void OnEnable() {
		input.OnInputReceived += Input_OnInputReceived;
	}

	private void Input_OnInputReceived(Vector2 screenXY) {
		//var tt = view.GetCell(screenXY);
		//print(grid.GetCell(screen.GetCell(Input.mousePosition)).isUnwalkable);
		var start = grid.GetCell(view.GetCell(Camera.main.WorldToScreenPoint(player.go.position)));
		var path = pathfinding.FindPath(start, grid.GetCell(view.GetCell(screenXY)));		
		if (path != null) {
			var list = new List<Vector2>();
			foreach (var item in path) {				
				var position = view.GetCellScreenPosition(item.x, item.y);
				list.Add(position);
			}
			player.SetWayPoints(list);
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
		grid?.Dispose();
		pathfinding?.Dispose();
		StopAllCoroutines();
		input.Enable = true;

	}
}
