using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DeckBuilding
{
    [Serializable]
    public class DeckJsonList
    {
        public List<DeckJsonObject> decks { get; set; }
    }

    [Serializable]
    public class CardInDeck
    {
        public string cardId { get; set; }
        public string number { get; set; }
    }

    [Serializable]
    public class DeckJsonObject
    {
        public string id { get; set; }
        public string deckName { get; set; }
        public string isMonsterDeck { get; set; }
        public List<CardInDeck> cards { get; set; }

        public Deck ConvertToDeck()
        {
            Deck deck = new Deck
            {
                id = Int32.Parse(id),
                name = deckName,
                type = (isMonsterDeck == "1" ? Decktype.Monsters : Decktype.Equipments),
            };
            
            deck.cards = new Dictionary<int, int>();

            foreach (CardInDeck cardInDeck in cards)
            {
                deck.cards.Add(Int32.Parse(cardInDeck.cardId), Int32.Parse(cardInDeck.number));
            }

            return deck;
        }
    }
    
    [Serializable]
    public class CollectionJsonList
    {
        public List<CardInCollection> collections { get; set; }
    }

    [Serializable]
    public class CardInCollection 
    {
        public string cardId { get; set; }
        public string numbers { get; set; }
    }

    public enum Decktype
    {
        Monsters,
        Equipments
    }
    
    public class Deck
    {
        public int id { get; set; }
        public string name { get; set; }
        public Decktype type { get; set; }
        
        // Card id - Number of same cards
        public Dictionary<int, int> cards { get; set; }

        private int CountRemainingCards()
        {
            int nbCard = 0;
            
            foreach (KeyValuePair<int, int> cardsPair in cards)
            {
                nbCard += cardsPair.Value;
            }

            return nbCard;
        }
        
        public Card DrawRandomCard()
        {
            int nbCard;
            if ((nbCard = CountRemainingCards()) <= 0)
            {
                return null;
            }

            int randomNumberCard = Random.Range(0, nbCard);
            int index = 0;
            int idCard = 0;

            foreach (KeyValuePair<int, int> cardsPair in cards)
            {
                if (cardsPair.Value == 0)
                {
                    continue;
                }

                if (index <= randomNumberCard && index + cardsPair.Value >= randomNumberCard)
                {
                    idCard = cardsPair.Key;
                    break;
                }

                index += cardsPair.Value;
            }

            if (idCard == 0)
            {
                return null;
            }

            cards[idCard] = cards[idCard] - 1;

            return DataObject.CardList.GetCardById(idCard);
        }
    }
}
