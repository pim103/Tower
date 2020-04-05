using System.Collections;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Menus {
public class ConnectionMenu : MonoBehaviour, MenuInterface {
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

  private void Start() {
    passwordField.inputType = InputField.InputType.Password;
    createButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.Registration); });
    connectButton.onClick.AddListener(CallLogin);
    quitButton.onClick.AddListener(mc.QuitGame);
  }

  private void CallLogin() { StartCoroutine(Login()); }

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
      mc.ActivateMenu(MenuController.Menu.MainMenu);
    } else if (www.responseCode == 406) {
      Debug.Log("Erreur dans les informations de connextion");
    } else {
      Debug.Log(www.responseCode);
      Debug.Log(www.downloadHandler.text);
      Debug.Log("Serveur d'authentification indisponible.");
    }
  }

  public void InitMenu() { Debug.Log("Connection Menu"); }
}
}