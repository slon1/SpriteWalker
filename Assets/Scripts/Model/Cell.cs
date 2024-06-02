using System;
using System.Collections;

namespace Model {	
	public class Cell {
		public int x;
		public int y;
		public bool isUnwalkable;

		public Cell(int x, int y, bool isUnwalkable) {
			this.x = x;
			this.y = y;
			this.isUnwalkable = isUnwalkable;
		}
	}
}