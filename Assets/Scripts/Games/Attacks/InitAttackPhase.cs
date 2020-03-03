using Games.Global;
using Games.Global.Entities;
using Games.Players;
using Scripts.Games;
using UnityEngine;

namespace Games.Attacks
{
    public class InitAttackPhase : MonoBehaviour
    {
        public const int MAP_SIZE = 25;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private ScriptsExposer se;

        private int[,] tempMap1;
        private int[,] tempMap2;

        public void GenerateArray()
        {
            tempMap1 = new int[MAP_SIZE, MAP_SIZE];
            tempMap2 = new int[MAP_SIZE, MAP_SIZE];

            for (int i = 0; i < MAP_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE; j++)
                {
                    tempMap1[i, j] = Random.Range(1, 11) % 10;
                    tempMap2[i, j] = Random.Range(1, 11) % 10;
                }
            }

            objectsInScene.playerExposer[GameController.PlayerIndex].playerGameObject.SetActive(true);
            objectsInScene.playerExposer[GameController.PlayerIndex].playerMovement.canMove = true;
        }

        private void GeneratingMap(int[,] map, int playerIndex)
        {
            int indexMap = playerIndex % 2 == 0 ? 0 : 1;
            GameObject currentMap = objectsInScene.maps[indexMap];

            Monster monster = se.dm.monsterList.GetMonsterById(1);

            InstantiateParameters param = new InstantiateParameters();
            param.item = monster;
            param.type = TypeItem.Monster;

            monster.InstantiateModel(param, Vector3.zero);
            
            Monster monster2 = se.dm.monsterList.GetMonsterById(1);

            param.item = monster2;
            param.type = TypeItem.Monster;

            monster2.InstantiateModel(param, Vector3.one * 10);
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
        }
    }
}