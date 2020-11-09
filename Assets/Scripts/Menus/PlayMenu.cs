using System.Collections;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Menus {
public class PlayMenu : MonoBehaviour, MenuInterface {
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

  private Rooms roomList;

  private void Start() {
    multiButton.onClick.AddListener(SearchRanked);

    privateButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.PrivateMatch); });

    returnButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.MainMenu); });
  }

  private void CreateMatch() { StartCoroutine(CreateMatchRequest()); }

  private void SearchMatch() { StartCoroutine(LoadRoomRequest()); }

  private void SearchRanked() {
    mc.SearchMatch();
    mc.ActivateMenu(MenuController.Menu.MainMenu);
  }

  IEnumerator CreateMatchRequest() {
    WWWForm form = new WWWForm();
    form.AddField("name", "RANKED_" + UniqueKey.KeyGenerator.GetUniqueKey(50));
    form.AddField("roomOwner", NetworkingController.AuthToken);
    form.AddField("maxPlayer", 2);
    form.AddField("mode", "1v1");
    form.AddField("isRanked", 1);
    form.AddField("isPublic", 1);
    var www = UnityWebRequest.Post(
        NetworkingController.PublicURL + "/services/room/add.php", form);
    www.certificateHandler = new AcceptCertificate();
    yield return www.SendWebRequest();
    yield return new WaitForSeconds(0.5f);
    if (www.responseCode == 201) {
      yield return new WaitForSeconds(0.5f);
      NetworkingController.CurrentRoomToken = www.downloadHandler.text;
      yield return new WaitForSeconds(0.5f);
      mc.ActivateMenu(MenuController.Menu.ListingPlayer);
    } else if (www.responseCode == 406) {
      Debug.Log("Impossible de créer la room");
    } else {
      Debug.Log(www.responseCode);
      Debug.Log(www.downloadHandler.text);
      Debug.Log("Serveur d'authentification indisponible.");
    }
  }

  IEnumerator LoadRoomRequest() {
    var www = UnityWebRequest.Get(NetworkingController.PublicURL +
                                  "/services/room/list.php");
    www.certificateHandler = new AcceptCertificate();
    yield return www.SendWebRequest();
    yield return new WaitForSeconds(0.5f);
    if (www.responseCode == 200) {
      Debug.Log(www.downloadHandler.text);
      if (www.downloadHandler.text == "{\"rooms\":[]}") {
        Debug.Log("Create Room");
        CreateMatch();
      } else {
        Debug.Log("Joining room");
        roomList = JsonUtility.FromJson<Rooms>(www.downloadHandler.text);
        NetworkingController.CurrentRoomToken = roomList.rooms[0].name;
        mc.ActivateMenu(MenuController.Menu.ListingPlayer);
      }
    } else {
      Debug.Log(www.responseCode);
      Debug.Log(www.downloadHandler.text);
      Debug.Log("Serveur d'authentification indisponible.");
    }
  }

  /* ============================== INTERFACE ============================== */

  public void InitMenu() {}
}
}