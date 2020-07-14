using System;
using System.Collections;
using System.ComponentModel;
using FullSerializer;
using Games.Global;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;

namespace Menus {
enum ErrorConnectionType {
  DisconnectedError,
  IdentificationError,
  ServerError,
  DisconnectedByAnother
}
enum InformationConnectionType {
  SendingData,
  AuthentificationStep,
  ValidateConnection
}

public static class ConnectionMessage {
  public static readonly string[] Error = {
      "Votre connexion a été interrompue",
      "Erreur d'identifiant ou de mot de passe", "Serveur indisponible",
      "Vous vous êtes connecté sur un autre périphérique"};
  public static readonly string[] Information = {
      "Envoie des données au serveur", "Authenfication en cours",
      "Connexion au serveur"};
}

public class ConnectionMenu : MonoBehaviour, MenuInterface {
  [SerializeField]
  private GameObject accountErrorPanel;
  [SerializeField]
  private Text accountErrorText;

  [SerializeField]
  private GameObject accountInformationPanel;
  [SerializeField]
  private Text accountInformationText;

  [SerializeField]
  private MenuController mc;

  [SerializeField]
  private InputField loginField;

  [SerializeField]
  private InputField passwordField;

  [SerializeField]
  private Button connectButton;

  [SerializeField]
  private Button createButton;

  [SerializeField]
  private Button quitButton;
  public string[] httpResponse;
  private CallbackMessages callbackHandlers;

  private void Start() {
    accountInformationPanel.SetActive(false);
    if (NetworkingController.ConnectionClosed != -1) {
      accountErrorPanel.SetActive(true);
      accountErrorText.text =
          NetworkingController.ConnectionClosed == 1005
              ? ConnectionMessage
                    .Error[(int) ErrorConnectionType.DisconnectedByAnother]
              : ConnectionMessage
                    .Error[(int) ErrorConnectionType.DisconnectedError];
      NetworkingController.ConnectionClosed = -1;
    }
    passwordField.inputType = InputField.InputType.Password;
    createButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.Registration); });
    connectButton.onClick.AddListener(CallLogin);
    quitButton.onClick.AddListener(mc.QuitGame);
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Tab)) {
      if (Input.GetKey(KeyCode.LeftShift)) {
        if (EventSystem.current.currentSelectedGameObject != null) {
          Selectable selectable = EventSystem.current.currentSelectedGameObject
                                      .GetComponent<Selectable>()
                                      .FindSelectableOnUp();
          if (selectable != null)
            selectable.Select();
        }
      } else {
        if (EventSystem.current.currentSelectedGameObject != null) {
          Selectable selectable = EventSystem.current.currentSelectedGameObject
                                      .GetComponent<Selectable>()
                                      .FindSelectableOnDown();
          if (selectable != null)
            selectable.Select();
        }
      }
    }
    if (Input.GetKeyDown(KeyCode.Return)) {
      CallLogin();
    }
  }

  private void CallLogin() {
    accountInformationPanel.SetActive(true);
    accountInformationText.text =
        ConnectionMessage
            .Information[(int) InformationConnectionType.SendingData];
    StartCoroutine(Login());
  }

  IEnumerator Login() {
    WWWForm form = new WWWForm();
    form.AddField("accountEmail", loginField.text);
    form.AddField("accountPassword", passwordField.text);
    form.AddField("gameToken", NetworkingController.GameToken);
    var www = UnityWebRequest.Post(
        "https://towers.heolia.eu/services/account/logging.php", form);
    www.certificateHandler = new AcceptCertificate();
    yield return www.SendWebRequest();
    yield return new WaitForSeconds(0.5f);
    if (www.responseCode == 202) {
      httpResponse = www.downloadHandler.text.Split('#');
      NetworkingController.NickName = httpResponse[0];
      NetworkingController.AuthToken = httpResponse[1];
      NetworkingController.AuthRole = httpResponse[2];
      yield return new WaitForSeconds(0.5f);
      TowersWebSocket.InitializeWebsocketEndpoint();
      TowersWebSocket.StartConnection();
      accountInformationText.text =
          ConnectionMessage.Information[(int) InformationConnectionType
                                            .AuthentificationStep];
      yield return new WaitForSeconds(1.5f);
      if (!NetworkingController.IsConnected) {
        accountInformationPanel.SetActive(false);
        TowersWebSocket.wsGame.Close();
        accountErrorPanel.SetActive(true);
        accountErrorText.text =
            ConnectionMessage
                .Error[(int) ErrorConnectionType.IdentificationError];
        NetworkingController.ConnectionStart = false;
        NetworkingController.AuthToken = "";
        NetworkingController.CurrentRoomToken = "";
        NetworkingController.AuthRole = "";
        yield break;
      }
      accountInformationText.text =
          ConnectionMessage
              .Information[(int) InformationConnectionType.ValidateConnection];
      yield return new WaitForSeconds(0.2f);

      DictionaryManager.wasConnected = true;
      mc.ActivateMenu(MenuController.Menu.MainMenu);
    } else if (www.responseCode == 406) {
      Debug.Log("Erreur dans les informations de connection");
      if (accountErrorPanel != null) {
        accountInformationPanel.SetActive(false);
        accountErrorPanel.SetActive(true);
        accountErrorText.text =
            ConnectionMessage
                .Error[(int) ErrorConnectionType.IdentificationError];
      }
    } else {
      Debug.Log(www.responseCode);
      Debug.Log(www.downloadHandler.text);
      Debug.Log("Serveur d'authentification indisponible.");
      if (accountErrorPanel != null) {
        accountInformationPanel.SetActive(false);
        accountErrorPanel.SetActive(true);
        accountErrorText.text =
            ConnectionMessage.Error[(int) ErrorConnectionType.ServerError];
      }
    }
  }

  public void InitMenu() { Debug.Log("Connection Menu"); }
}
}