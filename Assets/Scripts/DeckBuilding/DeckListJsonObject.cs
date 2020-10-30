using System;
using UnityEngine;
using Utils;

namespace DeckBuilding
{
    public class DeckListJsonObject: ObjectParsed
    {
        private int deckId;
        private int id;
        private int copies;

        public void PrintAttribute()
        {
            Debug.Log("Object id : " + id + " deckId : "+ deckId + " copies : "+copies);
        }
    
        public override void InsertValue(string key, string value)
        {
            switch (key)
            {
                case "id":
                    id = Int32.Parse(value);
                    break;
                case "deckId":
                    deckId = Int32.Parse(value);
                    break;
                case "copies":
                    copies = Int32.Parse(value);
                    break;
            }
        }

        public override void DoSomething()
        {
            throw new NotImplementedException();
        }

        public Card ConvertToCard()
        {
            Card card = new Card();

            card.id = id;
            //card.deckId = deckId;
            //card.copies = copies;

            return card;
        }
    }
}
