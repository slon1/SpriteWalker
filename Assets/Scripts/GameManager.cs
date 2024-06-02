using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Zenject;

public class GameManager : MonoBehaviour {
	public enum GameState { Loading, MainMenu, Gameplay }
	private GameState currentState;
	private SaveSystem saveSystem;
	private UIManager uiManager;
	[Inject]
	private void Construct(SaveSystem saveLoad, UIManager uI) {
		saveSystem = saveLoad;
		uiManager = uI;
	}
	


	void Start() {
		//ChangeState(GameState.Loading);
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
				// Load game data
				saveSystem.LoadGame();
				break;
		}
	}

	private IEnumerator LoadingRoutine() {
		yield return new WaitForSeconds(3); // Имитация загрузки
		ChangeState(GameState.MainMenu);
	}
}
