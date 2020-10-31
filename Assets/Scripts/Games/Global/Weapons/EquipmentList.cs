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
        private GameObject[] equipmentGameObject;

        public List<Weapon> weapons;
        public List<Armor> armors;

        public EquipmentList(GameObject[] list, string jsonObject)
        {
            equipmentGameObject = list;

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

        public void PrintDictionnary()
        {
            foreach (Weapon weapon in weapons)
            {
                Debug.Log(weapon);
                Debug.Log(weapon.category);
                Debug.Log(weapon.damage);
            }
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

                        if (equipmentGameObject != null && equipmentGameObject.Length > 0 && equipmentGameObject.ToList().Exists(go => go.name == loadedWeapon.modelName))
                        {
                            loadedWeapon.model = equipmentGameObject.First(go => go.name == loadedWeapon.modelName);
                        }

                        weapons.Add(loadedWeapon);
                    }
                    else
                    {
                        Armor loadedArmor = equipmentJsonObject.ConvertToArmor();

                        if (equipmentGameObject != null && equipmentGameObject.Length > 0 && equipmentGameObject.ToList().Exists(go => go.name == loadedArmor.modelName))
                        {
                            loadedArmor.model = equipmentGameObject.First(go => go.name == loadedArmor.modelName);
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