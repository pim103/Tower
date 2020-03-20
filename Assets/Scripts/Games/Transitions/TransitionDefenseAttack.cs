using System;
using System.Collections;
using Games.Attacks;
using Games.Defenses;
using Games.Global.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Transitions
{
    public class TransitionDefenseAttack : MonoBehaviour
    {
        private const int durationDefensePhase = 300;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private GameController gameController;
        
        [SerializeField]
        private Text counter;

        [SerializeField]
        private InitAttackPhase initAttackPhase;
        
        [SerializeField]
        private InitDefense initDefense;

        private int defenseTimer;

        private void Start()
        {
            defenseTimer = durationDefensePhase;
        }

        private IEnumerator WaitingEndDefense()
        {
            objectsInScene.waitingCanvasGameObject.SetActive(true);
            objectsInScene.waitingText.text = "";
            objectsInScene.counterText.text = "";
            while(defenseTimer > 0)
            {
                counter.text = defenseTimer.ToString();
                yield return new WaitForSeconds(1);
                defenseTimer--;
            }

            defenseTimer = durationDefensePhase;
            objectsInScene.waitingCanvasGameObject.SetActive(false);

            SendGridData();
            initAttackPhase.StartAttackPhase();
        }

        public void StartDefenseCounter()
        {
            StartCoroutine(WaitingEndDefense());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                SendGridData();
            }
        }

        private void SendGridData()
        {
            string stringToSend = "{\n";
            foreach (var gridCell in initDefense.gridCellList)
            {
                GridTileController cellController = gridCell.GetComponent<GridTileController>();
                stringToSend += "[" + cellController.coordinates.x + ":" + cellController.coordinates.y + ":";
                switch (cellController.contentType)
                {
                    case GridTileController.TypeData.Empty:
                        stringToSend += "0";
                        break;
                    case GridTileController.TypeData.Group:
                        CardBehavior currentCardBehavior = cellController.content.GetComponent<CardBehavior>();
                        stringToSend += "1:" + currentCardBehavior.groupId+":";
                        foreach (var equipement in currentCardBehavior.equipementsList)
                        {
                            stringToSend+=equipement.GetComponent<CardBehavior>().equipement.id+":";
                        }
                        
                        stringToSend = stringToSend.Remove(stringToSend.Length - 1);
                        break;
                    case GridTileController.TypeData.Wall:
                        stringToSend += "2";
                        break;
                    case GridTileController.TypeData.Trap:
                        TrapBehavior currentTrapBehavior = cellController.content.GetComponent<TrapBehavior>();
                        stringToSend += "3:" + (int) currentTrapBehavior.mainType + ":";
                        foreach (var effect in currentTrapBehavior.trapEffects)
                        {
                            stringToSend += (int) effect + ":";
                        }
                        stringToSend = stringToSend.Remove(stringToSend.Length - 1);
                        break;
                }
                stringToSend += "]\n";
            }

            stringToSend += "}";
            Debug.Log(stringToSend);
        }
    }
}