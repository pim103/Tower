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
        private DefenseControls defenseControls;

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
            defenseControls.enabled = true;
            hasValidated = false;
            objectsInScene.waitingText.text = "";
            objectsInScene.counterText.text = "";

            while(defenseTimer > 0 && !hasValidated)
            {
                counter.text = defenseTimer.ToString();
                if (defenseTimer <= 10 && !defenseControls.defenseUiController.keyAlreadyPut)
                {
                    defenseControls.defenseUiController.keyButton.transform.GetComponent<Image>().color = new Color(1,defenseTimer%2,defenseTimer%2,1);
                }

                await Task.Delay(1000);
            }
            Debug.Log("hmm");
            initDefense.FillGameGrid();
            Debug.Log("zapas");
            defenseUI.SetActive(false);
            waitingOtherPlayerPanel.SetActive(true);
            defenseTimer = durationDefensePhase;

            if (defenseControls.currentlyBlocked)
            {
                if (defenseControls.lastTileWithContent)
                {
                    defenseControls.lastTileWithContent.content = null;
                    defenseControls.lastTileWithContent.contentType = GridTileController.TypeData.Empty;
                }

                if (defenseControls.lastObjectPutInPlay)
                {
                    defenseControls.lastObjectPutInPlay.SetActive(false);
                }

                if (defenseControls.objectInHand)
                {
                    defenseControls.objectInHand.SetActive(false);
                }

                if (defenseControls.defenseUiController.keyAlreadyPut)
                {
                    defenseControls.tileHoldingKeyGroup.content.SetActive(false);
                    defenseControls.tileHoldingKeyGroup.content = null;
                    defenseControls.tileHoldingKeyGroup.contentType = GridTileController.TypeData.Empty;
                }
            }
            
            defenseControls.defenseUiController.keyObject.SetActive(false);
            defenseControls.enabled = false;
        }
    }
}