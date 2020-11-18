using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FullSerializer;
using Games.Global;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DeckBuilding
{   
    [Serializable]
    public class CardList
    {
        private List<Card> cards;
        public List<Deck> decks;

        // int : IdCard - int : number of this card
        private Dictionary<int, int> cardsInCollection;

        public CardList()
        {
            Debug.Log("init cardList");
            
            cards = new List<Card>();
            decks = new List<Deck>();
            cardsInCollection = new Dictionary<int, int>();
        }

        public List<Deck> GetDecks()
        {
            return decks;
        }

        public Deck GetDeckById(int id)
        {
            return Tools.Clone(decks.First(deck => deck.id == id));
        }

        public Card GetCardById(int id)
        {
            return Tools.Clone(cards.First(card => card.id == id));
        }

        public List<Card> GetCardsInCollection()
        {
            List<Card> collectionCards = new List<Card>();

            foreach (KeyValuePair<int, int> card in cardsInCollection)
            {
                collectionCards.Add(GetCardById(card.Key));
            }

            return collectionCards;
        }

        public int GetNbSpecificCardInCollection(int cardId)
        {
            if (cardsInCollection.ContainsKey(cardId))
            {
                return cardsInCollection[cardId];
            }

            return 0;
        }

        public int GetTotalDistinctCardsInCollection()
        {
            return cardsInCollection.Count;
        }

        public IEnumerator AddCardToCollection(Card card)
        {
            WWWForm form = new WWWForm();
            form.AddField("gameToken", NetworkingController.GameToken);
            form.AddField("accountToken", NetworkingController.AuthToken);
            form.AddField("cardId", card.id);
            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/account/update/cardCollection", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);

            if (www.responseCode == 201)
            {
                Debug.Log("Card added to collection");
            }
            else if (www.responseCode == 406)
            {
                Debug.LogWarning("Card not added to collection");
            }
            else if (www.responseCode == 403)
            {
                Debug.LogWarning("Verify fields (error 403)");
            }
            else if (www.responseCode == 401)
            {
                Debug.LogWarning("Unauthorized (error 401)");
            }
        }

        public void InitCards(string json)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            try
            {
                CardJsonList cardJsonList = null;
                data = fsJsonParser.Parse(json);
                serializer.TryDeserialize(data, ref cardJsonList);

                if (cardJsonList == null)
                {
                    return;
                }

                foreach (CardJsonObject card in cardJsonList.cards)
                {
                    cards.Add(card.ConvertToCard());
                }

                DictionaryManager.hasCardsLoad = true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.Data);
            }
        }

        public void InitDeck(string json)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            try
            {
                DeckJsonList deckJsonList = null;
                data = fsJsonParser.Parse(json);
                serializer.TryDeserialize(data, ref deckJsonList);

                if (deckJsonList == null)
                {
                    return;
                }

                foreach (DeckJsonObject deck in deckJsonList.decks)
                {
                    Deck deckConverted = deck.ConvertToDeck();
                    decks.Add(deckConverted);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.Data);
            }
        }

        public void InitCardCollection(string json)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            try
            {
                CollectionJsonList collectionList = null;
                data = fsJsonParser.Parse(json);
                serializer.TryDeserialize(data, ref collectionList);

                if (collectionList == null)
                {
                    return;
                }

                foreach (CardInCollection cardInCollection in collectionList.collections)
                {
                    cardsInCollection.Add(Int32.Parse(cardInCollection.cardId), Int32.Parse(cardInCollection.numbers));
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.Data);
            }
        }
    }
}