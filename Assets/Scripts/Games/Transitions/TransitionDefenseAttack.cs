using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeckBuilding;
using FullSerializer;
using Games.Attacks;
using Games.Defenses;
using Games.Global.Entities;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Games.Transitions
{
    public class TransitionDefenseAttack : MonoBehaviour
    {
        private const int durationDefensePhase = 30;

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

        [SerializeField] private Button validateButton;
        [SerializeField] private GameObject waitingOtherPlayerPanel;
        [SerializeField] private GameObject defenseUI;
        public static int defenseTimer;
        private bool hasValidated;
        
        string newMap;

        private void Start()
        {
            defenseTimer = durationDefensePhase;
            validateButton.onClick.AddListener(delegate { hasValidated = true; });
        }

        public async Task PlayDefensePhase()
        {
            defenseUI.SetActive(true);
            waitingOtherPlayerPanel.SetActive(false);
            hoverDetector.enabled = true;
            hasValidated = false;
            objectsInScene.waitingText.text = "";
            objectsInScene.counterText.text = "";

            while(defenseTimer > 0 && !hasValidated)
            {
                counter.text = defenseTimer.ToString();
                if (defenseTimer <= 10 && !hoverDetector.defenseUiController.keyAlreadyPut)
                {
                    hoverDetector.defenseUiController.keyButton.transform.GetComponent<Image>().color = new Color(1,defenseTimer%2,defenseTimer%2,1);
                }

                await Task.Delay(500);
            }

            defenseUI.SetActive(false);
            waitingOtherPlayerPanel.SetActive(true);
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

                if (hoverDetector.defenseUiController.keyAlreadyPut)
                {
                    hoverDetector.tileHoldingKeyGroup.content.SetActive(false);
                    hoverDetector.tileHoldingKeyGroup.content = null;
                    hoverDetector.tileHoldingKeyGroup.contentType = GridTileController.TypeData.Empty;
                }
            }
            
            hoverDetector.defenseUiController.keyObject.SetActive(false);
            hoverDetector.enabled = false;
        }
    }
}