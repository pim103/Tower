using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DeckBuilding;
using Games.Global;
using Games.Global.Weapons;
using Games.Players;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Games.Transitions
{
    public class ChooseDeckAndClass : MonoBehaviour
    {
        [SerializeField] private GameController gameController;

        [SerializeField] private GameObject characterSelectionParent;
        [SerializeField] private GameObject weaponSelectionParent;
        [SerializeField] private GameObject buttonCharTemplate;
        [SerializeField] private GameObject buttonWeaponTemplate;

        [SerializeField] private Button validateChoices;

        [SerializeField] private Button[] monsterDeckButtons;
        [SerializeField] private Button[] equipmentDeckButtons;

        public static Identity currentRoleIdentity;
        public static Identity currentWeaponIdentity;

        public static int monsterDeckId; 
        public static int equipmentDeckId; 

        private Image currentRoleImage;
        private Image currentWeaponImage;

        private Dictionary<Button, Identity> IdentityOfButton;

        private readonly float[] greyColor = {0.3962264f, 0.3962264f, 0.3962264f};
        private readonly float[] greenColor = {0.1081821f, 0.5566038f, 0f};

        public static bool isValidate;
        private List<Deck> playerDecks;

        private int activeMonsterButtonCount = 1;
        private int activeEquipmentButtonCount = 1;

        private bool isInit = false;

        private void OnEnable()
        {
            isValidate = false;
        }

        private void Start()
        {
            CurrentRoom.loadRoleAndDeck = true;
        }

        private void Update()
        {
            if (!DictionaryManager.hasWeaponsLoad ||
                !DictionaryManager.hasMonstersLoad ||
                !DictionaryManager.hasCardsLoad ||
                !DictionaryManager.hasClassesLoad)
            {
                return;
            }

            if (!isInit)
            {
                isInit = true;
                isValidate = false;
                IdentityOfButton = new Dictionary<Button, Identity>();

                InstantiateCharButtons();
                InstantiateWeaponButtons();

                validateChoices.onClick.AddListener(LaunchGame);
                monsterDeckId = 0;
                equipmentDeckId = 0;
                FetchDecks();
            }
        }

        private void InstantiateCharButtons()
        {
            foreach (Classes classes in DataObject.ClassesList.classes)
            {
                GameObject charSelector = Instantiate(buttonCharTemplate, characterSelectionParent.transform);

                Identity charIdentity = charSelector.GetComponent<Identity>();
                charIdentity.title.text = classes.name;

                Button charButton = charSelector.GetComponent<Button>();

                charIdentity.InitIdentityData(IdentityType.Role, classes.id);
                charButton.onClick.AddListener(delegate { GetIdentityOfButton(charButton); });

                IdentityOfButton.Add(charButton, charIdentity);
            }
        }

        private void InstantiateWeaponButtons()
        {
            foreach (CategoryWeapon categoryWeapon in DataObject.CategoryWeaponList.categories)
            {
                GameObject weaponSelector = Instantiate(buttonWeaponTemplate, weaponSelectionParent.transform);

                Identity weaponIdentity = weaponSelector.GetComponent<Identity>();
                weaponIdentity.title.text = categoryWeapon.name;

                Button weaponButton = weaponSelector.GetComponent<Button>();

                weaponIdentity.InitIdentityData(IdentityType.CategoryWeapon, categoryWeapon.id);
                weaponButton.onClick.AddListener(delegate { GetIdentityOfButton(weaponButton); });

                IdentityOfButton.Add(weaponButton, weaponIdentity);
            }
        }
        
        private void LaunchGame()
        {
            Dictionary<string, int> argsDict = new Dictionary<string, int>
            {
                {"class", currentRoleIdentity.GetIdentityId()}, {"weapon", currentWeaponIdentity.GetIdentityId()}
            };

            if (!gameController.byPassDefense)
            {
                TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken, "Player", "SendClassWeapon", TowersWebSocket.FromDictToString(argsDict));
            }

            isValidate = true;
        }

        private void GetIdentityOfButton(Button button)
        {
            Identity identity = IdentityOfButton[button];
            Image buttonImage = button.gameObject.GetComponent<Image>();

            if (identity.identityType == IdentityType.Role)
            {
                if (currentRoleImage != null)
                {
                    currentRoleImage.color = new Color(greyColor[0], greyColor[1], greyColor[2]);
                }

                buttonImage.color = new Color(greenColor[0], greenColor[1], greenColor[2]);

                currentRoleIdentity = identity;
                currentRoleImage = buttonImage;
            }
            else if (identity.identityType == IdentityType.CategoryWeapon)
            {
                if (currentWeaponImage != null)
                {
                    currentWeaponImage.color = new Color(greyColor[0], greyColor[1], greyColor[2]);
                }

                buttonImage.color = new Color(greenColor[0], greenColor[1], greenColor[2]);
                currentWeaponIdentity = identity;
                currentWeaponImage = buttonImage;
            }
        }

        private void FetchDecks()
        {
            playerDecks = DataObject.CardList.GetDecks();

            foreach (Deck deck in playerDecks)
            {
                if (deck.type == Decktype.Monsters)
                {
                    GameObject buttonGameObject = monsterDeckButtons[activeMonsterButtonCount].gameObject;
                    buttonGameObject.SetActive(true);
                    buttonGameObject.transform.GetChild(0).GetComponent<Text>().text = deck.name;
                    monsterDeckButtons[activeMonsterButtonCount].onClick.AddListener(delegate
                        {
                            monsterDeckId = deck.id;
                        });
                    activeMonsterButtonCount++;

                    if (monsterDeckId == 0)
                    {
                        monsterDeckId = deck.id;
                    }
                }

                if (deck.type == Decktype.Equipments)
                {
                    GameObject buttonGameObject = equipmentDeckButtons[activeEquipmentButtonCount].gameObject;
                    buttonGameObject.SetActive(true);
                    buttonGameObject.transform.GetChild(0).GetComponent<Text>().text = deck.name;
                    equipmentDeckButtons[activeEquipmentButtonCount].onClick.AddListener(delegate
                    {
                        equipmentDeckId = deck.id;
                    });
                    activeEquipmentButtonCount++;

                    if (equipmentDeckId == 0)
                    {
                        equipmentDeckId = deck.id;
                    }
                }
            }
        }
    }
}