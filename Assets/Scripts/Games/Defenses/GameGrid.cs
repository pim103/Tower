﻿using System;
 using System.Collections.Generic;
 using Games.Global.Entities;
 using UnityEngine;

 namespace Games.Defenses
{
    public enum CellType
    {
        ObjectToInstantiate,
        Wall,
        Hole,
        Spawn,
        End,
        None
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
        public CellType cellType { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public ThemeGrid theme { get; set; } = ThemeGrid.Dungeon;

        public int rotationY { get; set; } = 0;
        public GroupsMonster groupsMonster { get; set; } = null;
        public TrapBehavior trap { get; set; } = null;
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

        public void InitGridData()
        {
            List<GridCellData> gridCellDatas = gridCellDataList.gridCellDatas;

            foreach (GridCellData gridCellData in gridCellDatas)
            {
                switch (gridCellData.cellType)
                {
                    case CellType.ObjectToInstantiate:
                        if (gridCellData.groupsMonster != null)
                        {
//                            gridCellData.groupsMonster.InitGroups(gridCellData.x, gridCellData.y);
                        } 
                        else if (gridCellData.trap != null)
                        {
//                            gridCellData.trap.InitTrap();
                        }
                        break;
                    case CellType.End:
                        break;
                    case CellType.Hole:
                        break;
                    case CellType.Spawn:
                        break;
                    case CellType.Wall:
                        break;
                    case CellType.None:
                        break;
                }
            }
        }
    }
}