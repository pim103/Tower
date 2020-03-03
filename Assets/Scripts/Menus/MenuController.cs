using UnityEngine;

namespace Menus
{   
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] menus;

        public enum Menu
        {
            Connection,
            Registration,
            MainMenu,
            Play,
            PrivateMatch,
            CreateRoom,
            ListingPlayer,
            Craft,
            DeckManagement,
            Collection,
            CreateDeck,
            Settings,
            Shop
        };

        private void Start()
        {
            ActivateMenu(Menu.Connection);
        }

        public void ActivateMenu(Menu menuIndex)
        {
            int index = (int)menuIndex;

            DesactiveMenus();
            MenuInterface mi = menus[index].GetComponent<MenuInterface>();

            menus[index].SetActive(true);
            mi.InitMenu();
        }

        private void DesactiveMenus()
        {
            foreach (GameObject menu in menus)
            {
                menu.SetActive(false);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}