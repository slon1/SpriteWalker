using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Model {
	[Serializable]
	public class Grid {
		private Cell[,] cells;

		public int Width { get; private set; }
		public int Height { get; private set; }
		private Color unwalkableColor;

		public Grid(Sprite sprite, Vector2 cellSize, Color unwalkableColor) {
			this.unwalkableColor = unwalkableColor;
			GenerateGrid(sprite, cellSize);
		}

		private void GenerateGrid(Sprite sprite, Vector2 cellSize) {
			Width = Mathf.CeilToInt(sprite.texture.width / cellSize.x);
			Height = Mathf.CeilToInt(sprite.texture.height / cellSize.y);
			cells = new Cell[Width, Height];

			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					bool isUnwalkable = IsCellUnwalkable(sprite, x, y, cellSize);
					cells[x, y] = new Cell(x, y, isUnwalkable);
				}
			}
		}

		private bool IsCellUnwalkable(Sprite sprite, int cellX, int cellY, Vector2 cellSize) {
			int startX = Mathf.RoundToInt(cellX * cellSize.x);
			int startY = Mathf.RoundToInt(cellY * cellSize.y);
			int width = Mathf.RoundToInt(cellSize.x);
			int height = Mathf.RoundToInt(cellSize.y);

			// Ensure that the width and height do not exceed the texture bounds
			if (startX + width > sprite.texture.width) {
				width = sprite.texture.width - startX;
			}
			if (startY + height > sprite.texture.height) {
				height = sprite.texture.height - startY;
			}

			Color[] pixels = sprite.texture.GetPixels(startX, startY, width, height);

			foreach (Color pixelColor in pixels) {
				if (pixelColor == unwalkableColor) {
					return true;
				}
			}
			return false;
		}

		public Cell GetCell(Vector2Int pos) {
			if (IsValidPosition(pos.x,pos.y)) {
				return cells[pos.x, pos.y];
			}
			return null;
		}
		public Cell GetCell(int x, int y) {
			return GetCell(new Vector2Int(x,y));
		}
		public void SetCell(int x, int y, Cell cell) {
			if (IsValidPosition(x, y)) {
				cells[x, y] = cell;
			}
		}

		public bool IsValidPosition(int x, int y) {
			return x >= 0 && x < Width && y >= 0 && y < Height;
		}

		public IEnumerable<Cell> GetAllCells() {
			foreach (var cell in cells) {
				yield return cell;
			}
		}
				

		//public void DrawGizmos1(Vector2 cellSize) {
		//	Gizmos.color = Color.green;
		//	Vector3 cellSize3D = new Vector3(cellSize.x, cellSize.y, 1);

		//	GUIStyle style = new GUIStyle {
		//		normal = { textColor = Color.black }
		//	};

		//	foreach (var cell in GetAllCells()) {
		//		Vector3 cellCenter = new Vector3(cell.x * cellSize.x + cellSize.x / 2, cell.y * cellSize.y + cellSize.y / 2, 0);
		//		if (cell.isUnwalkable) {
		//			Gizmos.color = Color.red;
		//			Gizmos.DrawWireCube(cellCenter, cellSize3D);
		//		}
		//		else {
		//			Gizmos.color = Color.green;
		//			Gizmos.DrawWireCube(cellCenter, cellSize3D);
		//		}

		//		Handles.Label(cellCenter, $"({cell.x},{cell.y})", style);
		//	}
		//}
	}
}


