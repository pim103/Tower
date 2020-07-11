using System.Collections.Generic;
 using System.Collections;
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

        private void DesactiveAllGameObject()
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

        public IEnumerator WaitingForDefensePhase()
        {
            DesactiveAllGameObject();
            while (!CurrentRoom.loadGameDefense)
            {
                yield return new WaitForSeconds(0.5f);
            }

            objectsInScene.containerAttack.SetActive(false);
            objectsInScene.containerDefense.SetActive(true);
            initDefense.Init();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            CurrentRoom.loadGameDefense = false;
            CurrentRoom.loadGameAttack = false;
            Cursor.lockState = CursorLockMode.None;
            
            if (initDefense.currentLevel < initDefense.maps.Length)
            {
                TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "setDefenseReady", "null");
                StartCoroutine(WaitingForDefensePhase());
            }
            else
            {
                TowersWebSocket.TowerSender("OTHERS", NetworkingController.CurrentRoomToken, "Player", "HasWon", null);
                TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken, "Player", "SendDeath", null);
            }
        }
    }
}