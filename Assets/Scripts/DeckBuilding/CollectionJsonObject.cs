using System;
using Games.Global.Abilities;
using Games.Global.Weapons;
using UnityEngine;
using Utils;

namespace DeckBuilding
{
    public class CollectionJsonObject : ObjectParsed
    {
        private int type;
        private int id;
        private int copies;

        public void PrintAttribute()
        {
            Debug.Log("Object id : " + id + " type : " + type + " copies : " + copies);
        }

        public override void InsertValue(string key, string value)
        {
            switch (key)
            {
                case "id":
                    id = Int32.Parse(value);
                    break;
                case "type":
                    type = Int32.Parse(value);
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
            card.type = type;
            card.copies = copies;

            return card;
        }
    }
}
