using System;
using System.Collections;
using Games.Transitions;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Games {
public class GameController : MonoBehaviour {
  [SerializeField]
  private ObjectsInScene objectsInScene;

  [SerializeField]
  private TransitionMenuGame transitionMenuGame;

  [SerializeField]
  private ScriptsExposer se;
  [SerializeField]
  private string endPoint;
  [SerializeField]
  private string roomId;

  public static string staticRoomId;

  private string canStart = null;

  public static int PlayerIndex;

  private bool idAssigned = false;

  private bool otherPlayerDie = false;
  private CallbackMessages callbackHandlers;

  /*
   * Flag to skip defensePhase
   */
  public bool byPassDefense = true;

  public static string mapReceived;

  private IEnumerator CheckEndInit() {
    yield return new WaitForSeconds(0.1f);
    transitionMenuGame.WantToStartGame();
  }

  private IEnumerator WaitingDeathOtherPlayer() {
    while (!otherPlayerDie) {
      yield return new WaitForSeconds(1f);
    }

    Cursor.lockState = CursorLockMode.None;
    SceneManager.LoadScene("MenuScene");
  }

  // ================================== BASIC METHODS
  // ======================================

  // Start is called before the first frame update
  void Start() {
    staticRoomId = roomId;

    objectsInScene.mainCamera.SetActive(true);

    // TODO : change index
    PlayerIndex = 0;
    if (TowersWebSocket.wsGame != null) {
      TowersWebSocket.wsGame.OnMessage += (sender, args) => {
        if (args.Data.Contains("callbackMessages")) {
          Debug.Log("callbackMessages_Debug");
          callbackHandlers = JsonUtility.FromJson<CallbackMessages>(args.Data);
          /*foreach (CallbackMessage callback in
          callbackHandlers.callbackMessages)
          {
              if (callback.Identity != null)
              {
                  Debug.Log(callback.Identity);
              }
          }*/
        }
        if (args.Data.Contains("GRID")) {
          mapReceived = args.Data;
        }

        if (args.Data.Contains("DEATH")) {
          Debug.Log("Vous avez gagné");
          otherPlayerDie = true;
        }

        if (args.Data.Contains("WON")) {
          Debug.Log("Un autre joueur a gagné");
          otherPlayerDie = true;
        }
      };
    }

    if (byPassDefense) {
      canStart = "{\"CanStartHandler\":[{\"message\":\"true\"}]}";
    }

    StartCoroutine(WaitingDeathOtherPlayer());
    StartCoroutine(CheckEndInit());
  }
  // TODO : Control player's movement here and not in PlayerMovement
}
}
