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

  private Dictionary<string, GameObject> listPlayerCase;
  private Dictionary<string, bool> listPlayerIsReady;
  private CallbackMessages callbackHandlers;

  private IEnumerator WaitingForCanStart() {
    while (canStart == null) {
      yield return new WaitForSeconds(2f);
      Debug.Log("Waiting");
      TowersWebSocket.TowerSender("ONLY_ONE",
                                  NetworkingController.CurrentRoomToken, "null",
                                  "getRankedMatch", "null");
      yield return new WaitForSeconds(5f);
    }
    transitionMenuGame.InitGame();
  }

  private void Start() {
    returnButton.onClick.AddListener(ReturnAction);
    readyButton.onClick.AddListener(SetReadyAction);

    var setSocket = new Dictionary<string, string>();
    setSocket.Add("tokenPlayer", NetworkingController.AuthToken);
    setSocket.Add("room", NetworkingController.CurrentRoomToken);
    TowersWebSocket.TowerSender("SELF", "GENERAL", "null", "joinWaitingRanked",
                                TowersWebSocket.FromDictToString(setSocket));
    TowersWebSocket.wsGame.OnMessage += (sender, args) => {
      if (args.Data.Contains("callbackMessages")) {
        Debug.Log("Test");
        callbackHandlers = JsonUtility.FromJson<CallbackMessages>(args.Data);
        foreach (CallbackMessage callback in callbackHandlers
                     .callbackMessages) {
          if (callback.Message == "WaitingForRanked") {
            Debug.Log(callback.Message);
          }
          if (callback.Message == "MatchStart") {
            Debug.Log("Done!");
            canStart = args.Data;
          }
        }
      }
    };
    /*

    TowersWebSocket.wsGame.OnMessage += (sender, args) =>
    {
        if (args.Data == "{\"CanStartHandler\":[{\"message\":\"true\"}]}")
        {
            Debug.Log("Done!");
            canStart = args.Data;
        }
    };*/
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
