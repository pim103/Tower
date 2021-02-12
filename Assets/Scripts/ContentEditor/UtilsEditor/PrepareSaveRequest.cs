using System.Collections.Generic;
using Games.Global;
using Games.Global.Armors;
using Games.Global.Entities;
using Games.Global.Weapons;
using Networking;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.Networking;

namespace ContentEditor.UtilsEditor
{
    public class PrepareSaveRequest
    {
        public static void SaveChanges()
        {
            if (DataObject.EquipmentList != null)
            {
                bool weaponWasSaved = false;
                bool armorWasSaved = false;

                foreach (Weapon weapon in DataObject.EquipmentList.weapons)
                {
                    if (!Utils.Tools.IsSimilar(weapon, ContentGenerationEditor.weaponEditor.originalWeapon[weapon.id]))
                    {
                        Debug.Log("Need to save weapon " + weapon.id);
                        RequestSaveWeapon(weapon, false);
                        weaponWasSaved = true;
                    }
                }
                
                foreach (Armor armor in DataObject.EquipmentList.armors)
                {
                    if (!Utils.Tools.IsSimilar(armor, ContentGenerationEditor.armorEditor.originalArmor[armor.id]))
                    {
                        Debug.Log("Need to save armor " + armor.id);
                        RequestSaveArmor(armor, false);
                        armorWasSaved = true;
                    }
                }

                if (weaponWasSaved)
                {
                    ContentGenerationEditor.weaponEditor.CloneWeaponDictionary();
                }

                if (armorWasSaved)
                {
                    ContentGenerationEditor.armorEditor.CloneArmorDictionary();
                }
            }

            if (DataObject.MonsterList != null)
            {
                bool monsterWasSaved = false;

                foreach (Monster monster in DataObject.MonsterList.monsterList)
                {
                    if (!Utils.Tools.IsSimilar(monster, ContentGenerationEditor.monsterEditor.origMonsterList[monster.id]))
                    {
                        Debug.Log("Need to save monster " + monster.id);
                        RequestSaveMonster(monster, false);
                        monsterWasSaved = true;
                    }
                }
                
                foreach (GroupsMonster group in DataObject.MonsterList.groupsList)
                {
                    GroupsMonster originalGroup = ContentGenerationEditor.monsterEditor.origGroupsList[group.id];
                    List<MonsterInGroupTreatment> monsterInGroupTreatments =
                        ContentGenerationEditor.monsterEditor.GetTreatmentForMonsterInGroup(group.monstersInGroupList,
                            originalGroup.monstersInGroupList);
                    
                    if (!Utils.Tools.IsSimilar(group, originalGroup) && monsterInGroupTreatments.Count > 0)
                    {
                        Debug.Log("Need to save group " + group.id);
                        Debug.Log(Utils.Tools.IsSimilar(group, originalGroup));
                        Debug.Log(monsterInGroupTreatments.Count);
                        RequestSaveGroupMonster(group, false, monsterInGroupTreatments);
                        monsterWasSaved = true;
                    }
                }

                if (monsterWasSaved)
                {
                    ContentGenerationEditor.monsterEditor.CloneMonsterDictionary();
                }
            }
        }
        
        public static void RequestSaveWeapon(Weapon weapon, bool isNew)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", weapon.id);
            form.AddField("name", weapon.equipmentName);
            form.AddField("category", (int)weapon.category);
            form.AddField("type", (int)weapon.type);
            form.AddField("rarity", (int)weapon.rarity);
            form.AddField("lootRate", weapon.lootRate);
            form.AddField("cost", weapon.cost);
            form.AddField("damage", weapon.damage);
            form.AddField("attSpeed", (int)weapon.attSpeed);
            form.AddField("onDamageDealt", "");
            form.AddField("onDamageReceive", "");
            form.AddField("model", weapon.model ? UtilEditor.GetObjectInRessourcePath(weapon.model) : "");
            form.AddField("equipmentType", (int)weapon.equipmentType);
            form.AddField("spritePath", weapon.sprite ? UtilEditor.GetObjectInRessourcePath(weapon.sprite) : "");
            form.AddField("gameToken", NetworkingController.GameToken);

            UnityWebRequest www;
            if (isNew)
            {
                www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/equipment/add", form);
            }
            else
            {
                www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/equipment/update", form);
            }

            void Lambda() => ContentGenerationEditor.RequestLoadEquipment();

            ContentGenerationEditor.instance.StartCoroutine(DatabaseManager.SendData(www, Lambda));
        }
        
        public static void RequestSaveArmor(Armor armor, bool isNew)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", armor.id);
            form.AddField("name", armor.equipmentName);
            form.AddField("category", (int)armor.armorCategory);
            form.AddField("rarity", (int)armor.rarity);
            form.AddField("lootRate", armor.lootRate);
            form.AddField("cost", armor.cost);
            form.AddField("damage", armor.def);
            form.AddField("onDamageDealt", "");
            form.AddField("onDamageReceive", "");
            form.AddField("model", armor.model ? UtilEditor.GetObjectInRessourcePath(armor.model) : "");
            form.AddField("equipmentType", (int)armor.equipmentType);
            form.AddField("spritePath", armor.sprite ? UtilEditor.GetObjectInRessourcePath(armor.sprite) : "");
            form.AddField("gameToken", NetworkingController.GameToken);
            // Use for weapon
            form.AddField("type", 0);
            form.AddField("attSpeed", 0);

            UnityWebRequest www;
            if (isNew)
            {
                www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/equipment/add", form);
            }
            else
            {
                www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/equipment/update", form);
            }

            void Lambda() => ContentGenerationEditor.RequestLoadEquipment();

            ContentGenerationEditor.instance.StartCoroutine(DatabaseManager.SendData(www, Lambda));
        }

        public static void RequestSaveMonster(Monster monster, bool isNew)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", monster.id);
            form.AddField("typeWeapon", (int) monster.GetConstraint());
            form.AddField("name", monster.mobName);
            form.AddField("hp", (int) monster.hp);
            form.AddField("def", monster.def);
            form.AddField("att", (int) monster.att);
            form.AddField("speed", (int) monster.speed);
            form.AddField("nbWeapon", monster.nbWeapon);
            form.AddField("onDamageDealt", "");
            form.AddField("onDamageReceive", "");
            form.AddField("model", monster.model ? UtilEditor.GetObjectInRessourcePath(monster.model) : "");
            form.AddField("weaponId", monster.weaponOriginalId);
            form.AddField("attSpeed", (int) monster.attSpeed);
            form.AddField("spritePath", monster.sprite ? UtilEditor.GetObjectInRessourcePath(monster.sprite) : "");
            form.AddField("gameToken", NetworkingController.GameToken);

            UnityWebRequest www;
            if (isNew)
            {
                www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/monster/add", form);
            }
            else
            {
                www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/monster/update", form);
            }

            void Lambda() => ContentGenerationEditor.RequestLoadMonster();

            ContentGenerationEditor.instance.StartCoroutine(DatabaseManager.SendData(www, Lambda));
        }

        public static void RequestSaveGroupMonster(GroupsMonster group, bool isNew, List<MonsterInGroupTreatment> monsterInGroupTreatmentsList)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", group.id);
            form.AddField("family", (int) group.family);
            form.AddField("cost", group.cost);
            form.AddField("radius", group.radius);
            form.AddField("groupName", group.name);
            form.AddField("spritePath", group.sprite ? UtilEditor.GetObjectInRessourcePath(group.sprite) : "");
            form.AddField("gameToken", NetworkingController.GameToken);

            if (isNew)
            {
                foreach (MonstersInGroup monstersInGroup in group.monstersInGroupList)
                {
                    form.AddField("monster_groups_list_monsters[]", monstersInGroup.GetMonsterId());
                    form.AddField("monster_groups_list_nbMonster[]", monstersInGroup.nbMonster);
                }
            }
            else if (monsterInGroupTreatmentsList != null)
            {
                foreach (MonsterInGroupTreatment monsterInGroupTreatments in monsterInGroupTreatmentsList)
                {
                    form.AddField("monster_groups_list_monsters[]", monsterInGroupTreatments.monstersInGroup.GetMonsterId());
                    form.AddField("monster_groups_list_nbMonster[]", monsterInGroupTreatments.monstersInGroup.nbMonster);
                    form.AddField("monster_groups_list_treatment[]", (int) monsterInGroupTreatments.treatment);
                }

            }

            UnityWebRequest www;
            if (isNew)
            {
                www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/group/monster/add", form);
            }
            else
            {
                www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/group/monster/update", form);
            }

            void Lambda() => ContentGenerationEditor.RequestLoadMonster();
            ContentGenerationEditor.instance.StartCoroutine(DatabaseManager.SendData(www, Lambda));
        }
    }
}