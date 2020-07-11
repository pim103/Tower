using System;
using System.Collections;
using FullSerializer;
using Games.Defenses;
using Games.Global;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Games.Transitions {
  public class TransitionMenuGame : MonoBehaviour {
    private const int durationChooseDeckPhase = 100;

    [SerializeField]
    private ObjectsInScene objectsInScene;

    [SerializeField]
    private InitDefense initDefense;

    private static int waitingForStart;
    public static int timerAttack = Int32.MaxValue;

    private string waitingGameStartText;

    [SerializeField]
    private GameObject chooseRoleAndDeckGameObject;

    private void Start() {
      waitingForStart = durationChooseDeckPhase;
      waitingGameStartText = "Waiting for another player";
    }

    public bool InitGame() {
      CurrentRoom.loadGameDefense = false;
      CurrentRoom.loadGameAttack = false;

      StartCoroutine(LoadGame());
      TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,
                                  "null", "setGameLoaded", "null");
      if (TowersWebSocket.wsGame != null) {
        TowersWebSocket.wsGame.OnMessage += (sender, args) => {
          if (args.Data.Contains("callbackMessages")) {
            fsSerializer serializer = new fsSerializer();
            fsData data;
            CallbackMessages callbackMessage = null;
            try {
              data = fsJsonParser.Parse(args.Data);
              serializer.TryDeserialize(data, ref callbackMessage);
              callbackMessage = Tools.Clone(callbackMessage);
              if (callbackMessage.callbackMessages.message == "LoadGame") {
                CurrentRoom.loadGame = true;
              }
              if (callbackMessage.callbackMessages.message == "setGameLoaded") {
                Debug.Log("En attente de l'adversaire");
              }

              if (CurrentRoom.loadGame) {
                if (callbackMessage.callbackMessages.roleTimer != -1) {
                  waitingForStart = callbackMessage.callbackMessages.roleTimer;
                  CurrentRoom.loadRoleAndDeck = true;
                }
                if (callbackMessage.callbackMessages.message ==
                    "StartDefense") {
                  CurrentRoom.loadGameDefense = true;
                }
                if (callbackMessage.callbackMessages.message == "StartAttack") {
                  CurrentRoom.loadGameAttack = true;
                }
                if (CurrentRoom.loadGameAttack) {
                  if (callbackMessage.callbackMessages.attackTimer != -1) {
                    timerAttack = callbackMessage.callbackMessages.attackTimer;
                  }
                }
              }

            } catch (Exception e) {
              Debug.Log("Can't read callback : " + e.Message);
            }
          }
        };
      }

      return true;
    }

    public IEnumerator LoadGame() {
      while (!CurrentRoom.loadGame) {
        yield return new WaitForSeconds(0.1f);
      }
      SceneManager.LoadScene("GameScene");
    }

    public IEnumerator WaitingForStart() {
      while (!CurrentRoom.loadRoleAndDeck) {
        yield return new WaitForSeconds(0.5f);
      }

      chooseRoleAndDeckGameObject.SetActive(true);

      while (waitingForStart > 0 && !ChooseDeckAndClass.isValidate) {
        objectsInScene.waitingText.text = waitingGameStartText;
        objectsInScene.counterText.text = waitingForStart.ToString();
        yield return new WaitForSeconds(0.5f);
      }

      waitingForStart = durationChooseDeckPhase;
      TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,
                                  "null", "setDefenseReady", "null");
      while (!CurrentRoom.loadGameDefense) {
        yield return new WaitForSeconds(0.5f);
      }
      // TODO : Need RPC to launch game
      StartGameWithDefense();
    }

    public void WantToStartGame() {
      waitingGameStartText = "La partie commence dans     secondes";
      StartCoroutine(WaitingForStart());
    }

    public void StartGameWithDefense() {
      chooseRoleAndDeckGameObject.SetActive(false);
      objectsInScene.mainCamera.SetActive(false);

      objectsInScene.containerAttack.SetActive(false);
      objectsInScene.containerDefense.SetActive(true);
      initDefense.Init();
    }
  }
}
