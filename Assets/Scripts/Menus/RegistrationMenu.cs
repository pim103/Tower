using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
{
    public class RegistrationMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private InputField loginField;

        [SerializeField]
        private InputField passwordField;

        [SerializeField]
        private InputField confirmPasswordField;

        [SerializeField]
        private InputField emailField;

        [SerializeField]
        private Button createButton;

        [SerializeField]
        private Button returnButton;

        private void Start()
        {
            createButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Connection);
            });

            returnButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Connection);
            });
        }

        public void InitMenu()
        {
            Debug.Log("Registration Menu");
            //throw new System.NotImplementedException();
        }
    }
}