using System.Collections;
using System.Collections.Generic;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Menus {
public class PrivateMatchMenu : MonoBehaviour, MenuInterface {
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

  private Dictionary<string, GameObject>listRoom;
  private Rooms roomList;

  private string roomSelected;

  // Start is called before the first frame update
  private void Start() {
    createRoomButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.CreateRoom); });

    joinRoomButton.onClick.AddListener(JoinRoom);

    returnButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.Play); });
    StartCoroutine(LoadJsonRoom());
  }

  private void ClearRoom() {
    foreach(var room in listRoom.Values) { Destroy(room); }

    listRoom.Clear();
  }

  private void SelectRoom(string roomName) {
    if (roomSelected != null) {
      listRoom[roomSelected].transform.GetChild(0).GetComponent<Image>().color =
          Color.white;
    }

    roomSelected = roomName;
    listRoom[roomName].transform.GetChild(0).GetComponent<Image>().color =
        Color.green;
    joinRoomButton.interactable = true;
  }

  private void JoinRoom() { Debug.Log(roomSelected); }

  /* ============================== INTERFACE ============================== */

  public void InitMenu() {
    if (listRoom == null) {
      listRoom = new Dictionary<string, GameObject>();
    } else {
      ClearRoom();
    }

    joinRoomButton.interactable = false;
  }

  IEnumerator LoadJsonRoom() {
    var www =
        UnityWebRequest.Get("https://towers.heolia.eu/services/room/list.php");
    www.certificateHandler = new AcceptCertificate();
    yield return www.SendWebRequest();
    yield return new WaitForSeconds(0.5f);
    if (www.responseCode == 200) {
      Debug.Log(www.downloadHandler.text);
      roomList = JsonUtility.FromJson<Rooms>(www.downloadHandler.text);
      foreach(Room room in roomList.rooms) {
        GameObject newRoom = Instantiate(roomCase, content, false);
        RoomListing roomListing = newRoom.GetComponent<RoomListing>();
        roomListing.RoomInfo = room;
        roomListing.SetRoomInfo(room);
        Debug.Log(roomListing.RoomInfo.mode);
      }
    }
  }
}
}
