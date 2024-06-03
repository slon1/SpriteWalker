using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {
	public Vector2 dir;
	public float speed;
	public float rotationSpeed;
	public PlayerState state;
	public List<Vector2> waypoints;
	public Vector2 position;
}
