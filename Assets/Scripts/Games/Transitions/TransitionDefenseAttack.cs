using System;
using System.Collections;
using System.Linq;
using Games.Attacks;
using Games.Defenses;
using Games.Global.Entities;
using Networking.Client;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Transitions
{
    public class TransitionDefenseAttack : MonoBehaviour
    {
        private const int durationDefensePhase = 20;

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
            // \"_TARGET\":\"ALL\", \"_ARGS\":\"null\",
            string stringToSend = "{";
            foreach (var gridCell in initDefense.gridCellList)
            {
                GridTileController cellController = gridCell.GetComponent<GridTileController>();
                if (cellController.contentType != GridTileController.TypeData.Empty)
                {
                    stringToSend += cellController.coordinates.x + ":" + cellController.coordinates.y + ":";
                    switch (cellController.contentType)
                    {
                        case GridTileController.TypeData.Group:
                            CardBehavior currentCardBehavior = cellController.content.GetComponent<CardBehavior>();
                            stringToSend += "1:" + currentCardBehavior.groupId + ":[";
                            if (currentCardBehavior.meleeWeaponSlot)
                            {
                                stringToSend += currentCardBehavior.meleeWeaponSlot.GetComponent<CardBehavior>()
                                                    .equipement.id + ",";
                            }
                            else
                            {
                                stringToSend += "0,";
                            }
                            if (currentCardBehavior.rangedWeaponSlot)
                            {
                                stringToSend += currentCardBehavior.rangedWeaponSlot.GetComponent<CardBehavior>()
                                                    .equipement.id + ",";
                            }
                            else
                            {
                                stringToSend += "0,";
                            }
                            if (currentCardBehavior.helmetSlot)
                            {
                                stringToSend += currentCardBehavior.helmetSlot.GetComponent<CardBehavior>()
                                                    .equipement.id + ",";
                            }
                            else
                            {
                                stringToSend += "0,";
                            }
                            if (currentCardBehavior.chestSlot)
                            {
                                stringToSend += currentCardBehavior.chestSlot.GetComponent<CardBehavior>()
                                                    .equipement.id + ",";
                            }
                            else
                            {
                                stringToSend += "0,";
                            }
                            if (currentCardBehavior.grievesSlot)
                            {
                                stringToSend += currentCardBehavior.grievesSlot.GetComponent<CardBehavior>()
                                                    .equipement.id;
                            }
                            else
                            {
                                stringToSend += "0";
                            }
                            
                            /*stringToSend += currentCardBehavior.rangedWeaponSlot.GetComponent<CardBehavior>().equipement.id + ",";
                            stringToSend += currentCardBehavior.helmetSlot.GetComponent<CardBehavior>().equipement.id + ",";
                            stringToSend += currentCardBehavior.chestSlot.GetComponent<CardBehavior>().equipement.id + ",";
                            stringToSend += currentCardBehavior.grievesSlot.GetComponent<CardBehavior>().equipement.id ;*/

                            stringToSend += "]";
                            break;
                        case GridTileController.TypeData.Wall:
                            stringToSend += "2";
                            break;
                        case GridTileController.TypeData.Trap:
                            TrapBehavior currentTrapBehavior = cellController.content.GetComponent<TrapBehavior>();
                            stringToSend += "3:" + (int) currentTrapBehavior.mainType + ":[";
                            if (currentTrapBehavior.trapEffects.Any())
                            {
                                foreach (var effect in currentTrapBehavior.trapEffects)
                                {
                                    stringToSend += (int) effect + ",";
                                }
                                stringToSend = stringToSend.Remove(stringToSend.Length - 1);
                            }
                            stringToSend += "]";
                            break;
                    }

                    stringToSend += ";";
                }
            }

            stringToSend += "}";
            //Debug.Log(stringToSend);
            TowersWebSocket.TowerSender("ALL", gameController.networking.roomId,"GRID",stringToSend);
        }
    }
}