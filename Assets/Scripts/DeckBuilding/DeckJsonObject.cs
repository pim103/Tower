using System;
using Games.Global.Abilities;
using Games.Global.Weapons;
using UnityEngine;
using Utils;

namespace DeckBuilding
{
    public class DeckJsonObject: ObjectParsed
    {
        private int id;
        private string name;
        private int type;
    
        public void PrintAttribute()
        {
            Debug.Log("Object id : " + id + " name : " + name);
        }
    
        public override void InsertValue(string key, string value)
        {
            switch (key)
            {
                case "id":
                    id = Int32.Parse(value);
                    break;
                case "name":
                    name = value;
                    break;
                case "type":
                    type = Int32.Parse(value);
                    break;
            }
        }

        public override void DoSomething()
        {
            throw new NotImplementedException();
        }

        public Deck ConvertToDeck()
        {
            Deck deck = new Deck();

            deck.id = id;
            deck.name = name;
            deck.type = (Deck.Decktype)type;

            return deck;
        }
    }
}
