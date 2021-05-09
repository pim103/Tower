using System;
using System.Collections.Generic;
using FullSerializer;
using Games.Global.Spells;
using UnityEngine;
using Utils;

namespace Games.Global.Weapons
{
    public class CategoryWeaponList
    {
        public List<CategoryWeapon> categories;

        public CategoryWeaponList(string json)
        {
            categories = new List<CategoryWeapon>();
            InitCategoriesFromJson(json);
        }

        public CategoryWeapon GetCategoryFromId(int id)
        {
            CategoryWeapon categoryWeapon = categories?.Find(c => c.id == id);

            return categoryWeapon != null ? Tools.Clone(categoryWeapon) : null;
        }

        private void InitCategoriesFromJson(string json)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            try
            {
                CategoryWeaponListJsonObject categoryWeaponListJsonObject = null;
                data = fsJsonParser.Parse(json);
                serializer.TryDeserialize(data, ref categoryWeaponListJsonObject);

                foreach (CategoryWeaponJsonObject categoryWeaponJsonObject in categoryWeaponListJsonObject.categories)
                {
                    categories.Add(categoryWeaponJsonObject.ConvertToCategoryWeapon());
                }

                DictionaryManager.hasCategoriesLoad = true;
            }
            catch (Exception e)
            {
                Debug.Log("Error");
                Debug.Log(json);
                Debug.Log(e.Message);
                Debug.Log(e.Data);
            }
        }
    }
    
    public class CategoryWeapon
    {
        public int id { get; set; }
        public string name { get; set; }
        public Spell spellAttack { get; set; }
    }
}