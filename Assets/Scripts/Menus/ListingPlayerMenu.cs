using System.Collections;
using System.Collections.Generic;
using Games.Transitions;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.UI;

namespace Menus {
public class ListingPlayerMenu : MonoBehaviour, MenuInterface {
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

  private string canStart = null;

  private Dictionary<string, GameObject>listPlayerCase;
  private Dictionary<string, bool>listPlayerIsReady;

  private IEnumerator WaitingForCanStart() {
    while (canStart == null) {
      yield return new WaitForSeconds(1f);
    }
    transitionMenuGame.InitGame();
  }

  private void Start() {
    returnButton.onClick.AddListener(ReturnAction);
    readyButton.onClick.AddListener(SetReadyAction);

    var setSocket = new Dictionary<string, string>();
    setSocket.Add("tokenPlayer", NetworkingController.AuthToken);
    setSocket.Add("room", NetworkingController.CurrentRoomToken);

    TowersWebSocket.TowerSender("ALL", "GENERAL", "null", "joinRoom",
                                TowersWebSocket.FromDictToString(setSocket));
    TowersWebSocket.wsGame.OnMessage += (sender, args) => {
      if (args.Data == "{\"CanStartHandler\":[{\"message\":\"true\"}]}") {
        Debug.Log("Done!");
        canStart = args.Data;
      }
    };
    StartCoroutine(WaitingForCanStart());
  }

  private void ReturnAction() {
    mc.ActivateMenu(MenuController.Menu.PrivateMatch);
  }

  private void SetReadyAction() {}

  /* ============================== INTERFACE ============================== */

  public void InitMenu() {}
}
}