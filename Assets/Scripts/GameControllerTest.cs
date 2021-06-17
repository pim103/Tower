using System;
using System.Collections;
using System.Collections.Generic;
using DeckBuilding;
using Games;
using Games.Defenses;
using Games.Global;
using Games.Transitions;
using Menus;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameControllerTest : MonoBehaviour
    {
        [SerializeField] private Button sendGrid;
        [SerializeField] private GameGridController gameGridController;
        [SerializeField] private NavMeshSurface nms;

        private static InitDefense initDefense;

        private void Start()
        {
            GameControllerNetwork.InitGameControllerNetwork();
            
            sendGrid.onClick.AddListener(delegate
            {
                StartCoroutine(EndDef());
            });
        }

        public IEnumerator EndDef()
        {
            initDefense.FillGameGrid();
            GameController.currentGameGrid.DisplayGridData();
            GameControllerNetwork.SendGridData(initDefense.defenseGrid);

            gameGridController.DesactiveMap();
            
            yield return new WaitForSeconds(4);
            gameGridController.InitGridData(GameController.currentGameGrid);
        }

        public static IEnumerator CreateDefenseInstance(InitDefense initDefensePhase)
        {
            initDefense = initDefensePhase;
            NetworkingController.Environnement = "LOCAL";

            ChooseDeckAndClass.monsterDeckId = 1;
            ChooseDeckAndClass.equipmentDeckId = 2;
            
            WWWForm form = new WWWForm();
            form.AddField("accountEmail", "pim@pim.pim");
            form.AddField("accountPassword", "pimpimpim");
            form.AddField("gameToken", NetworkingController.GameToken);
            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/account/logging", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);

            if (www.responseCode == 202)
            {
                string[] httpResponse = www.downloadHandler.text.Split('#');
                NetworkingController.NickName = httpResponse[0];
                NetworkingController.AuthToken = httpResponse[1];
                NetworkingController.AuthRole = httpResponse[2];
                yield return new WaitForSeconds(0.5f);
                TowersWebSocket.InitializeWebsocketEndpoint();
                TowersWebSocket.StartConnection();

                yield return new WaitForSeconds(0.2f);

                Debug.Log("IsConnected");
                DictionaryManager.wasConnected = true;

                while (DataObject.CardList.decks == null || DataObject.CardList.decks.Count == 0)
                {
                    yield return new WaitForSeconds(1);
                }

                SetFirstValidDeck();
                
                initDefensePhase.Init();
            }
        }

        public static void SetFirstValidDeck()
        {
            List<Deck> playerDecks = DataObject.CardList.GetDecks();

            foreach (Deck deck in playerDecks)
            {
                if (ChooseDeckAndClass.monsterDeckId != 0 && ChooseDeckAndClass.equipmentDeckId != 0)
                {
                    break;
                }

                if (deck.type == Decktype.Monsters)
                {
                    ChooseDeckAndClass.monsterDeckId = deck.id;
                }

                if (deck.type == Decktype.Equipments)
                {
                    ChooseDeckAndClass.equipmentDeckId = deck.id;
                }
            }
        }
    }
}