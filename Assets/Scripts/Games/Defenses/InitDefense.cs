using System;
using System.Collections.Generic;
using DeckBuilding;
using Games.Global.Entities;
using Games.Transitions;
using Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Games.Defenses
{
    public class InitDefense : MonoBehaviour
    {
        [SerializeField] 
        public DefenseUIController defenseUIController;

        public int currentLevel = 0;
        
        [SerializeField] 
        private GameObject gridCell;
        private GameObject currentCell;

        [SerializeField] 
        private GameObject defenseCamera;
     
        public List<GameObject> gridCellList;

        public GameGrid defenseGrid;
        
        public void Init()
        {
            if (GameController.currentGameGrid == null)
            {
                return;
            }

            defenseGrid = GameController.currentGameGrid;

            Generate(GameController.currentGameGrid.size);
            Vector3 pos = defenseCamera.transform.position;
            pos.x = (GameController.currentGameGrid.size / 2) * GameGridController.TileOffset;
            pos.y = GameController.currentGameGrid.size * 4;
            pos.z = (GameController.currentGameGrid.size / 2) * GameGridController.TileOffset;

            defenseCamera.transform.localPosition = pos;
            
            defenseUIController.enabled = true;

            currentLevel++;
        }

        public void FillGameGrid()
        {
            foreach (GameObject gridCell in gridCellList)
            {
                GridTileController cellController = gridCell.GetComponent<GridTileController>();
                GridCellData gridCellData = defenseGrid.GetGridCellDataFromCoordinates((int)cellController.coordinates.x, (int)cellController.coordinates.y);

                if (cellController.contentType != GridTileController.TypeData.Empty)
                {
                    switch (cellController.contentType)
                    {
                        case GridTileController.TypeData.Group:
                            CardBehaviorInGame currentCardBehaviorInGame = cellController.content.GetComponent<CardBehaviorInGame>();
                            GroupsMonster groupsMonster = currentCardBehaviorInGame.group;

                            gridCellData.cellType = (int)CellType.ObjectToInstantiate;

                            int meleeId = currentCardBehaviorInGame.meleeWeaponSlot
                                ? currentCardBehaviorInGame.meleeWeaponSlot.GetComponent<CardBehaviorInGame>()
                                    .equipement.id
                                : 0;
                            int rangeId = currentCardBehaviorInGame.rangedWeaponSlot
                                ? currentCardBehaviorInGame.rangedWeaponSlot.GetComponent<CardBehaviorInGame>()
                                    .equipement.id
                                : 0;

                            groupsMonster.AddEquipmentId(meleeId, rangeId);

                            if (currentCardBehaviorInGame.keySlot)
                            {
                                currentCardBehaviorInGame.group.hasKey = true;
                            }

                            gridCellData.groupsMonster = currentCardBehaviorInGame.group;
                            break;
                        case GridTileController.TypeData.Wall:
                            gridCellData.cellType = (int) CellType.Wall;
                            break;
                        case GridTileController.TypeData.Trap:
                            TrapBehavior currentTrapBehavior = cellController.content.GetComponent<TrapBehavior>();
                            gridCellData.cellType = (int) CellType.ObjectToInstantiate;
                            gridCellData.trap = currentTrapBehavior.trapData;
                            gridCellData.rotationY = currentTrapBehavior.rotation * 90;
                            break;
                    }
                }
                
                gridCell.SetActive(false);
                if (cellController.content != null) cellController.content.SetActive(false);
            }
        }

        private void Generate(int size)
        {
            gridCellList = new List<GameObject>();

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    currentCell = Instantiate(gridCell, new Vector3( i * GameGridController.TileOffset,  3f, j * GameGridController.TileOffset), Quaternion.Euler(90,0,0));

                    GridTileController currentTileController = currentCell.GetComponent<GridTileController>();
                    currentTileController.coordinates.x = i;
                    currentTileController.coordinates.y = j;
                    gridCellList.Add(currentCell);
                }
            }
        }
    }
}