using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FullSerializer;
using Games.Global.Armors;
using UnityEngine;
using UnityEngine.XR;
using Utils;

namespace Games.Global.Weapons
{
    public class EquipmentList
    {
        public List<Weapon> weapons;
        public List<Armor> armors;

        public EquipmentList(string jsonObject)
        {
            weapons = new List<Weapon>();
            armors = new List<Armor>();
            InitEquipmentDictionnary(jsonObject);
        }

        public Weapon GetWeaponWithName(string findName)
        {
            Weapon findingWeapon = weapons.First(we => we.equipmentName == findName);
            Weapon cloneWeapon = Tools.Clone(findingWeapon);
            return cloneWeapon;
        }

        public Weapon GetWeaponWithId(int id)
        {
            Weapon findingWeapon = weapons.First(we => we.id == id);
            Weapon cloneWeapon = Tools.Clone(findingWeapon);
            return cloneWeapon;
        }
        
        public Armor GetArmorWithName(string findName)
        {
            Armor findingWeapon = armors.First(ar => ar.equipmentName == findName);
            Armor cloneWeapon = Tools.Clone(findingWeapon);
            return cloneWeapon;
        }

        public Armor GetArmorWithId(int id)
        {
            Armor findingWeapon = armors.First(we => we.id == id);
            Armor cloneWeapon = Tools.Clone(findingWeapon);
            return cloneWeapon;
        }

        public Weapon GetFirstWeaponFromIdCategory(int idCategory)
        {
            return Tools.Clone(weapons.Find(weapon => weapon.category.id == idCategory));
        }

        private void InitEquipmentDictionnary(string json)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            try
            {
                EquipmentJsonList equipmentList = null;
                data = fsJsonParser.Parse(json);
                serializer.TryDeserialize(data, ref equipmentList);

                foreach (EquipmentJsonObject equipmentJsonObject in equipmentList.equipment)
                {
                    EquipmentType type = (EquipmentType) Int32.Parse(equipmentJsonObject.equipmentType);

                    if (type == EquipmentType.WEAPON)
                    {
                        Weapon loadedWeapon = equipmentJsonObject.ConvertToWeapon();

                        GameObject weaponModel = Resources.Load(loadedWeapon.modelName) as GameObject;
                        if (weaponModel)
                        {
                            loadedWeapon.model = weaponModel;
                        }

                        weapons.Add(loadedWeapon);
                    }
                    else
                    {
                        Armor loadedArmor = equipmentJsonObject.ConvertToArmor();

                        GameObject armorModel = Resources.Load(loadedArmor.modelName) as GameObject;
                        if (armorModel)
                        {
                            loadedArmor.model = armorModel;
                        }

                        armors.Add(loadedArmor);
                    }

                }

                DictionaryManager.hasWeaponsLoad = true;
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
}