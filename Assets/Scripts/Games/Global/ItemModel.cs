using Games.Global.Entities;
//using Games.Global.Patterns;
using Games.Global.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Global
{
    public enum TypeItem
    {
        Weapon,
        Monster
    }
    
    public class ItemModel
    {
        // Model Attribute
        public string modelName { get; set; } = "";
        public GameObject model { get; set; } = null;
    }
}