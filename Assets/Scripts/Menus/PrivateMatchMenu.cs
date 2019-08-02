using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
{
    public class PrivateMatchMenu : MonoBehaviourPunCallbacks, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private Button createRoomButton;

        [SerializeField]
        private Button joinRoomButton;

        [SerializeField]
        private Button returnButton;

        [SerializeField]
        private RectTransform content;

        [SerializeField]
        private GameObject roomCase;

        private Dictionary<string, GameObject> listRoom;

        private string roomSelected;

        // Start is called before the first frame update
        private void Start()
        {
            createRoomButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.CreateRoom);
            });

            joinRoomButton.onClick.AddListener(JoinRoom);

            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.Play);
            });
        }

        private void ClearRoom()
        {
            foreach(var room in listRoom.Values)
            {
                Destroy(room);
            }

            listRoom.Clear();
        }

        private void SelectRoom(string roomName)
        {
            if(roomSelected != null)
            {
                listRoom[roomSelected].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }

            roomSelected = roomName;
            listRoom[roomName].transform.GetChild(0).GetComponent<Image>().color = Color.green;
            joinRoomButton.interactable = true;
        }

        private void JoinRoom()
        {
            if(roomSelected != null)
            {
                PhotonNetwork.JoinRoom(roomSelected);
            }
        }

        private IEnumerator CheckConnectionToLobby()
        {
            while(true)
            {
                yield return new WaitForSeconds(1.0f);

                if(!PhotonNetwork.IsConnected)
                {
                    mc.ConnectToPhoton();
                }
                else if(!PhotonNetwork.InLobby)
                {
                    TypedLobby tl = new TypedLobby
                    {
                        Name = "private"
                    };

                    PhotonNetwork.JoinLobby(tl);
                }
            }
        }

        /* ============================== PHOTON ============================== */

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Refresh Room List");
            foreach(var room in roomList)
            {
                if(room.RemovedFromList && listRoom.ContainsKey(room.Name))
                {
                    Destroy(listRoom[room.Name]);
                    listRoom.Remove(room.Name);
                }
                else if(!listRoom.ContainsKey(room.Name))
                {
                    var newRoomCase = Instantiate(roomCase, content);
                    newRoomCase.transform.GetChild(1).GetComponent<Text>().text = room.Name + " : " + room.PlayerCount + " / " + room.MaxPlayers;

                    newRoomCase.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        SelectRoom(room.Name);
                    });

                    listRoom.Add(room.Name, newRoomCase);
                }
                else
                {
                    listRoom[room.Name].transform.GetChild(1).GetComponent<Text>().text = room.Name + " : " + room.PlayerCount + " / " + room.MaxPlayers;
                }
            }
        }

        public override void OnJoinedRoom()
        {
            mc.ActivateMenu(MenuController.Menu.ListingPlayer);
        }

        /* ============================== INTERFACE ============================== */

        public void InitMenu()
        {
            if (listRoom == null)
            {
                listRoom = new Dictionary<string, GameObject>();
            }
            else
            {
                ClearRoom();
            }

            TypedLobby tl = new TypedLobby
            {
                Name = "private"
            };

            PhotonNetwork.JoinLobby(tl);

            joinRoomButton.interactable = false;

            StartCoroutine(CheckConnectionToLobby());
        }
    }
}