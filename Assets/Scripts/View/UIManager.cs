using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour
{
	
	public GameObject mainMenu;
	public GameObject gameplayUI;
	public GameObject Loader;
	public GameObject Spin;

	private GameManager gameManager;
	[Inject]
	private void Construct(GameManager gameManager) {
		this.gameManager = gameManager;
	}
	

	public void ShowMainMenu() {
		mainMenu.SetActive(true);
		gameplayUI.SetActive(false);
		Loader.SetActive(false);
	}

	public void HideMainMenu() {
		mainMenu.SetActive(false);
		gameplayUI.SetActive(true);
		Loader.SetActive(false);
	}

	public void OnStartButtonClicked() {
		gameManager.ChangeState(GameManager.GameState.Gameplay);
	}

	public void OnBackButtonClicked() {
		gameManager.ChangeState(GameManager.GameState.MainMenu);
	}

	public void OnExitButtonClicked() {
		gameManager.ChangeState(GameManager.GameState.MainMenu);
	}

	
}

