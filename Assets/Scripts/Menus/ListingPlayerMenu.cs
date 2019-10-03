using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Scripts.Games.Transitions;

namespace Scripts.Menus
{
    public class ListingPlayerMenu : MonoBehaviourPunCallbacks, MenuInterface
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

        private void InitPlayerCase()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (listPlayerCase.ContainsKey(player.UserId))
                {
                    continue;
                }

                GameObject newPlayerCase = Instantiate(playerCase, contentListPlayer);
                newPlayerCase.transform.GetChild(1).GetComponent<Text>().text = player.UserId;

                listPlayerCase.Add(player.UserId, newPlayerCase);
                listPlayerIsReady.Add(player.UserId, false);
            }
        }

        private void ReturnAction()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void ClearPlayer()
        {
            foreach (var keys in listPlayerCase.Keys)
            {
                Destroy(listPlayerCase[keys]);
            }

            listPlayerCase.Clear();
            listPlayerIsReady.Clear();
        }

        private void SetReadyAction()
        {
            bool gameCanStart = true;

            if(!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SetReadyRPC", RpcTarget.MasterClient);
            }
            else
            {
                foreach(var player in PhotonNetwork.PlayerList)
                {
                    if(player.UserId != PhotonNetwork.AuthValues.UserId && listPlayerIsReady.ContainsKey(player.UserId) && !listPlayerIsReady[player.UserId])
                    {
                        gameCanStart = false;
                    }
                }

                if(gameCanStart)
                {
                    transitionMenuGame.InitGame();
                    Debug.Log("Launch GAME");
                }
            }
        }

        [PunRPC]
        private void SetReadyRPC(PhotonMessageInfo info)
        {
            photonView.RPC("SetReadyForAllClient", RpcTarget.All, info.Sender.UserId, !listPlayerIsReady[info.Sender.UserId]);
        }

        [PunRPC]
        private void SetReadyForAllClient(string userId, bool isReady)
        {
            listPlayerIsReady[userId] = isReady;

            if (isReady)
            {
                listPlayerCase[userId].transform.GetChild(0).GetComponent<Image>().color = Color.green;
            } else
            {
                listPlayerCase[userId].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
        }

        /* ============================== PHOTON ============================== */

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if(listPlayerCase.ContainsKey(newPlayer.UserId))
            {
                return;
            }

            GameObject newPlayerCase = Instantiate(playerCase, contentListPlayer);
            newPlayerCase.transform.GetChild(1).GetComponent<Text>().text = newPlayer.UserId;

            listPlayerCase.Add(newPlayer.UserId, newPlayerCase);
            listPlayerIsReady.Add(newPlayer.UserId, false);
        }

        public override void OnLeftRoom()
        {
            mc.ActivateMenu(MenuController.Menu.PrivateMatch);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Destroy(listPlayerCase[otherPlayer.UserId]);
            listPlayerCase.Remove(otherPlayer.UserId);
        }

        /* ============================== INTERFACE ============================== */

        public void InitMenu()
        {
            if(listPlayerCase == null)
            {
                listPlayerCase = new Dictionary<string, GameObject>();
                listPlayerIsReady = new Dictionary<string, bool>();
            }
            else
            {
                ClearPlayer();
            }

            InitPlayerCase();
        }
    }
}