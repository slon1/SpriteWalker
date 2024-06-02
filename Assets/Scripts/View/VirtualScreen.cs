using UnityEngine;
using UnityEditor;

using UnityEngine.U2D;
using System.Runtime.CompilerServices;
namespace View {
	public class VirtualScreen : MonoBehaviour {
		//private Sprite sprite;
		private Vector2 cellSize; // Размер ячеек в пикселях		
		private Vector2 spriteSize;
		private Vector2 spritePosition;
		
		private Vector2 worldCellSize;

		public void Init(Sprite sprite, Vector2 cellSize) {
			this.cellSize = cellSize;
			float spriteWidth = sprite.rect.width / sprite.pixelsPerUnit;
			float spriteHeight = sprite.rect.height / sprite.pixelsPerUnit;
			spriteSize = new Vector2(spriteWidth, spriteHeight);
			spritePosition = sprite.bounds.center;
			worldCellSize = cellSize / sprite.pixelsPerUnit;

		}
		//void OnDrawGizmos1() {
		//	if (!Application.isPlaying) return;



		//	int numCellsX = Mathf.FloorToInt(spriteSize.x / worldCellSize.x);
		//	int numCellsY = Mathf.FloorToInt(spriteSize.y / worldCellSize.y);

		//	Gizmos.color = Color.black;


		//	for (int x = 0; x <= numCellsX; x++) {
		//		Vector3 start = new Vector3(spritePosition.x + x * worldCellSize.x - spriteSize.x / 2, spritePosition.y - spriteSize.y / 2, 0);
		//		Vector3 end = new Vector3(spritePosition.x + x * worldCellSize.x - spriteSize.x / 2, spritePosition.y + spriteSize.y / 2, 0);
		//		Gizmos.DrawLine(start, end);
		//	}


		//	for (int y = 0; y <= numCellsY; y++) {
		//		Vector3 start = new Vector3(spritePosition.x - spriteSize.x / 2, spritePosition.y + y * worldCellSize.y - spriteSize.y / 2, 0);
		//		Vector3 end = new Vector3(spritePosition.x + spriteSize.x / 2, spritePosition.y + y * worldCellSize.y - spriteSize.y / 2, 0);
		//		Gizmos.DrawLine(start, end);
		//	}

		//	
		//	GUIStyle style = new GUIStyle();
		//	style.normal.textColor = Color.black;

		//	for (int x = 0; x < numCellsX; x++) {
		//		for (int y = 0; y < numCellsY; y++) {
		//			Vector3 cellCenter = new Vector3(spritePosition.x + x * worldCellSize.x + worldCellSize.x / 2 - spriteSize.x / 2, spritePosition.y + y * worldCellSize.y + worldCellSize.y / 2 - spriteSize.y / 2, 0);
		//			Handles.Label(cellCenter, $"({x},{y})", style);
		//		}
		//	}
		//}
		public Vector2 GetCellScreenPosition(Vector2Int pos) {
			return GetCellScreenPosition(pos.x, pos.y);
		}
		public Vector2 GetCellScreenPosition(int x, int y) {
			return spritePosition + new Vector2(x * worldCellSize.x + worldCellSize.x * 0.5f - spriteSize.x * 0.5f,
															  y * worldCellSize.y + worldCellSize.y * 0.5f - spriteSize.y * 0.5f);
			
		}

		public Vector2Int GetCell(Vector2 screenPos) {
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
			Vector2 localPos = worldPos - (Vector3)spritePosition + new Vector3(spriteSize.x * 0.5f, spriteSize.y * 0.5f, 0);
			int cellX = Mathf.FloorToInt(localPos.x / worldCellSize.x);
			int cellY = Mathf.FloorToInt(localPos.y / worldCellSize.y);
			return new Vector2Int(cellX, cellY);
		}
	}
}