using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using View;
using Zenject;
using Grid = Model.Grid;

public class Control : MonoBehaviour
{
    //public TextureGridProcessor processor;
    private Grid grid;
    public VirtualScreen screen;
    public Sprite sprite;
    public Vector2 cellsize;
    Pathfinding pathfinding;
    public Player player;

    //[Inject]
    //private void Construct(Grid grid, VirtualScreen view, Player player) {
    //    this.grid = grid;
    //    this.screen = view;
    //    this.player = player;    
    //}

	// Start is called before the first frame update
	void Start()
    {
        grid= new Grid (sprite,cellsize,Color.black);
        screen.Init(sprite, cellsize);
        pathfinding = new Pathfinding(grid);
        player.Init(screen.GetCellScreenPosition(12,6));

		
	}
	
	void Update1()
    {
		if (Input.GetMouseButtonDown(0)) {
            var tt = screen.GetCell(Input.mousePosition);
			print(grid.GetCell(screen.GetCell(Input.mousePosition)).isUnwalkable);
            var tt1 = grid.GetCell(screen.GetCell(Camera.main.WorldToScreenPoint( player.go.position)));

			var t= pathfinding.FindPath(tt1, grid.GetCell(screen.GetCell(Input.mousePosition)));
			var ret = new List<Vector2>();
            if (t != null) {
                foreach (var item in t) {
                    //WayPoint wp= new WayPoint();
                    var position = screen.GetCellScreenPosition(item.x, item.y);
                    ret.Add(position);

                }
                player.SetWayPoints(ret);
            }

        }
	}
    //void OnDrawGizmos() {
    //   grid?.DrawGizmos(cellsize);
    //}
	private void OnDestroy() {
		grid=null;
        pathfinding?.Dispose();
	}
}
