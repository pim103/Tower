using System;
using System.Collections.Generic;
using System.IO;
using DeckBuilding;
using Games.Global;
using Games.Global.Weapons;
using Games.Players;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace Games.Transitions
{
    public class ChooseDeckAndClass : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        
        [SerializeField] private Button[] buttonsOfChoice;

        [SerializeField] private Button validateChoices;

        [SerializeField] private Button[] monsterDeckButtons;
        [SerializeField] private Button[] equipmentDeckButtons;

        public static Identity currentRoleIdentity;
        public static Identity currentWeaponIdentity;

        public static int monsterDeckId; 
        public static int equipmentDeckId; 

        private Image currentRoleImage;
        private Image currentWeaponImage;

        private Dictionary<Classes, List<CategoryWeapon>> avalaibleWeaponForClass;
        private Dictionary<Button, Identity> IdentityOfButton;

        private readonly float[] greyColor = {0.3962264f, 0.3962264f, 0.3962264f};
        private readonly float[] greenColor = {0.1081821f, 0.5566038f, 0f};

        public static bool isValidate;
        private List<Deck> playerDecks;

        private int activeMonsterButtonCount = 1;
        private int activeEquipmentButtonCount = 1;

        private void OnEnable()
        {
            isValidate = false;
        }

        private void Start()
        {
            isValidate = false;
            SetAvailableWeaponForClass();
            IdentityOfButton = new Dictionary<Button, Identity>();

            Button activeButton = null;

            foreach (Button button in buttonsOfChoice)
            {
                Identity identity = button.GetComponent<Identity>();
                button.onClick.AddListener(delegate { GetIdentityOfButton(button); });

                IdentityOfButton.Add(button, identity);
                if (identity.identityType == IdentityType.Role && activeButton == null)
                {
                    activeButton = button;
                }
                else if (identity.identityType == IdentityType.CategoryWeapon)
                {
                    button.interactable = false;
                }
            }

            if (activeButton != null)
            {
                activeButton.onClick.Invoke();
            }

            validateChoices.onClick.AddListener(LaunchGame);
            monsterDeckId = 0;
            equipmentDeckId = 0;
            FetchDecks();
        }

        private void LaunchGame()
        {
            Dictionary<string, int> argsDict = new Dictionary<string, int>();
            argsDict.Add("class", (int)currentRoleIdentity.classe);
            argsDict.Add("weapon", (int)currentWeaponIdentity.categoryWeapon);

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
                currentWeaponIdentity = null;
                currentWeaponImage = null;

                ActiveButtonForSpecificRole(identity.classe);
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

        private void ActiveButtonForSpecificRole(Classes classe)
        {
            List<CategoryWeapon> weaponForClass = avalaibleWeaponForClass[classe];
            Button weaponToChoose = null;

            foreach (KeyValuePair<Button, Identity> pair in IdentityOfButton)
            {
                if (pair.Value.identityType == IdentityType.CategoryWeapon)
                {
                    if (weaponForClass.Contains(pair.Value.categoryWeapon))
                    {
                        if (weaponToChoose == null)
                        {
                            weaponToChoose = pair.Key;
                        }

                        pair.Key.interactable = true;
                        pair.Key.gameObject.GetComponent<Image>().color =
                            new Color(greyColor[0], greyColor[1], greyColor[2]);
                    }
                    else
                    {
                        pair.Key.interactable = false;
                        pair.Key.gameObject.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f);
                    }
                }
            }

            if (weaponToChoose != null)
            {
                weaponToChoose.onClick.Invoke();
            }
        }

        private void SetAvailableWeaponForClass()
        {
            avalaibleWeaponForClass = new Dictionary<Classes, List<CategoryWeapon>>();

            List<CategoryWeapon> categoryWeaponsWarrior = new List<CategoryWeapon>();
            categoryWeaponsWarrior.Add(CategoryWeapon.SHORT_SWORD);
            categoryWeaponsWarrior.Add(CategoryWeapon.BOW);
//            categoryWeaponsWarrior.Add(CategoryWeapon.SPEAR);
            categoryWeaponsWarrior.Add(CategoryWeapon.STAFF);
            categoryWeaponsWarrior.Add(CategoryWeapon.DAGGER);

            avalaibleWeaponForClass.Add(Classes.Warrior, categoryWeaponsWarrior);

            List<CategoryWeapon> categoryWeaponsRanger = new List<CategoryWeapon>();

            categoryWeaponsRanger.Add(CategoryWeapon.SHORT_SWORD);
            categoryWeaponsRanger.Add(CategoryWeapon.BOW);
//            categoryWeaponsRanger.Add(CategoryWeapon.SPEAR);
            categoryWeaponsRanger.Add(CategoryWeapon.STAFF);
            categoryWeaponsRanger.Add(CategoryWeapon.DAGGER);

            avalaibleWeaponForClass.Add(Classes.Ranger, categoryWeaponsRanger);

            List<CategoryWeapon> categoryWeaponsMage = new List<CategoryWeapon>();

            categoryWeaponsMage.Add(CategoryWeapon.SHORT_SWORD);
            categoryWeaponsMage.Add(CategoryWeapon.BOW);
//            categoryWeaponsMage.Add(CategoryWeapon.SPEAR);
//            categoryWeaponsMage.Add(CategoryWeapon.STAFF);
            categoryWeaponsMage.Add(CategoryWeapon.DAGGER);

            avalaibleWeaponForClass.Add(Classes.Mage, categoryWeaponsMage);

            List<CategoryWeapon> categoryWeaponsRogue = new List<CategoryWeapon>();

            categoryWeaponsRogue.Add(CategoryWeapon.SHORT_SWORD);
            categoryWeaponsRogue.Add(CategoryWeapon.BOW);
//            categoryWeaponsRogue.Add(CategoryWeapon.SPEAR);
//            categoryWeaponsRogue.Add(CategoryWeapon.STAFF);
            categoryWeaponsRogue.Add(CategoryWeapon.DAGGER);

            avalaibleWeaponForClass.Add(Classes.Rogue, categoryWeaponsRogue);
        }
        
        private void FetchDecks()
        {
            playerDecks = DataObject.CardList.GetDecks();

            Debug.Log(playerDecks.Count);
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
                        Debug.Log("default monster deck = " + deck.id);
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
                        Debug.Log("default weapon deck = " + deck.id);
                        equipmentDeckId = deck.id;
                    }
                }
            }
        }
    }
}