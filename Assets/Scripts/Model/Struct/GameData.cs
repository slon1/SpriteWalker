using System;
using UnityEngine;

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
	public Color border = Color.black;
}
