//using System;
//using Games.Global.Abilities;
//using Games.Global.Weapons;
//using UnityEngine;
//using Utils;
//
//namespace DeckBuilding
//{
//    [Serializable]
//    public class DeckJsonObject: ObjectParsed
//    {
//        public string id { get; set; }
//        public string deck_name { get; set; }
//        public string type { get; set; }
//
//        public string account_id { get; set; }
//    
//        public void PrintAttribute()
//        {
//            Debug.Log("Object id : " + id + " name : " + deck_name);
//        }
//    
//        public override void InsertValue(string key, string value)
//        {
//            switch (key)
//            {
//                case "id":
//                    id = value;
//                    break;
//                case "name":
//                    deck_name = value;
//                    break;
//                case "type":
//                    type = value;
//                    break;
//            }
//        }
//
//        public override void DoSomething()
//        {
//            throw new NotImplementedException();
//        }
//
//        public Deck ConvertToDeck()
//        {
//            Deck deck = new Deck();
//
//            deck.id = Int32.Parse(id);
//            deck.name = deck_name;
//            deck.type = (Deck.Decktype)Int32.Parse(type);
//
//            return deck;
//        }
//    }
//}
