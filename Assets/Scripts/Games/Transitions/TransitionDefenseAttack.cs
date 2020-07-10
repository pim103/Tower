using System;
using System.Collections;
using System.Linq;
using DeckBuilding;
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
        private const int durationDefensePhase = 90;

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
            objectsInScene.waitingText.text = "";
            objectsInScene.counterText.text = "";
            while(defenseTimer > 0)
            {
                counter.text = defenseTimer.ToString();
                yield return new WaitForSeconds(1);
                defenseTimer--;
            }

            defenseTimer = durationDefensePhase;

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
                            CardBehaviorInGame currentCardBehaviorInGame = cellController.content.GetComponent<CardBehaviorInGame>();
                            stringToSend += "1:" + currentCardBehaviorInGame.groupId + ":[";
                            if (currentCardBehaviorInGame.meleeWeaponSlot)
                            {
                                stringToSend += currentCardBehaviorInGame.meleeWeaponSlot.GetComponent<CardBehaviorInGame>()
                                                    .equipement.id + ",";
                            }
                            else
                            {
                                stringToSend += "0,";
                            }
                            if (currentCardBehaviorInGame.rangedWeaponSlot)
                            {
                                stringToSend += currentCardBehaviorInGame.rangedWeaponSlot.GetComponent<CardBehaviorInGame>()
                                                    .equipement.id + ",";
                            }
                            else
                            {
                                stringToSend += "0,";
                            }
                            if (currentCardBehaviorInGame.helmetSlot)
                            {
                                stringToSend += currentCardBehaviorInGame.helmetSlot.GetComponent<CardBehaviorInGame>()
                                                    .equipement.id + ",";
                            }
                            else
                            {
                                stringToSend += "0,";
                            }
                            if (currentCardBehaviorInGame.chestSlot)
                            {
                                stringToSend += currentCardBehaviorInGame.chestSlot.GetComponent<CardBehaviorInGame>()
                                                    .equipement.id + ",";
                            }
                            else
                            {
                                stringToSend += "0,";
                            }
                            if (currentCardBehaviorInGame.grievesSlot)
                            {
                                stringToSend += currentCardBehaviorInGame.grievesSlot.GetComponent<CardBehaviorInGame>()
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
                                stringToSend += currentTrapBehavior.rotation + ",";
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
            Debug.Log(stringToSend);
            TowersWebSocket.TowerSender("OTHERS", NetworkingController.CurrentRoomToken,"GRID",stringToSend);
        }
    }
}