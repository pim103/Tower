using System;
using System.Threading.Tasks;
using Games.Defenses;
using Games.Global;
using Games.Global.Entities;
using Games.Transitions;
using Networking.Client.Room;
using UnityEngine;

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
        [SerializeField] private DefenseControls defenseControls;
        [SerializeField] private GameGridController gameGridController;

        public async Task Init()
        {
            if (defenseControls.objectInHand != null)
            {
                defenseControls.objectInHand.SetActive(false);
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

        public async Task PlayAttackPhase()
        {
            objectsInScene.playerPrefab[GameController.PlayerIndex].playerGameObject.SetActive(true);
            objectsInScene.playerPrefab[GameController.PlayerIndex].canMove = true;

            objectsInScene.playerPrefab[GameController.PlayerIndex].camera.gameObject.SetActive(true);
            objectsInScene.playerPrefab[GameController.PlayerIndex].transform.position = gameGridController.startZone.transform.position;
            
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
    }
}