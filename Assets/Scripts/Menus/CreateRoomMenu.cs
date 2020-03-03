using Scripts.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class CreateRoomMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private InputField roomNameField;

        [SerializeField]
        private InputField passwordField;

        [SerializeField]
        private Toggle isPrivate;

        [SerializeField]
        private Text nbPlayersText;

        [SerializeField]
        private Slider nbPlayersSlider;

        [SerializeField]
        private Button createButton;

        [SerializeField]
        private Button returnButton;

        private void Start()
        {
            createButton.onClick.AddListener(CreateRoomAction);

            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.PrivateMatch);
            });

            nbPlayersSlider.onValueChanged.AddListener(ChangeTextListener);
        }

        private void ChangeTextListener(float value)
        {
            nbPlayersText.text = "Nombres de joueurs : " + value.ToString();
        }

        private void CreateRoomAction()
        {
            if(roomNameField.text == "")
            {
                return;
            }

            mc.ActivateMenu(MenuController.Menu.ListingPlayer);
        }

        public void InitMenu()
        {
            
        }
    }
}