namespace Games.Global
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public struct InstantiateParameters
    {
        public TypeItem type;
        public object item;
    }
    
    // Class needed to Equipement and Material
    public class Item: ItemModel
    {
        public int id = -1;

        public Rarity rarity = Rarity.Common;

        // In percent
        public int lootRate = 100;
    }
}
