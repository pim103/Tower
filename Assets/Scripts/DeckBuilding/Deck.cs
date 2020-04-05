namespace DeckBuilding
{
    public class Deck
    {
        public enum Decktype
        {
            Monsters,
            Equipments
        }
        
        public int id;
        public string name;
        public Decktype type;
    }
}
