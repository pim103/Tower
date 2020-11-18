﻿using System;
 using System.Collections.Generic;
 using UnityEngine;

 namespace Games.Defenses
{
    public enum CellType
    {
        Group,
        Trap,
        Wall,
        Hole,
        Spawn,
        End,
        None
    }

    [Serializable]
    public class GridCellDataList
    {
        public List<GridCellData> gridCellDatas { get; set; }
    }
    
    [Serializable]
    public class GridCellData
    {
        public CellType cellType { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    [Serializable]
    public class GameGrid
    {
        public int index { get; set; }
        public int size { get; set; }
        public GridCellDataList gridCellDataList { get; set; }

        public void DisplayGridData()
        {
            foreach (GridCellData g in gridCellDataList.gridCellDatas)
            {
                Debug.Log(" x : " + g.x + " y : " + g.y + " type : " + g.cellType);
            }
        }
    }
}