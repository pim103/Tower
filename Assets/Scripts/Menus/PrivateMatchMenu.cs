using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class PrivateMatchMenu : MonoBehaviour, MenuInterface
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
            Debug.Log(roomSelected);
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

            joinRoomButton.interactable = false;
        }
    }
}