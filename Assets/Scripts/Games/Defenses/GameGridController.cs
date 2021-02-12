using System;
using System.Collections.Generic;
using Games.Attacks;
using Games.Defenses.Traps;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Games.Defenses
{
    public static class MapThemePrefab
    {
        public static int IdPlaceableWall = 5;
        public static int IdOnlyRoof = 4;
        public static int IdWall = 3;
        public static int IdAngleWall = 2;
        public static int IdBasicLight = 1;
        public static int IdBasic = 0;
    }
    
    public class GameGridController : MonoBehaviour
    {
        [SerializeField] public GameObject endZone;
        [SerializeField] public GameObject startZone;
        [SerializeField] private KeyBehavior keyBehaviorEndZone;

        [SerializeField] private ObjectPooler dungeonObjectPooler;
        [SerializeField] private ObjectPooler trapPooler;
        [SerializeField] private NavMeshSurface navMeshSurface;
        
        public const int TileOffset = 4;

        private List<GameObject> currentMap = new List<GameObject>();

        private GameGrid GenerateGrid()
        {
            GameGrid grid = new GameGrid();
            grid.gridCellDataList = new GridCellDataList();
            grid.gridCellDataList.gridCellDatas = new List<GridCellData>();
            
            grid.size = 20;
            for (int i = 0; i < grid.size; ++i)
            {
                for (int j = 0; j < grid.size; ++j)
                {
                    CellType cellType = CellType.None;

                    if (i == 1 && j == 1)
                    {
                        cellType = CellType.Spawn;
                    } else if (i == grid.size-2 && j == grid.size-2)
                    {
                        cellType = CellType.End;
                    }
                    
                    grid.gridCellDataList.gridCellDatas.Add(new GridCellData
                    {
                        x = i,
                        y = j,
                        cellType = (int)cellType
                    });
                }
            }

            return grid;
        }

        public void GenerateAndInitFakeGrid()
        {
            GameController.currentGameGrid = GenerateGrid();
            InitGridData(GameController.currentGameGrid);
        }
        
        public void InitGridData(GameGrid grid)
        {
            DataObject.playerInScene.Clear();
            DataObject.monsterInScene.Clear();
            DataObject.objectInScene.Clear();
            
            List<GridCellData> gridCellDatas = grid.gridCellDataList.gridCellDatas;
            
            bool foundKey;

            Debug.Log("==================================== START ====================================");
            navMeshSurface.enabled = false;
            foreach (GridCellData gridCellData in gridCellDatas)
            {
                Debug.Log(gridCellData.x + " " + gridCellData.y + " " + gridCellData.cellType);
                
                switch ((CellType) gridCellData.cellType)
                {
                    case CellType.ObjectToInstantiate:
                        PoolGameObject(gridCellData.x, gridCellData.y, (ThemeGrid)grid.theme, MapThemePrefab.IdBasicLight, Vector3.zero);
                        if (gridCellData.groupsMonster != null)
                        {
                            if (InitGroups(gridCellData.groupsMonster, gridCellData.x, gridCellData.y, TileOffset, currentMap))
                            {
                                foundKey = true;
                            }
                        }
                        else if (gridCellData.trap != null)
                        {
                            InitTrap(gridCellData.trap, gridCellData.x, gridCellData.y, gridCellData.rotationY, TileOffset);
                        }
                        break;
                    case CellType.End:
                        Vector3 pos = endZone.transform.position;
                        pos.x = gridCellData.x * TileOffset;
                        pos.y = 0;
                        pos.z = gridCellData.y * TileOffset;

                        endZone.transform.position = pos;
                        endZone.SetActive(true);
                        PoolGameObject(gridCellData.x, gridCellData.y, (ThemeGrid)grid.theme, MapThemePrefab.IdBasicLight, Vector3.zero);
                        break;
                    case CellType.Hole:
                        PoolGameObject(gridCellData.x, gridCellData.y, (ThemeGrid)grid.theme, MapThemePrefab.IdOnlyRoof, Vector3.zero);
                        break;
                    case CellType.Spawn:
                        startZone.transform.position = new Vector3
                        {
                            x = gridCellData.x * TileOffset,
                            y = 1,
                            z = gridCellData.y * TileOffset,
                        };
                        PoolGameObject(gridCellData.x, gridCellData.y, (ThemeGrid)grid.theme, MapThemePrefab.IdBasicLight, Vector3.zero);
                        break;
                    case CellType.Wall:
                        PoolGameObject(gridCellData.x, gridCellData.y, (ThemeGrid)grid.theme, MapThemePrefab.IdPlaceableWall, Vector3.zero);
                        break;
                    case CellType.None:
                        int x = gridCellData.x;
                        int y = gridCellData.y;

                        Vector3 rot = Vector3.zero;
                        int idPoolObject = 0;
                        
                        if ((x == 0 && y == 0) || (x == 0 && y == grid.size - 1) ||
                            (x == grid.size - 1 && y == grid.size - 1) || (x == grid.size - 1 && y == 0))
                        {
                            rot = FindRotation(x, y, grid.size - 1);
                            idPoolObject = MapThemePrefab.IdAngleWall;
                        } else if (x == 0 || y == 0 || x == grid.size - 1 || y == grid.size - 1)
                        {
                            rot = FindRotation(x, y, grid.size - 1);
                            idPoolObject = MapThemePrefab.IdWall;
                        }
                        else
                        {
                            idPoolObject = MapThemePrefab.IdBasic;
                        }

                        PoolGameObject(gridCellData.x, gridCellData.y, (ThemeGrid)grid.theme, idPoolObject, rot);
                        break;
                }
            }

            Debug.Log("==================================== END ====================================");
            navMeshSurface.enabled = true;
            navMeshSurface.BuildNavMesh();
        }

        private Vector3 FindRotation(int x, int y, int size)
        {
            Vector3 rot = Vector3.zero;

            if (x == 0 && y != 0)
            {
                rot.y = 90;
            }
            else if (x == size)
            {
                rot.y = -90;
            }

            if (x != 0 && y == size)
            {
                rot.y = 180;
            }
            
            return rot;
        }

        private void PoolGameObject(int x, int y, ThemeGrid themeGrid, int idPoolObject, Vector3 rot)
        {
            ObjectPooler objectPooler = null;

            switch (themeGrid)
            {
                case ThemeGrid.Dungeon:
                    objectPooler = dungeonObjectPooler;
                    break;
            }

            if (objectPooler == null)
            {
                return;
            }
            
            GameObject objectPooled = objectPooler.GetPooledObject(idPoolObject);
            Vector3 pos = objectPooled.transform.position;
            pos.x = x * TileOffset;
            pos.z = y * TileOffset;
            objectPooled.transform.position = pos;

            if (rot != Vector3.zero)
            {
                objectPooled.transform.localEulerAngles = rot;
            }

            currentMap.Add(objectPooled);
            objectPooled.SetActive(true);
        }
        
        public static bool InitGroups(GroupsMonster groups, int x, int y, int offset = 1, List<GameObject> currentMap = null)
        {
            Monster monster;
            int nbMonsterInit = 0;

            foreach (MonstersInGroup monstersInGroup in groups.monstersInGroupList)
            {
                nbMonsterInit = 0;
                
                for (int i = 0; i < monstersInGroup.nbMonster; i++)
                {
                    Vector3 position = Vector3.zero;
                    position.x = x * offset;
                    position.y = 1.5f;
                    position.z = y * offset;
                    
                    monster = monstersInGroup.GetMonster();

                    GameObject monsterGameObject = Instantiate(DataObject.MonsterList.GetMonsterById(monster.id).model);
                    
                    monster.IdEntity = DataObject.nbEntityInScene;
                    DataObject.nbEntityInScene++;
                    nbMonsterInit++;

                    MonsterPrefab monsterPrefab = monsterGameObject.GetComponent<MonsterPrefab>();
                    monster.InitMonster(monsterPrefab);
                    monster.InitOriginalWeapon();

                    position += GroupsPosition.position[nbMonsterInit];
                    monsterGameObject.transform.position = position;
                    Debug.Log(monsterGameObject.transform.position);

                    DataObject.monsterInScene.Add(monster);
                    currentMap?.Add(monsterGameObject);
                }
            }

            return groups.hasKey;
        }

        public void InitTrap(TrapData trap, int x, int y, int rotY, int offset)
        {
            GameObject goTrap = trapPooler.GetPooledObject(0);

            TrapBehavior trapBehavior = goTrap.GetComponent<TrapBehavior>();

            Vector3 pos = Vector3.zero;
            pos.x = x * offset;
            pos.y = 0.5f;
            pos.z = y * offset;
            goTrap.transform.position = pos;

            Vector3 rot = goTrap.transform.localEulerAngles;
            rot.y = rotY;
            goTrap.transform.localEulerAngles = rot;

            trapBehavior.SetAndActiveTraps(trap);
            goTrap.SetActive(true);
            currentMap.Add(goTrap);
        }

        public void DesactiveMap()
        {
            foreach (GameObject go in currentMap)
            {
                go.SetActive(false);
            }
        }
    }
}