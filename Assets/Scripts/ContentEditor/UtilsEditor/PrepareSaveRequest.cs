#if UNITY_EDITOR_64 || UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using FullSerializer;
using Games.Global;
using Games.Global.Armors;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Weapons;
using Games.Players;
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
            form.AddField("category", weapon.category.id);
            form.AddField("type", (int)weapon.type);
            form.AddField("rarity", (int)weapon.rarity);
            form.AddField("lootRate", weapon.lootRate);
            form.AddField("cost", weapon.cost);
            form.AddField("damage", weapon.damage);
            form.AddField("attSpeed", (int)weapon.attSpeed);
            form.AddField("onDamageDealt", "");
            form.AddField("onDamageReceive", "");
            form.AddField("model", weapon.model ? UtilEditor.GetObjectPathInRessourceFolder(weapon.model) : "");
            form.AddField("equipmentType", (int)weapon.equipmentType);
            form.AddField("spritePath", weapon.sprite ? UtilEditor.GetObjectPathInRessourceFolder(weapon.sprite) : "");
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
            form.AddField("model", armor.model ? UtilEditor.GetObjectPathInRessourceFolder(armor.model) : "");
            form.AddField("equipmentType", (int)armor.equipmentType);
            form.AddField("spritePath", armor.sprite ? UtilEditor.GetObjectPathInRessourceFolder(armor.sprite) : "");
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
            form.AddField("monsterType", (int) monster.monsterType);
            form.AddField("name", monster.mobName);
            form.AddField("hp", (int) monster.hp);
            form.AddField("def", monster.def);
            form.AddField("att", (int) monster.att);
            form.AddField("speed", (int) monster.speed);
            form.AddField("nbWeapon", monster.nbWeapon);
            form.AddField("onDamageDealt", "");
            form.AddField("onDamageReceive", "");
            form.AddField("model", monster.model ? UtilEditor.GetObjectPathInRessourceFolder(monster.model) : "");
            form.AddField("weaponId", monster.weaponOriginalId);
            form.AddField("attSpeed", (int) monster.attSpeed);
            form.AddField("spritePath", monster.sprite ? UtilEditor.GetObjectPathInRessourceFolder(monster.sprite) : "");
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
            form.AddField("spritePath", group.sprite ? UtilEditor.GetObjectPathInRessourceFolder(group.sprite) : "");
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

        public static void SaveClasses(Classes classes, bool isNew)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", classes.id);
            form.AddField("name", classes.name);
            form.AddField("hp", classes.hp);
            form.AddField("def", classes.def);
            form.AddField("att", classes.att);
            form.AddField("speed", classes.speed);
            form.AddField("attSpeed", classes.attSpeed);
            form.AddField("ressource", classes.ressource);
            form.AddField("spellDefense", classes.defenseSpell?.id ?? -1);
            form.AddField("gameToken", NetworkingController.GameToken);

            var www = isNew ? 
                UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/classes/add", form) : 
                UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/classes/update", form);

            void Lambda() => ContentGenerationEditor.RequestLoadClasses();

            ContentGenerationEditor.instance.StartCoroutine(DatabaseManager.SendData(www, Lambda));
        }

        public static void SaveSpellForClasses(ClassesWeaponSpell classesWeaponSpell, bool isNew)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", classesWeaponSpell.id);
            form.AddField("categoryWeapon", classesWeaponSpell.categoryWeapon.id);
            form.AddField("classes", classesWeaponSpell.classes.id);
            form.AddField("spell1", classesWeaponSpell.spell1?.id ?? -1);
            form.AddField("spell2", classesWeaponSpell.spell2?.id ?? -1);
            form.AddField("spell3", classesWeaponSpell.spell3?.id ?? -1);
            form.AddField("gameToken", NetworkingController.GameToken);

            var www = isNew ? 
                UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/classesCategory/add", form) : 
                UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/classesCategory/update", form);

            void Lambda() => ContentGenerationEditor.RequestLoadClasses();

            ContentGenerationEditor.instance.StartCoroutine(DatabaseManager.SendData(www, Lambda));
        }

        public static void SaveSpellForMonster(Monster monster, Spell spell)
        {
            WWWForm form = new WWWForm();
            form.AddField("monsterId", monster.id);
            form.AddField("skillId", spell.id);
            form.AddField("gameToken", NetworkingController.GameToken);

            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/monster/addSpell", form);

            void Lambda() => ContentGenerationEditor.RequestLoadMonster();

            ContentGenerationEditor.instance.StartCoroutine(DatabaseManager.SendData(www, Lambda));
        }

        public static void DeleteSpellForMonster(Monster monster, Spell spell)
        {
            WWWForm form = new WWWForm();
            form.AddField("monsterId", monster.id);
            form.AddField("skillId", spell.id);
            form.AddField("gameToken", NetworkingController.GameToken);

            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/monster/delSpell", form);

            void Lambda() => ContentGenerationEditor.RequestLoadMonster();

            ContentGenerationEditor.instance.StartCoroutine(DatabaseManager.SendData(www, Lambda));
        }

        public static void SaveCategoryWeapon(CategoryWeapon category, bool isNew)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", category.id);
            form.AddField("name", category.name);
            form.AddField("spellAttack", category.spellAttack?.id ?? -1);
            form.AddField("gameToken", NetworkingController.GameToken);

            var www = isNew ? 
                UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/category/add", form) : 
                UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/category/update", form);

            void Lambda() => ContentGenerationEditor.RequestLoadClasses();

            ContentGenerationEditor.instance.StartCoroutine(DatabaseManager.SendData(www, Lambda));
        }

        public static void SaveSpell(Dictionary<string, Spell> spellsToExport)
        {
            
            foreach (KeyValuePair<string, Spell> spellData in spellsToExport)
            {
                Spell currentSpell = spellData.Value;
                string pathNewSpell = Application.dataPath + "/Data/SpellsJson/" + spellData.Key + ".json";
                
                fsSerializer serializer = new fsSerializer();
                serializer.TrySerialize(currentSpell.GetType(), currentSpell, out fsData data);
                File.WriteAllText(pathNewSpell, fsJsonPrinter.CompressedJson(data));

                DeleteFtpFile(pathNewSpell);
                UploadFtpFile(pathNewSpell);
                
                WWWForm form = new WWWForm();
                form.AddField("id", currentSpell.id);
                form.AddField("name", spellData.Key);
                form.AddField("gameToken", NetworkingController.GameToken);

                UnityWebRequest www;
                if (currentSpell.id == -1)
                {
                    www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/skill/add", form);
                }
                else
                {
                    www = UnityWebRequest.Post(NetworkingController.PublicURL + "/api/v1/skill/update", form);
                }

                ContentGenerationEditor.instance.StartCoroutine(DatabaseManager.SendData(www));
            }
        }

        private static void UploadFtpFile(string filename)
        {
            FtpWebRequest request;
            
            request = WebRequest.Create(new Uri($@"ftp://{"heolia.eu"}/{"www/data/spell"}/{Path.GetFileName(filename)}")) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = true;
            request.Credentials = new NetworkCredential("towers", "f7pWu2heDgCH8jMi");    

            using (FileStream fs = File.OpenRead(filename))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                Stream stream = request.GetRequestStream();
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                stream.Close();
            }
        }

        private static bool DeleteFtpFile(string filename){
            FtpWebRequest request;

            request = WebRequest.Create(new Uri($@"ftp://{"heolia.eu"}/{"www/data/spell"}/{Path.GetFileName(filename)}")) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = true;    
            request.Credentials = new NetworkCredential("towers", "f7pWu2heDgCH8jMi");

            try
            {
                if (request.GetResponse() is FtpWebResponse response)
                {
                    return response.StatusCode == FtpStatusCode.CommandOK;
                }
            }
            catch (WebException e)
            {
            }

            return false;
        }
    }
}

#endif