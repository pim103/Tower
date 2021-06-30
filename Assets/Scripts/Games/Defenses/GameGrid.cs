﻿using System;
 using System.Collections.Generic;
 using System.Linq;
 using Games.Defenses.Traps;
 using Games.Global.Entities;
 using UnityEngine;

 namespace Games.Defenses
{
    public enum CellType
    {
        None,
        ObjectToInstantiate,
        Wall,
        Hole,
        Spawn,
        End
    }

    public enum ThemeGrid
    {
        Dungeon
    }

    [Serializable]
    public class GridCellDataList
    {
        public List<GridCellData> gridCellDatas { get; set; }
    }
    
    [Serializable]
    public class GridCellData
    {
        // En vrai, c'est un cellType mais le serveur tu vois
        public int cellType { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public int rotationY { get; set; } = 0;
        public GroupsMonster groupsMonster { get; set; } = null;
        public TrapData trap { get; set; } = null;
    }

    [Serializable]
    public class GameGrid
    {
        public int index { get; set; }
        public int size { get; set; }
        public GridCellDataList gridCellDataList { get; set; }

        // En vrai, c'est un Theme mais le serveur tu vois
        public int theme { get; set; } = (int)ThemeGrid.Dungeon;

        public void DisplayGridData()
        {
            foreach (GridCellData g in gridCellDataList.gridCellDatas)
            {
                //Debug.Log(" x : " + g.x + " y : " + g.y + " type : " + g.cellType);
            }
        }

        public GridCellData GetGridCellDataFromCoordinates(int x, int y)
        {
            if (gridCellDataList == null)
            {
                return null;
            }

            return gridCellDataList.gridCellDatas.First(data => data.x == x && data.y == y);
        }
    }
}