using System.Collections.Generic;
using Menus;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Games.Transitions;

namespace Scripts.Menus
{
    public class ListingPlayerMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private Button readyButton;

        [SerializeField]
        private Button returnButton;

        [SerializeField]
        private GameObject playerCase;

        [SerializeField]
        private RectTransform contentListPlayer;

        [SerializeField]
        private TransitionMenuGame transitionMenuGame;

        private Dictionary<string, GameObject> listPlayerCase;
        private Dictionary<string, bool> listPlayerIsReady;

        private void Start()
        {
            returnButton.onClick.AddListener(ReturnAction);

            readyButton.onClick.AddListener(SetReadyAction);
        }

        private void ReturnAction()
        {
            mc.ActivateMenu(MenuController.Menu.PrivateMatch);
        }

        private void SetReadyAction()
        {
            transitionMenuGame.InitGame();
        }

        /* ============================== INTERFACE ============================== */

        public void InitMenu()
        {
        }
    }
}