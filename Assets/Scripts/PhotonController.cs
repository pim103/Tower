using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public enum TypeLobby
    {
        RANKED,
        PRIVATE,
        BYPASS_DEFENSE
    }

    public class PhotonController : MonoBehaviourPunCallbacks
    {
        public bool wantToCreateWhenFailedJoin = false;
        public bool wantToJoinRoomAfterLobby = false;
        public bool wantToJoinLobbyAfterConnection = false;

        public TypeLobby typeLobbySelected = TypeLobby.RANKED;

        public bool isConnected = false;
        public bool isInLobby = false;
        public bool isInGame = false;

        // ======================================= INIT BASIC VARIABLES ========================================

        public void setupPhotonController(TypeLobby typeLobby)
        {
            switch (typeLobby)
            {
                case TypeLobby.PRIVATE:
                    wantToCreateWhenFailedJoin = false;
                    wantToJoinRoomAfterLobby = false;
                    wantToJoinLobbyAfterConnection = false;
                    typeLobbySelected = TypeLobby.PRIVATE;
                    break;
                case TypeLobby.RANKED:
                    wantToCreateWhenFailedJoin = true;
                    wantToJoinRoomAfterLobby = true;
                    wantToJoinLobbyAfterConnection = false;
                    typeLobbySelected = TypeLobby.RANKED;
                    break;
                case TypeLobby.BYPASS_DEFENSE:
                    wantToCreateWhenFailedJoin = true;
                    wantToJoinRoomAfterLobby = true;
                    wantToJoinLobbyAfterConnection = true;
                    typeLobbySelected = TypeLobby.RANKED;
                    break;
                default:
                    wantToCreateWhenFailedJoin = false;
                    wantToJoinRoomAfterLobby = false;
                    wantToJoinLobbyAfterConnection = false;
                    typeLobbySelected = TypeLobby.RANKED;
                    break;
            }
        }

        // ======================================= PHOTON COMPLEMENT ========================================

        public void ConnectToPhoton()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        private TypedLobby getTypedLobby()
        {
            TypedLobby tl;

            switch (typeLobbySelected)
            {
                case TypeLobby.PRIVATE:
                    tl = new TypedLobby
                    {
                        Name = "Private"
                    };
                    break;
                case TypeLobby.RANKED:
                    tl = new TypedLobby
                    {
                        Name = "Ranked"
                    };
                    break;
                default:
                    tl = new TypedLobby();
                    break;
            }

            return tl;
        }

        public void JoinLobby()
        {
            TypedLobby tl = getTypedLobby();
            PhotonNetwork.JoinLobby(tl);
        }

        public void CreateRoom()
        {
            RoomOptions ro = new RoomOptions
            {
                MaxPlayers = 2,
                PublishUserId = true,
                IsOpen = true,
                IsVisible = true,
                EmptyRoomTtl = 1000
            };

            TypedLobby tl = getTypedLobby();

            PhotonNetwork.CreateRoom("Ranked", ro, tl);
        }

        // ======================================= PHOTON ========================================

        public override void OnConnectedToMaster()
        {
            isConnected = true;

            if (wantToJoinLobbyAfterConnection)
            {
                JoinLobby();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnected = false;
        }

        public override void OnJoinedLobby()
        {
            isInLobby = true;

            TypedLobby tl = getTypedLobby();

            if (wantToJoinRoomAfterLobby)
            {
                PhotonNetwork.JoinRandomRoom(null, byte.MinValue, MatchmakingMode.RandomMatching, tl, null);
            }
        }

        public override void OnLeftLobby()
        {
            isInLobby = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            if(wantToCreateWhenFailedJoin)
            {
                CreateRoom();
            }
        }

        public override void OnJoinedRoom()
        {
            isInGame = true;
        }

        public override void OnLeftRoom()
        {
            isInGame = false;
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(returnCode + " : " + message);
        }
    }
}
