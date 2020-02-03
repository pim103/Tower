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
        private GameController gameController;

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
        }

        private void GeneratingMap(int[,] map, int playerIndex)
        {
            int indexMap = playerIndex % 2 == 0 ? 0 : 1;
            GameObject currentMap = objectsInScene.maps[indexMap];
            GameObject newObject;

            for (int i = 0; i < MAP_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE; j++)
                {
                    if (map[i, j] == 0)
                    {
                        newObject = Instantiate(objectsInScene.simpleWall, currentMap.transform);
                        newObject.transform.position = new Vector3(i * 2 + (indexMap * 125), 2, -j * 2);
                    }
                }
            }

            //TODO : SEND SIGNAL WHEN END OF GENERATION
        }

        public void StartAttackPhase()
        {
            objectsInScene.containerDefense.SetActive(false);
            objectsInScene.containerAttack.SetActive(true);
            objectsInScene.mainCamera.SetActive(false);

            int playerIndex = gameController.PlayerIndex;

            objectsInScene.playerExposer[playerIndex].playerCamera.SetActive(true);

            objectsInScene.playerExposer[playerIndex].player.InitPlayerStats(Classes.WARRIOR);

            // TODO : Temp Method
            GenerateArray();

            // TODO : Temp condition
            if (gameController.PlayerIndex % 2 == 0)
            {
                GeneratingMap(tempMap2, playerIndex);
            }
            else
            {
                GeneratingMap(tempMap1, playerIndex);
            }

            // TODO : IF COOP, CHANGE POSITION OF PLAYERS ?
            for (int i = 0; i < objectsInScene.playerExposer.Length; i++)
            {
                if (gameController.idToUserId[i] != null && gameController.idToUserId[i] != "")
                {
                    objectsInScene.playerExposer[i].playerGameObject.SetActive(true);
                    objectsInScene.playerExposer[i].playerMovement.canMove = true;
                }
            }
        }
    }
}