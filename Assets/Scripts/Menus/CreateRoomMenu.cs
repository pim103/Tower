using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
{
    public class CreateRoomMenu : MonoBehaviourPunCallbacks, MenuInterface
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

            RoomOptions ro = new RoomOptions
            {
                MaxPlayers = (byte)nbPlayersSlider.value,
                PublishUserId = true,
                IsOpen = true,
                IsVisible = true,
                EmptyRoomTtl = 1000
            };

            TypedLobby tl = new TypedLobby
            {
                Name = "private"
            };

            PhotonNetwork.CreateRoom(roomNameField.text, ro, tl);
        }

        public override void OnJoinedRoom()
        {
            mc.ActivateMenu(MenuController.Menu.ListingPlayer);
        }

        public void InitMenu()
        {
        }
    }
}