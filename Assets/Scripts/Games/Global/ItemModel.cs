using Games.Global.Entities;
using Games.Global.Patterns;
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
    
    public class ItemModel: MonoBehaviour
    {
        // Model Attribute
        public string modelName = "";
        public GameObject model = null;
        public GameObject instantiateModel;

        // TODO : Need to work with of an object and not the dictionary
        public void InstantiateModel(InstantiateParameters param, Vector3 localization, Transform parent = null)
        {
            GameObject modelItem = Instantiate(model, parent);
            
            if (parent)
            {
                localization = Vector3.zero;
            }

            modelItem.transform.localPosition = localization;
            instantiateModel = modelItem;

            switch (param.type)
            {
                case TypeItem.Weapon:
                    BoxCollider bc = modelItem.GetComponent<BoxCollider>();
                    bc.enabled = false;

                    WeaponPrefab wp = modelItem.GetComponent<WeaponPrefab>();
                    wp.SetWeapon((Weapon)param.item);
                    wp.SetWielder((Entity)param.wielder);

                    // TODO : need adaptation for the position of instantiate weapon
                    float scaleY = modelItem.transform.localScale.y;
                    modelItem.transform.localPosition = new Vector3(0, 0.5f, 0);
                    break;
                case TypeItem.Monster:
                    MobPrefab mp = modelItem.GetComponent<MobPrefab>();
                    mp.SetMonster((Monster)param.item);
                    ((Monster)param.item).movementPatternController = instantiateModel.AddComponent<MovementPatternController>();
                    break;
            }
        }
    }
}