using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Games.Attacks
{
    public enum TypeData {
        Nothing,
        Trap,
        Group,
        Wall
    }

    public class InitAttackPhase : MonoBehaviour
    {
        private static int idMobInit = 0;
        public const int MAP_SIZE = 25;

        [SerializeField] 
        private ObjectPooler objectPoolerDefense;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private ScriptsExposer se;

        private string[,] tempMap1;
        private string[,] tempMap2;

        private Monster monster2;

        private bool endOfGeneration = false;

        public void GenerateArray()
        {
            tempMap1 = new string[MAP_SIZE, MAP_SIZE];
            tempMap2 = new string[MAP_SIZE, MAP_SIZE];

            for (int i = 0; i < MAP_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE; j++)
                {
                    int rand = Random.Range(1, 11) % 10;

//                    tempMap1[i, j] = (rand == 0 ? (int)TypeData.Wall : (int)TypeData.Nothing) + ":" + "" + ":" + "[]";
//                    tempMap2[i, j] = (rand == 0 ? (int)TypeData.Wall : (int)TypeData.Nothing) + ":" + "" + ":" + "[]";

                    tempMap1[i, j] = (int)TypeData.Nothing + ":" + "" + ":" + "[]";
                    tempMap2[i, j] = (int)TypeData.Nothing + ":" + "" + ":" + "[]";
                }
            }

            tempMap2[MAP_SIZE / 4, MAP_SIZE / 4] = (int)TypeData.Group + ":" + "1" + ":" + "[]";
            tempMap2[MAP_SIZE - 3, MAP_SIZE - 3] = (int)TypeData.Group + ":" + "1" + ":" + "[3]";
        }

        public (TypeData, int, List<int>) ParseString(string lineToParse)
        {
            TypeData type;
            int idElement;
            List<int> idEquipements = new List<int>();

            int firstColon = lineToParse.IndexOf(':');
            int secondColon = lineToParse.IndexOf(':', firstColon + 1);

            type = (TypeData)Int32.Parse(lineToParse.Substring(0, firstColon).Trim());

            if (firstColon == secondColon - 1)
            {
                return (type, 0, idEquipements);
            }

            string idElementString = lineToParse.Substring(firstColon + 1, secondColon - (firstColon + 1)).Trim();
            idElement = Int32.Parse(idElementString);

            int firstBracket = lineToParse.IndexOf('[');
            int secondBracket = lineToParse.IndexOf(']');

            if (firstBracket == secondBracket - 1)
            {
                return (type, idElement, idEquipements);
            }

            string lineArray = lineToParse.Substring(firstBracket + 1);
            string valueToParse = lineArray.Trim();
            int indexComma;

            bool wantToLeave = false;
            do
            {
                indexComma = valueToParse.IndexOf(',');
                if (indexComma == -1)
                {
                    indexComma = valueToParse.Length - 1;
                    wantToLeave = true;
                }

                idEquipements.Add(Int32.Parse(valueToParse.Substring(0, indexComma)));

                if (!wantToLeave)
                {    
                    valueToParse = valueToParse.Substring(indexComma + 1);
                }
            } while (!wantToLeave);

            return (type, idElement, idEquipements);
        }

        private void GeneratingMap(string[,] map, int playerIndex)
        {
            int indexMap = playerIndex % 2 == 0 ? 0 : 1;
            GameObject currentMap = objectsInScene.maps[indexMap];

            TypeData type;
            int idElement;
            List<int> idEquipements;

            for (int i = 0; i < MAP_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE; j++)
                {
                    (type, idElement, idEquipements) = ParseString(map[i,j]);

                    switch (type)
                    {
                        case TypeData.Nothing:
                            break;
                        case TypeData.Group:
                            Vector3 newPos = new Vector3(i * 2 + (indexMap * 125), 1.5f, -j * 2);
                            GroupsMonster groups = DataObject.MonsterList.GetGroupsMonsterById(idElement);
                            InstantiateGroupsMonster(groups, newPos, idEquipements);
                            break;
                        case TypeData.Trap:
                            break;
                        case TypeData.Wall:
                            GameObject wall = objectPoolerDefense.GetPooledObject(0);
                            wall.transform.position = new Vector3(i * 2 + (indexMap * 125), 1.5f, -j * 2);
                            wall.SetActive(true);
                            break;
                    }
                }
            }
        }

        private void Update()
        {
            if (endOfGeneration)
            {
                if (DataObject.monsterInScene != null)
                {
                    foreach (Monster monster in DataObject.monsterInScene)
                    {
                        monster.BasicAttack();
                    }   
                }
            }
        }

        public void StartAttackPhase()
        {
            objectsInScene.containerDefense.SetActive(false);
            objectsInScene.containerAttack.SetActive(true);
            
            // TODO : Temp Method
            GenerateArray();

            // TODO : Temp condition
            if (GameController.PlayerIndex % 2 == 0)
            {
                GeneratingMap(tempMap2, GameController.PlayerIndex);
            }
            else
            {
                GeneratingMap(tempMap1, GameController.PlayerIndex);
            }

            ActivePlayer();

            endOfGeneration = true;
        }

        private void ActivePlayer()
        {
            objectsInScene.mainCamera.SetActive(false);

            objectsInScene.playerExposer[GameController.PlayerIndex].playerGameObject.SetActive(true);
            objectsInScene.playerExposer[GameController.PlayerIndex].playerPrefab.canMove = true;

            objectsInScene.playerExposer[GameController.PlayerIndex].playerCamera.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;

            DataObject.playerInScene.Add(GameController.PlayerIndex, objectsInScene.playerExposer[GameController.PlayerIndex].playerPrefab);
        }

        public void InstantiateGroupsMonster(GroupsMonster groups, Vector3 position, List<int> equipment)
        {
            InstantiateParameters param;
            Monster monster;
            int nbMonsterInit = 0;

            Vector3 origPos = position;

            foreach (KeyValuePair<int, int> mobs in groups.monsterInGroups)
            {
                for (int i = 0; i < mobs.Value; i++)
                {
                    monster = DataObject.MonsterList.GetMonsterById(mobs.Key);

                    GameObject monsterGameObject = Instantiate(monster.model);
                    monsterGameObject.transform.position = position;
                    
                    monster.IdEntity = idMobInit;
                    idMobInit++;
                    nbMonsterInit++;

                    MonsterPrefab monsterPrefab = monsterGameObject.GetComponent<MonsterPrefab>();
                    monster.SetMonsterPrefab(monsterPrefab);
                    monsterPrefab.SetMonster(monster);

                    groups.InitSpecificEquipment(monster, equipment);

                    position = origPos + GroupsPosition.position[nbMonsterInit];

                    DataObject.monsterInScene.Add(monster);
                }
            }
        }
    }
}