using System;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState {
	Idle,
	Go
}
/// <summary>
/// Represents a player in the game, capable of moving between waypoints with a specified speed and rotation speed.
/// </summary>
public class Player : MonoBehaviour  {
	private Vector2 dir;
	private float speed, rotationSpeed;
	private PlayerState state = PlayerState.Idle;
	private List<Vector2> waypoints = new List<Vector2>();
	public Transform go;

	private void Update() {
		if (state == PlayerState.Go && waypoints.Count >=0) {
			MoveToNextWaypoint();
		}
	}
	
	public void SetWayPoints(List<Vector2> newWaypoints) {
		waypoints.AddRange(newWaypoints);
		if (state == PlayerState.Idle) {
			state = PlayerState.Go;
		}
	}
	
	private void MoveToNextWaypoint() {
		if (waypoints.Count == 0) {
			state = PlayerState.Idle;
			return;
		}

		Vector2 target = waypoints[0];
		Vector2 currentPosition = go.position;
		Vector2 direction = (target - currentPosition).normalized;

		go.position = Vector2.MoveTowards(currentPosition, target, speed * Time.deltaTime);
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		go.rotation = Quaternion.Lerp(go.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);

		if (Vector2.SqrMagnitude(currentPosition - target) < 0.1f) {
			waypoints.RemoveAt(0);
		}

		dir = direction;
	}
	public void LoadFromGameData(PlayerData data) {
		dir = data.dir;
		if (dir != Vector2.zero) {
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			go.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		}
		waypoints =data.waypoints;
		speed = data.speed;
		rotationSpeed = data.rotationSpeed;
		go.position = data.position;
		state=data.state;
	}
	public PlayerData GetGameData() {
		PlayerData data = new PlayerData();
		data.dir = dir;
		data.waypoints = waypoints;
		data.speed=speed  ;
		data.rotationSpeed = rotationSpeed;
		data.position= go.position ;
		data.state= state;
		return data;
	}

	internal void Init(Vector2 pos, float speed, float rotationSpeed) {
		go.position = pos;		    
		this.speed=speed;
		this.rotationSpeed = rotationSpeed;

	}
	private void OnDestroy() {
		waypoints.Clear();
		go = null;
	}

	
}
