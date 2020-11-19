using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Games.Defenses;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using Games.Players;
using Games.Transitions;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Games.Attacks
{
    [Serializable]
    public class TransistionTest
    {
        public string transition;

        public TransistionTest(string transition)
        {
            this.transition = transition;
        }
    }
    public enum TypeData {
        Nothing,
        Group,
        Wall,
        Trap
    }

    public class InitAttackPhase : MonoBehaviour
    {
        [SerializeField] private ObjectsInScene objectsInScene;
        [SerializeField] private InitDefense initDefense;
        [SerializeField] private HoverDetector hoverDetector;

        private string[,] tempMap1;
        private string[,] tempMap2;

        private Monster monster2;

        private bool endOfGeneration = false;

        private string currentMap;

        private void DesactiveDefenseMap()
        {
            if (hoverDetector.objectInHand != null)
            {
                hoverDetector.objectInHand.SetActive(false);
            }

            foreach (GameObject go in initDefense.gridCellList)
            {
                GridTileController gridTileController = go.GetComponent<GridTileController>();

                if (gridTileController.content != null)
                {
                    gridTileController.content.transform.position = new Vector3(0,-10,0);
                    gridTileController.content.SetActive(false);
                }

                go.SetActive(false);
            }
        }

        public async Task StartAttackPhase()
        {
            DesactiveDefenseMap();

            objectsInScene.containerDefense.SetActive(false);
            objectsInScene.containerAttack.SetActive(true);
            
            DataObject.playerInScene.Clear();
            DataObject.monsterInScene.Clear();
            DataObject.objectInScene.Clear();
            
            GameController.currentGameGrid.InitGridData();

            endOfGeneration = true;
        }

        public async Task ActivePlayer()
        {
            objectsInScene.mainCamera.SetActive(false);

            objectsInScene.playerPrefab[GameController.PlayerIndex].playerGameObject.SetActive(true);
            objectsInScene.playerPrefab[GameController.PlayerIndex].canMove = true;

            objectsInScene.playerPrefab[GameController.PlayerIndex].cameraGameObject.SetActive(true);
            MapStats mapStats = initDefense.currentMap.GetComponent<MapStats>();
            objectsInScene.playerPrefab[GameController.PlayerIndex].transform.position = mapStats.spawnPosition.transform.position;
            
            Cursor.lockState = CursorLockMode.Locked;

            DataObject.playerInScene.Add(GameController.PlayerIndex, objectsInScene.playerPrefab[GameController.PlayerIndex]);
            
            while (CurrentRoom.loadGameAttack)
            {
                int nbMin = TransitionMenuGame.timerAttack / 60;
                int nbSec = TransitionMenuGame.timerAttack % 60;
                if (DataObject.playerInScene.Count > 0)
                {
                    DataObject.playerInScene[GameController.PlayerIndex].timerAttack.text =
                        "Timer : " + nbMin + (nbMin > 0 ? "min" : "") + nbSec;
                }

                await Task.Delay(500);
            }
        }

        public bool InstantiateGroupsMonster(GroupsMonster groups, Vector3 position, List<int> equipment)
        {
            InstantiateParameters param;
            Monster monster;
            int nbMonsterInit = 0;
            bool shouldPutKey = false;
            bool hasAKey = false;
            Vector3 origPos = position;
            if (equipment[5] != 0)
            {
                shouldPutKey = true;
            }
            foreach (MonstersInGroup monstersInGroup in groups.monstersInGroupList)
            {
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

                    groups.InitSpecificEquipment(monster, equipment);
                    if (shouldPutKey)
                    {
//                        monster.InitKey(initDefense.maps[initDefense.currentLevel-1].mapsInLevel[NetworkingController.CurrentRoomMapsLevel[initDefense.currentLevel-1]].GetComponent<MapStats>().keyParent);
                        hasAKey = true;
                        shouldPutKey = false;
                    }
                    position = origPos + GroupsPosition.position[nbMonsterInit];

                    DataObject.monsterInScene.Add(monster);
                }
            }

            return hasAKey;
        }
    }
}