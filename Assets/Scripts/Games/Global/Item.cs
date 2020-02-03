using UnityEngine;

namespace Games.Global
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        VeryRare,
        Epic,
        Legendary
    }
    
    // Class needed to Equipement and Material
    public class Item : MonoBehaviour
    {
        public Rarity rarity = Rarity.Common;
        
        // In percent
        public int lootRate = 100;
    }
}
