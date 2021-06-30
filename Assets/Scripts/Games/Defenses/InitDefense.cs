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

        [SerializeField] private GameObject[] maps;
        
        public void Init()
        {
            /*if (GameController.currentGameGrid == null)
            {
                return;
            }*/

            defenseGrid = GameController.currentGameGrid;
            
            maps[0].SetActive(true);
            
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
            Debug.Log(gridCellList.Count);
            foreach (GameObject gridCell in gridCellList)
            {
                Debug.Log("inforeach");
                if (gridCell == null)
                {
                    continue;
                }
                Debug.Log("beforeGetComponent");
                GridTileController cellController = gridCell.GetComponent<GridTileController>();
                if (cellController == null)
                {
                    continue;
                }
                Debug.Log("beforeGridCellData");
                GridCellData gridCellData = defenseGrid.GetGridCellDataFromCoordinates((int)cellController.coordinates.x, (int)cellController.coordinates.y);
                Debug.Log("gotCellController");
                if (cellController.contentType != GridTileController.TypeData.Empty)
                {
                    Debug.Log("firstcond");
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
                                Debug.Log("sentKey");
                                currentCardBehaviorInGame.group.hasKey = true;
                            }

                            gridCellData.groupsMonster = currentCardBehaviorInGame.group;
                            break;
                        case GridTileController.TypeData.Wall:
                            Debug.Log("wall");
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
                Debug.Log("aftercond");
                
                gridCell.SetActive(false);
                if (cellController.content != null) cellController.content.SetActive(false);
                Debug.Log("lafin");
            }
            Debug.Log("lavraiefin");
        }

        private void Generate(int size)
        {
            gridCellList = new List<GameObject>();

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    int yPos = 3;
                    int xPos = i % (size / 2);
                    if (i >= size / 2) yPos += 4;

                    currentCell = Instantiate(gridCell, new Vector3(xPos * GameGridController.TileOffset, yPos, j * GameGridController.TileOffset), Quaternion.Euler(90, 0, 0));
                    GridTileController currentTileController = currentCell.GetComponent<GridTileController>();
                    currentTileController.coordinates.x = i;
                    currentTileController.coordinates.y = j;
                    gridCellList.Add(currentCell);
                }
            }
        }
    }
}