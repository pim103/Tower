using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
{
    public class PlayMenu : MonoBehaviourPunCallbacks, MenuInterface
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
            TypedLobby tl = new TypedLobby
            {
                Name = "Ranked"
            };

            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.JoinLobby(tl);
        }

        /* ============================== PHOTON ============================== */

        public override void OnJoinedLobby()
        {
            TypedLobby tl = new TypedLobby
            {
                Name = "Ranked"
            };

            PhotonNetwork.JoinRandomRoom(null, byte.MinValue, MatchmakingMode.RandomMatching, tl, null);
        }

        public override void OnJoinedRoom()
        {
            mc.ActivateMenu(MenuController.Menu.ListingPlayer);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Fail join room :  "+ message + " Creating...");

            RoomOptions ro = new RoomOptions
            {
                MaxPlayers = 2,
                PublishUserId = true,
                IsOpen = true,
                IsVisible = true,
                EmptyRoomTtl = 1000
            };

            TypedLobby tl = new TypedLobby
            {
                Name = "Ranked"
            };

            PhotonNetwork.CreateRoom("Ranked", ro, tl);
        }

        /* ============================== INTERFACE ============================== */

        public void InitMenu()
        {
        }
    }
}