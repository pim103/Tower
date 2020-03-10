using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class PlayMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private Button campaignButton;

        [SerializeField]
        private Button multiButton;

        [SerializeField]
        private Button privateButton;

        [SerializeField]
        private Button returnButton;

        private void Start()
        {
            multiButton.onClick.AddListener(SearchMatch);

            privateButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.PrivateMatch);
            });

            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.MainMenu);
            });
        }

        private void SearchMatch()
        {
            mc.ActivateMenu(MenuController.Menu.ListingPlayer);
        }

        /* ============================== INTERFACE ============================== */

        public void InitMenu()
        {
        }
    }
}