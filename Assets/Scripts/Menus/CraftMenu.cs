using System.Collections;
using System.Collections.Generic;
using Menus;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
{
    public class CraftMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private Button returnButton;

        private void Start()
        {
            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.MainMenu);
            });
        }

        public void InitMenu()
        {
            Debug.Log("Craft Menu");
        }
    }
}