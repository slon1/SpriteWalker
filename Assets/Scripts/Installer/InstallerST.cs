using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Model;
using Grid = Model.Grid;
using View;
using Utils;

public class InstallerST : MonoInstaller
{
	public SaveSystem saveLoad;
	public GameManager gameManager;
	public UIManager ui;
	//public Grid grid;
	//public VirtualScreen view;
	//public Player player;
	public override void InstallBindings() {
			Container.Bind<SaveSystem>().FromInstance(saveLoad).AsSingle();
			Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
			Container.Bind<UIManager>().FromInstance(ui).AsSingle();
		//	Container.Bind<Grid>().FromInstance(grid).AsSingle();
		//	Container.Bind<VirtualScreen>().FromInstance(view).AsSingle();
		//	Container.Bind<Player>().FromInstance(player).AsSingle();
	}
}
