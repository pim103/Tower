using UnityEngine;

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
    
    // Class needed to Equipement and Material
    public class Item: MonoBehaviour
    {
        public int id = -1;

        public Rarity rarity = Rarity.Common;

        // In percent
        public int lootRate = 100;

        // Model Attribute
        public string modelName = "";
        public GameObject model = null;
        public GameObject instantiateModel;

        public void InstantiateModel(Transform parent = null)
        {
            GameObject modelWeapon = Instantiate(model, parent);
            BoxCollider bc = modelWeapon.GetComponent<BoxCollider>();
            bc.enabled = false;
            instantiateModel = modelWeapon;

            // TODO : need adaptation for the position of instantiate weapon
            float scaleY = modelWeapon.transform.localScale.y;
            modelWeapon.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
    }
}
