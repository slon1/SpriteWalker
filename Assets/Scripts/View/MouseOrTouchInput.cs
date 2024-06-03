using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOrTouchInput : MonoBehaviour {
	public event Action<Vector2> OnInputReceived;
	public bool Enable { get; set; } = false;
	void Update() {
		if (!Enable) return;
		
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {			
			OnInputReceived?.Invoke(Input.mousePosition);
		}
		
		if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject()) {
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				OnInputReceived?.Invoke(touch.position);
			}
		}
	}

	
}
