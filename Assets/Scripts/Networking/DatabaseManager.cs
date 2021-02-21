using System;
using System.Collections;
using DeckBuilding;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using Games.Players;
using Networking.Client;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public class DatabaseManager
    {
        public static IEnumerator GetWeapons()
        {
            var www = UnityWebRequest.Get(NetworkingController.PublicURL + "/services/game/equipment/list.php");
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);

            if (www.responseCode == 200)
            {
                DataObject.EquipmentList = new EquipmentList(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Can't get weapons...");
            }
        }

        public static IEnumerator GetGroupsMonster()
        {
            var www = UnityWebRequest.Get(NetworkingController.PublicURL + "/services/game/group/list.php");
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);

            if (www.responseCode == 200)
            {
                DataObject.MonsterList = new MonsterList(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Can't get Group Monsters...");
            }

            www = UnityWebRequest.Get(NetworkingController.PublicURL + "/services/game/monster/list.php");
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 200)
            {
                DataObject.MonsterList.InitSpecificMonsterList(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Can't get Monsters...");
            }
        }

        public static IEnumerator GetClasses() {
            var www = UnityWebRequest.Get(NetworkingController.PublicURL + "/services/game/classes/list.php");
            www.certificateHandler = new AcceptCertificate();

            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);

            if (www.responseCode == 200)
            {
                DataObject.ClassesList = new ClassesList(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Can't get Cards decks...");
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
            }
        }

        public static IEnumerator GetCards()
        {
            var www = UnityWebRequest.Get(NetworkingController.PublicURL + "/services/game/card/list.php");
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 200)
            {
                DataObject.CardList.InitCards(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Can't get Cards...");
            }
        }

        public static IEnumerator GetCardCollection()
        {
            CardList.collectionIsLoaded = false;
            
            WWWForm form = new WWWForm();
            if (NetworkingController.AuthToken != "")
            {
                form.AddField("collectionOwner", NetworkingController.AuthToken);
            }
            else
            {
                form.AddField("collectionOwner", "HZ0PUiJjDly8EDkyYUiP");
            }
            
            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/services/game/card/listAccountCollection.php", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 200)
            {
                DataObject.CardList.InitCardCollection(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Can't get Cards collection...");
            }
        }
        
        public static IEnumerator GetDecks()
        {
            WWWForm form = new WWWForm();
            if (NetworkingController.AuthToken != "")
            {
                form.AddField("deckOwner", NetworkingController.AuthToken);
            }
            else
            {
                form.AddField("deckOwner", "HZ0PUiJjDly8EDkyYUiP");
            }
            
            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/services/game/card/listCardDeck.php", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 200)
            {
                DataObject.CardList.InitDeck(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Can't get Cards decks...");
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
            }
        }

        public static IEnumerator SendData(UnityWebRequest www)
        {
            yield return SendData(www, null);
        }

        public static IEnumerator SendData(UnityWebRequest www, Action successEndCallback)
        {
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            
            if (www.responseCode == 201)
            {
                successEndCallback();
                Debug.Log("Request was send");
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("ERROR");
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}