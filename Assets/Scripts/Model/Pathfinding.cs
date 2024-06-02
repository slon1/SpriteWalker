using System;
using System.Collections.Generic;

using UnityEngine;
namespace Model { 
public class Pathfinding : IDisposable {
	private Grid grid;

	public Pathfinding(Grid grid) {
		this.grid = grid;
	}

	public List<Cell> FindPath(Cell startCell, Cell targetCell) {		

		if (startCell == null || targetCell == null || startCell.isUnwalkable || targetCell.isUnwalkable) {
			return null; // Path cannot be found
		}

		List<Cell> openList = new List<Cell> { startCell };
		HashSet<Cell> closedList = new HashSet<Cell>();

		Dictionary<Cell, float> gCosts = new Dictionary<Cell, float> { [startCell] = 0 };
		Dictionary<Cell, float> fCosts = new Dictionary<Cell, float> { [startCell] = Heuristic(startCell, targetCell) };

		Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();

		while (openList.Count > 0) {
			Cell current = GetCellWithLowestFCost(openList, fCosts);

			if (current == targetCell) {
				return ReconstructPath(cameFrom, current);
			}

			openList.Remove(current);
			closedList.Add(current);

			foreach (Cell neighbor in GetNeighbors(current)) {
				if (closedList.Contains(neighbor) || neighbor.isUnwalkable) {
					continue;
				}

				float tentativeGCost = gCosts[current] + 1; // Assuming uniform cost for simplicity

				if (!openList.Contains(neighbor)) {
					openList.Add(neighbor);
				}
				else if (tentativeGCost >= gCosts[neighbor]) {
					continue;
				}

				cameFrom[neighbor] = current;
				gCosts[neighbor] = tentativeGCost;
				fCosts[neighbor] = gCosts[neighbor] + Heuristic(neighbor, targetCell);
			}
		}

		return null; // Path not found
	}

	private float Heuristic(Cell a, Cell b) {
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance
	}

	private Cell GetCellWithLowestFCost(List<Cell> cells, Dictionary<Cell, float> fCosts) {
		Cell lowestFCostCell = cells[0];
		foreach (Cell cell in cells) {
			if (fCosts[cell] < fCosts[lowestFCostCell]) {
				lowestFCostCell = cell;
			}
		}
		return lowestFCostCell;
	}

	private List<Cell> GetNeighbors(Cell cell) {
		List<Cell> neighbors = new List<Cell>();

		int[,] directions = {
			{ 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 }, // Cardinal directions
            { -1, 1 }, { 1, 1 }, { 1, -1 }, { -1, -1 } // Diagonal directions
        };

		for (int i = 0; i < directions.GetLength(0); i++) {
			int dx = directions[i, 0];
			int dy = directions[i, 1];
			int newX = cell.x + dx;
			int newY = cell.y + dy;

			if (grid.IsValidPosition(newX, newY)) {
				neighbors.Add(grid.GetCell(newX, newY));
			}
		}

		return neighbors;
	}

	private List<Cell> ReconstructPath(Dictionary<Cell, Cell> cameFrom, Cell current) {
		List<Cell> path = new List<Cell> { current };
		while (cameFrom.ContainsKey(current)) {
			current = cameFrom[current];
			path.Add(current);
		}
		path.Reverse();
		return path;
	}

		public void Dispose() {
			grid.Dispose();
		}
	}
}