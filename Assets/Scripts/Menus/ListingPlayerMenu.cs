using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using FullSerializer;
using Games.Transitions;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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
    TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,
                                "null", "joinWaitingRanked",
                                TowersWebSocket.FromDictToString(setSocket));
    TowersWebSocket.wsGame.OnMessage += (sender, args) => {
      if (args.Data.Contains("callbackMessages")) {
        fsSerializer serializer = new fsSerializer();
        fsData data;
        CallbackMessages callbackMessage = null;
        try {
          data = fsJsonParser.Parse(args.Data);
          Debug.Log(data);
          serializer.TryDeserialize(data, ref callbackMessage);
          callbackMessage = Tools.Clone(callbackMessage);
          if (callbackMessage.callbackMessages.room != null) {
            NetworkingController.CurrentRoomToken =
                callbackMessage.callbackMessages.room;
            canStart = args.Data;
          }
        } catch (Exception e) {
          Debug.Log("Can't read callback : " + e.Message);
        }
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
