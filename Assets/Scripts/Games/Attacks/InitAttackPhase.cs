using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;
using Debug = UnityEngine.Debug;
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
            tempMap2[MAP_SIZE - 3, MAP_SIZE - 3] = (int)TypeData.Group + ":" + "1" + ":" + "[]";

            objectsInScene.playerExposer[GameController.PlayerIndex].playerGameObject.SetActive(true);
            objectsInScene.playerExposer[GameController.PlayerIndex].playerMovement.canMove = true;
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

                Debug.Log(valueToParse.Substring(0, indexComma));
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
                            groups.InstantiateMonster(newPos, idEquipements);
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
            /*
            Monster monster = DataObject.MonsterList.GetMonsterById(1);

            InstantiateParameters param = new InstantiateParameters();
            param.item = monster;
            param.type = TypeItem.Monster;

            monster.InstantiateModel(param, Vector3.zero);

            monster2 = DataObject.MonsterList.GetMonsterById(1);

            param.item = monster2;
            param.type = TypeItem.Monster;

            monster2.InstantiateModel(param, Vector3.one * 10);
            monster2.InitWeapon(2);
            */
        }

        private void Update()
        {
            if (endOfGeneration)
            {
//                monster2.BasicAttack();
            }
        }

        public void StartAttackPhase()
        {
            objectsInScene.containerDefense.SetActive(false);
            objectsInScene.containerAttack.SetActive(true);
            objectsInScene.mainCamera.SetActive(false);

            int playerIndex = GameController.PlayerIndex;

            objectsInScene.playerExposer[playerIndex].playerCamera.SetActive(true);

            objectsInScene.playerExposer[playerIndex].player.InitPlayerStats(Classes.WARRIOR);

            // TODO : Temp Method
            GenerateArray();

            // TODO : Temp condition
            if (GameController.PlayerIndex % 2 == 0)
            {
                GeneratingMap(tempMap2, playerIndex);
            }
            else
            {
                GeneratingMap(tempMap1, playerIndex);
            }

            endOfGeneration = true;
        }
    }
}