using System.Collections;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Menus
{
    public class PlayMenu : MonoBehaviour, MenuInterface
    {
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

        private void Start()
        {
            multiButton.onClick.AddListener(SearchMatch);

            privateButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.PrivateMatch);
            });

            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.MainMenu);
            });
        }

        public void CreateMatch()
        {
            StartCoroutine(Room.CreateMatchRequest(mc));
        }

        public void SearchMatch()
        {
            StartCoroutine(LoadRoomRequest());
        }
        
        IEnumerator LoadRoomRequest()
        {
            WWWForm form = new WWWForm();
            form.AddField("searchForRanked", 1);
            form.AddField("gameToken", NetworkingController.GameToken);
            var www = UnityWebRequest.Post(NetworkingController.HttpsEndpoint + "/services/room/list.php", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 200)
            {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "{\"rooms\":[]}")
                {
                    Debug.Log("Create Room");
                    CreateMatch();
                }
                else
                {
                    Debug.Log("Joining room");
                    roomList = JsonUtility.FromJson<Rooms>(www.downloadHandler.text);
                    NetworkingController.CurrentRoomToken = roomList.rooms[0].name;
                    mc.ActivateMenu(MenuController.Menu.ListingPlayer);
                }
            }
            else
            {
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
                Debug.Log("Serveur d'authentification indisponible.");
            }
        }

        /* ============================== INTERFACE ============================== */

        public void InitMenu()
        {
        }
    }
}