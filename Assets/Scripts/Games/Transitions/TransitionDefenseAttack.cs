using System;
using System.Collections;
using System.Linq;
using Games.Attacks;
using Games.Defenses;
using Games.Global.Entities;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Transitions
{
    public class TransitionDefenseAttack : MonoBehaviour
    {
        private const int durationDefensePhase = 10;

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

        [SerializeField] 
        private HoverDetector hoverDetector;
        
        private int defenseTimer;
        
        string newMap;

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

            if (hoverDetector.currentlyBlocked)
            {
                if (hoverDetector.lastTileWithContent)
                {
                    hoverDetector.lastTileWithContent.content = null;
                    hoverDetector.lastTileWithContent.contentType = GridTileController.TypeData.Empty;
                }

                if (hoverDetector.lastObjectPutInPlay)
                {
                    hoverDetector.lastObjectPutInPlay.SetActive(false);
                }

                if (hoverDetector.objectInHand)
                {
                    hoverDetector.objectInHand.SetActive(false);
                }
            }
            SendGridData();

            while (GameController.mapReceived == null)
            {
                yield return new WaitForSeconds(1);   
            }
            
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
            TowersWebSocket.TowerSender("OTHERS", NetworkingController.CurrentRoomToken,"GRID",stringToSend);
        }
    }
}