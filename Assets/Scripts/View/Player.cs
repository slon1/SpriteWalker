using System;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState {
	Idle,
	Go
}
[Serializable]
public class Player : MonoBehaviour  {
	public Vector2 dir;
	public float speed, rotationSpeed;
	public PlayerState state = PlayerState.Idle;
	public List<Vector2> waypoints = new List<Vector2>();
	public Transform go;

	private void Update() {
		if (state == PlayerState.Go && waypoints.Count >=0) {
			MoveToNextWaypoint();
		}
	}
	//todo add wp add trace
	public void SetWayPoints(List<Vector2> newWaypoints) {
		waypoints.AddRange(newWaypoints);
		if (state == PlayerState.Idle) {
			state = PlayerState.Go;
		}
	}
	//todo rotation
	private void MoveToNextWaypoint() {
		if (waypoints.Count == 0) {
			state = PlayerState.Idle;
			return;
		}

		Vector2 target = waypoints[0];
		Vector2 currentPosition = go.position;
		Vector2 direction = (target - currentPosition).normalized;

		go.position = Vector2.MoveTowards(currentPosition, target, speed * Time.deltaTime);
		//go.rotation = Quaternion.Slerp(go.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

		if (Vector2.SqrMagnitude(currentPosition - target) < 0.1f) {
			waypoints.RemoveAt(0);
		}

		dir = direction;
	}

	//public static Player Deserialize(string json) {
	//	PlayerData data = JsonUtility.FromJson<PlayerData>(json);
	//	Player player = playerTransform.gameObject.AddComponent<Player>();
	//	player.dir = data.dir;
	//	player.speed = data.speed;
	//	player.rotationSpeed = data.rotationSpeed;
	//	player.state = data.state;
	//	player.waypoints = data.waypoints;
		
	//	return player;
	//}

	internal void Init(Vector2 pos) {
		go.position = pos;
		//go = sprite.GetComponent<SpriteRenderer>().transform;        
		
	}
	private void OnDestroy() {
		waypoints.Clear();
	}
}
