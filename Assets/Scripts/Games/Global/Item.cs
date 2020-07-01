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
        public TypeItem type { get; set; }
        public object item { get; set; }
        public object wielder { get; set; }
    }
    
    // Class needed to Equipement and Material
    public class Item: ItemModel
    {
        public int id { get; set; } = -1;

        public Rarity rarity { get; set; } = Rarity.Common;

        // In percent
        public int lootRate { get; set; } = 100;
    }
}
