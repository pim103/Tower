using System.Collections.Generic;
using Games.Attacks;
using Games.Defenses.Traps;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using UnityEngine;
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
        
        public const int TileOffset = 4;

        private List<GameObject> currentMap = new List<GameObject>();

        public void InitGridData(GameGrid grid)
        {
            DataObject.playerInScene.Clear();
            DataObject.monsterInScene.Clear();
            DataObject.objectInScene.Clear();
            
            List<GridCellData> gridCellDatas = grid.gridCellDataList.gridCellDatas;
            
            bool foundKey;

            foreach (GridCellData gridCellData in gridCellDatas)
            {
                switch (gridCellData.cellType)
                {
                    case CellType.ObjectToInstantiate:
                        PoolGameObject(gridCellData.x, gridCellData.y, grid.theme, MapThemePrefab.IdWall, Vector3.zero);
                        if (gridCellData.groupsMonster != null)
                        {
                            if (InitGroups(gridCellData.groupsMonster, gridCellData.x, gridCellData.y, TileOffset))
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
                        PoolGameObject(gridCellData.x, gridCellData.y, grid.theme, MapThemePrefab.IdBasicLight, Vector3.zero);
                        break;
                    case CellType.Hole:
                        PoolGameObject(gridCellData.x, gridCellData.y, grid.theme, MapThemePrefab.IdOnlyRoof, Vector3.zero);
                        break;
                    case CellType.Spawn:
                        startZone.transform.position = new Vector3
                        {
                            x = gridCellData.x * TileOffset,
                            y = 1,
                            z = gridCellData.y * TileOffset,
                        };
                        PoolGameObject(gridCellData.x, gridCellData.y, grid.theme, MapThemePrefab.IdBasicLight, Vector3.zero);
                        break;
                    case CellType.Wall:
                        PoolGameObject(gridCellData.x, gridCellData.y, grid.theme, MapThemePrefab.IdPlaceableWall, Vector3.zero);
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

                        PoolGameObject(gridCellData.x, gridCellData.y, grid.theme, idPoolObject, rot);
                        break;
                }
            }
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

            objectPooled.SetActive(true);
        }
        
        public bool InitGroups(GroupsMonster groups, int x, int y, int offset)
        {
            Monster monster;
            int nbMonsterInit = 0;

            Vector3 position = Vector3.zero;
            position.x = x * offset;
            position.z = y * offset;

            foreach (MonstersInGroup monstersInGroup in groups.monstersInGroupList)
            {
                nbMonsterInit = 0;
                
                for (int i = 0; i < monstersInGroup.nbMonster; i++)
                {
                    monster = monstersInGroup.GetMonster();

                    GameObject monsterGameObject = Instantiate(monster.model);
                    monsterGameObject.transform.position = position;
                    
                    monster.IdEntity = DataObject.nbEntityInScene;
                    DataObject.nbEntityInScene++;
                    nbMonsterInit++;

                    MonsterPrefab monsterPrefab = monsterGameObject.GetComponent<MonsterPrefab>();
                    monster.InitMonster(monsterPrefab);
                    monster.InitOriginalWeapon();

                    position += GroupsPosition.position[nbMonsterInit];

                    DataObject.monsterInScene.Add(monster);
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
            pos.z = y * offset;

            Vector3 rot = goTrap.transform.localEulerAngles;
            rot.y = rotY;
            goTrap.transform.localEulerAngles = rot;

            trapBehavior.SetAndActiveTraps(trap);
            goTrap.SetActive(true);
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