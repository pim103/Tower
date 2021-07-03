using System.Collections.Generic;
 using System.Collections;
using System.Linq;
using Games.Defenses;
using Games.Global;
using Games.Global.Entities;
using Games.Players;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Games.Attacks
{
    public class EndCube : MonoBehaviour
    {
        [SerializeField] 
        private ObjectsInScene objectsInScene;
        [SerializeField] 
        private InitDefense initDefense;

        public void DesactiveAllGameObject()
        {
            foreach (GameObject go in DataObject.objectInScene)
            {
                go.transform.position = Vector3.zero;
                go.SetActive(false);
            }

            foreach (Monster monster in DataObject.monsterInScene)
            {
                monster.entityPrefab.gameObject.SetActive(false);
            }

            foreach (KeyValuePair<int, PlayerPrefab> players in DataObject.playerInScene)
            {
                players.Value.Reset();
            }

            initDefense.defenseUIController.enabled = false;
            objectsInScene.playerPrefab[GameController.PlayerIndex].playerGameObject.SetActive(false);
            objectsInScene.playerPrefab[GameController.PlayerIndex].canMove = false;
            objectsInScene.mainCamera.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            Cursor.lockState = CursorLockMode.None;

            int mapPlayed = 2;

            int playerLayer = LayerMask.NameToLayer("Player");

            if (other.gameObject.layer != playerLayer)
            {
                return;
            }

            // TODO : set nb map played
            if (initDefense.currentLevel < mapPlayed)
            {
                GameController.WaitingOpponent();
                DataObject.playerInScene.First().Value.canDoSomething = false;
                TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "setDefenseReady", "null");
            }
            else
            {
                TowersWebSocket.TowerSender("OTHERS", NetworkingController.CurrentRoomToken, "Player", "HasWon", null);
                TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken, "Player", "SendDeath", null);
            }
        }
    }
}