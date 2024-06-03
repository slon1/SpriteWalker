using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Model {	
	public class Grid : IDisposable{
		private Cell[,] cells;		
		private int width { get; set; }
		private int height { get; set; }
		private Color unwalkableColor;
		//todo !=null !=(0,0)
		public Grid(Sprite sprite, Vector2 cellSize, Color unwalkableColor) {
			this.unwalkableColor = unwalkableColor;
			GenerateGrid(sprite, cellSize);
			
		}

		private void GenerateGrid(Sprite sprite, Vector2 cellSize) {
			width = Mathf.CeilToInt(sprite.texture.width / cellSize.x);
			height = Mathf.CeilToInt(sprite.texture.height / cellSize.y);
			cells = new Cell[width, height];

			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
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
			return x >= 0 && x < width && y >= 0 && y < height;
		}

		public IEnumerable<Cell> GetAllCells() {
			foreach (var cell in cells) {
				yield return cell;
			}
		}


		public void DrawGizmos(Vector2 cellSize) {
			if (!Application.isPlaying) return;
			Gizmos.color = Color.green;
			Vector3 cellSize3D = new Vector3(cellSize.x, cellSize.y, 1);

			GUIStyle style = new GUIStyle {
				normal = { textColor = Color.black }
			};

			foreach (var cell in GetAllCells()) {
				Vector3 cellCenter = new Vector3(cell.x * cellSize.x + cellSize.x * 0.5f, cell.y * cellSize.y + cellSize.y * 0.5f, 0);
				if (cell.isUnwalkable) {
					Gizmos.color = Color.red;
					Gizmos.DrawWireCube(cellCenter, cellSize3D);
				}
				else {
					Gizmos.color = Color.green;
					Gizmos.DrawWireCube(cellCenter, cellSize3D);
				}

				Handles.Label(cellCenter, $"({cell.x},{cell.y})", style);
			}
		}

		public void Dispose() {
			cells = null;
		}
	}
}


