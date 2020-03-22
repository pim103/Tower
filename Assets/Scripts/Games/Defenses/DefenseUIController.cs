using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Defenses
{
    public class DefenseUIController : MonoBehaviour
    {
        [SerializeField] 
        private Button wallButton;
        
        [SerializeField]
        private Button[] trapButtons;

        [SerializeField] 
        public Text wallButtonText;
        
        [SerializeField] 
        private InitDefense initDefense;

        [SerializeField] 
        private ObjectPooler defensePooler;

        [SerializeField] 
        private HoverDetector hoverDetector;

        [SerializeField] 
        private GameObject cardPrefab;
        
        public int currentWallNumber;
        private int currentWallType;

        public MobDecklist mobDecklist;
        public List<GroupsMonster> mobDeckContent;
        public List<Equipement> equipmentDeckContent;

        [SerializeField] 
        private GameObject[] mobCardContainers;
        
        [SerializeField] 
        private GameObject[] equipementCardContainers;
        private void Start()
        {
            wallButton.onClick.AddListener(PutWallInHand);

            foreach (var button in trapButtons)
            {
                button.transform.GetChild(0).GetComponent<Text>().text =
                    button.gameObject.GetComponent<TrapBehavior>().mainType.ToString();
                button.onClick.AddListener(delegate
                {
                    PutTrapInHand(button.gameObject.GetComponent<TrapBehavior>());
                });
            }
        }

        void OnEnable()
        {
            currentWallNumber = initDefense.currentMapStats.wallNumber;
            currentWallType = initDefense.currentMapStats.wallType;
            wallButtonText.text = "Mur x" + currentWallNumber;
            
            DrawCards();
        }

        public void PutWallInHand()
        {
            if (currentWallNumber > 0 && !hoverDetector.objectInHand)
            {
                currentWallNumber -= 1;
                wallButtonText.text = "Mur x" + currentWallNumber;
                GameObject wall = defensePooler.GetPooledObject(currentWallType+1);
                hoverDetector.objectInHand = wall;
                wall.SetActive(true);
            }
        }

        public void PutCardInHand(GameObject card)
        {
            card.transform.parent = null;
            hoverDetector.objectInHand = card;
            card.layer = LayerMask.NameToLayer("CardInHand");
        }
        
        public void PutCardBackToHand(GameObject equipementCard)
        {
            if (equipementCard)
            {
                CardBehavior equipementCardBehavior = equipementCard.GetComponent<CardBehavior>();
                equipementCardBehavior.groupParent.SetActive(false);
                equipementCardBehavior.groupParent.transform.localPosition = Vector3.zero;
                equipementCardBehavior.ownMeshRenderer.enabled = true;
                equipementCardBehavior.transform.SetParent(equipementCardBehavior.ownCardContainer);
                equipementCardBehavior.transform.localPosition = Vector3.zero;
                equipementCardBehavior.gameObject.layer = LayerMask.NameToLayer("Card");
                equipementCard.SetActive(true);
            }
        }

        public void PutTrapInHand(TrapBehavior trapBehavior)
        {
            if (!hoverDetector.objectInHand)
            {
                GameObject trap = defensePooler.GetPooledObject(0);
                trap.GetComponent<TrapBehavior>().CopyBehavior(trapBehavior);
                hoverDetector.objectInHand = trap;
                trap.SetActive(true);
            }
        }
        
        public void DrawCards()
        {
            foreach (var cardContainer in mobCardContainers)
            {
                if (cardContainer.transform.childCount == 0)
                {
                    InitCard(cardContainer, 0);
                }
            }
            foreach (var cardContainer in equipementCardContainers)
            {
                if (cardContainer.transform.childCount == 0)
                {
                    InitCard(cardContainer, 1);
                }
            }
        }

        private void InitCard(GameObject cardContainer, int type)
        {
            GameObject card = Instantiate(cardPrefab, cardContainer.transform, true);
            card.transform.localPosition = new Vector3(0,0,0);
            card.transform.localEulerAngles = new Vector3(0,0,0);
            card.GetComponent<CardBehavior>().SetCard(type);
        }
        
    }
}
