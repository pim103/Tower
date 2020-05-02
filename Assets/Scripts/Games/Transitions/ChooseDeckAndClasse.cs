using System;
using System.Collections.Generic;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Games.Transitions
{   
    public class ChooseDeckAndClasse: MonoBehaviour
    {
        [SerializeField] private Button[] buttonsOfChoice;

        [SerializeField] private Button validateChoices;

        public static Identity currentRoleIdentity;
        public static Identity currentWeaponIdentity;

        private Image currentRoleImage;
        private Image currentWeaponImage;

        private Dictionary<Classes, List<CategoryWeapon>> avalaibleWeaponForClass;
        private Dictionary<Button, Identity> IdentityOfButton;

        private readonly float[] greyColor = {0.3962264f, 0.3962264f, 0.3962264f};
        private readonly float[] greenColor = {0.1081821f, 0.5566038f, 0f};

        public static bool isValidate;

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
                if (identity.identityType == IdentityType.Role)
                {
                    activeButton = button;
                } else if (identity.identityType == IdentityType.CategoryWeapon)
                {
                    button.interactable = false;
                }
            }

            if (activeButton != null)
            {
                activeButton.onClick.Invoke();
            }

            validateChoices.onClick.AddListener(LaunchGame);
        }

        private void LaunchGame()
        {
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

                ActiveButtonForSpecificRole(identity.classe);
            } else if (identity.identityType == IdentityType.CategoryWeapon)
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
            foreach (KeyValuePair<Button, Identity> pair in IdentityOfButton)
            {
                if (pair.Value.identityType == IdentityType.CategoryWeapon)
                {
                    if (weaponForClass.Contains(pair.Value.categoryWeapon))
                    {
                        pair.Key.interactable = true;
                        pair.Key.gameObject.GetComponent<Image>().color = new Color(greyColor[0], greyColor[1], greyColor[2]);
                    }
                    else
                    {
                        pair.Key.interactable = false;
                        pair.Key.gameObject.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f);
                    }
                }
            }
        }

        private void SetAvailableWeaponForClass()
        {
            avalaibleWeaponForClass = new Dictionary<Classes, List<CategoryWeapon>>();

            List<CategoryWeapon> categoryWeapons = new List<CategoryWeapon>();
            categoryWeapons.Add(CategoryWeapon.SHORT_SWORD);
            categoryWeapons.Add(CategoryWeapon.BOW);
            categoryWeapons.Add(CategoryWeapon.SPEAR);
            categoryWeapons.Add(CategoryWeapon.STAFF);
            categoryWeapons.Add(CategoryWeapon.DAGGER);
            
            avalaibleWeaponForClass.Add(Classes.Warrior, categoryWeapons);
            
            categoryWeapons.Clear();
            categoryWeapons.Add(CategoryWeapon.SHORT_SWORD);
            categoryWeapons.Add(CategoryWeapon.BOW);
            categoryWeapons.Add(CategoryWeapon.SPEAR);
            categoryWeapons.Add(CategoryWeapon.STAFF);
            categoryWeapons.Add(CategoryWeapon.DAGGER);
            
            avalaibleWeaponForClass.Add(Classes.Ranger, categoryWeapons);
            
            categoryWeapons.Clear();
            categoryWeapons.Add(CategoryWeapon.SHORT_SWORD);
            categoryWeapons.Add(CategoryWeapon.BOW);
            categoryWeapons.Add(CategoryWeapon.SPEAR);
            categoryWeapons.Add(CategoryWeapon.STAFF);
            categoryWeapons.Add(CategoryWeapon.DAGGER);
            
            avalaibleWeaponForClass.Add(Classes.Mage, categoryWeapons);
            
            categoryWeapons.Clear();
            categoryWeapons.Add(CategoryWeapon.SHORT_SWORD);
            categoryWeapons.Add(CategoryWeapon.BOW);
            categoryWeapons.Add(CategoryWeapon.SPEAR);
            categoryWeapons.Add(CategoryWeapon.STAFF);
            categoryWeapons.Add(CategoryWeapon.DAGGER);
            
            avalaibleWeaponForClass.Add(Classes.Rogue, categoryWeapons);

        }
    }
}