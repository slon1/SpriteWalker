using UnityEngine;
using System;

public class MouseOrTouchInput : MonoBehaviour {
	public event Action<Vector2> OnInputReceived;
	public bool Enable { get; set; } = false;
	void Update() {
		if (!Enable) return;
		// Check for mouse input
		if (Input.GetMouseButtonDown(0)) {
			Vector2 mousePosition = Input.mousePosition;
			FireInputEvent(mousePosition);
		}

		// Check for touch input
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				Vector2 touchPosition = touch.position;
				FireInputEvent(touchPosition);
			}
		}
	}

	private void FireInputEvent(Vector2 position) {
		OnInputReceived?.Invoke(position);
	}
}
