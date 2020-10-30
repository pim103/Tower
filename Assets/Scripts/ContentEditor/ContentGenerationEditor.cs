#if UNITY_EDITOR_64 || UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Games.Global;
using Games.Global.Armors;
using Games.Global.Entities;
using Games.Global.Weapons;
using Networking;
using Networking.Client;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace ContentEditor
{
    public static class EditorConstant
    {
        public static float MID_WIDTH;
        public static float MID_HEIGHT;
        public static float WIDTH;
        public static float HEIGHT;
        public static float QUART_WIDTH;
        public static float QUART_HEIGHT;
        public static float EIGHTH_WIDTH;
        public static float EIGHTH_HEIGHT;

        public static Texture2D weaponSprite;
        public static Texture2D spellSprite;
        public static Texture2D monsterSprite;
        public static Texture2D armorSprite;
        public static Texture2D playerSprite;

        public static void ResetConst()
        {
            WIDTH = Screen.width;
            HEIGHT = Screen.height;
            MID_WIDTH = WIDTH / 2;
            MID_HEIGHT = HEIGHT / 2;
            QUART_WIDTH = WIDTH / 4;
            QUART_HEIGHT = HEIGHT / 4;
            EIGHTH_WIDTH = WIDTH / 8;
            EIGHTH_HEIGHT = HEIGHT / 8;
        }

        public static void LoadTextures()
        {
            weaponSprite = Resources.Load<Texture2D>("Editor/sword");
            spellSprite = Resources.Load<Texture2D>("Editor/spell");
            playerSprite = Resources.Load<Texture2D>("Editor/Warrior");
            monsterSprite = Resources.Load<Texture2D>("Editor/monster");
            armorSprite = Resources.Load<Texture2D>("Editor/armor");
        }
    }
    
    public class ContentGenerationEditor : EditorWindow
    {
        private enum Category
        {
            WEAPON,
            ARMOR,
            SPELL,
            MONSTER,
            PLAYER
        }

        private Category currentCategory;

        [MenuItem("Tools/Content generation")]
        public static void ShowWindow ()
        {
            GetWindow<ContentGenerationEditor>("Content generation");
            EditorConstant.LoadTextures();
        }

        private void CheckInitEditor()
        {
            if (weaponEditor.contentGenerationEditor == null)
            {
                weaponEditor.contentGenerationEditor = this;
            }
            if (armorEditor.contentGenerationEditor == null)
            {
                armorEditor.contentGenerationEditor = this;
            }
            if (monsterEditor.contentGenerationEditor == null)
            {
                monsterEditor.contentGenerationEditor = this;
            }
        }
        
        private Vector2 scrollPos;
        private WeaponEditor weaponEditor = new WeaponEditor();
        private ArmorEditor armorEditor = new ArmorEditor();
        private MonsterEditor monsterEditor = new MonsterEditor();
        private PlayerEditor playerEditor = new PlayerEditor();
        private SpellEditor spellEditor = new SpellEditor();
        private IEditorInterface currentEditor;

        public void DisplayHeader()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(EditorConstant.weaponSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                currentCategory = Category.WEAPON;
                currentEditor = weaponEditor;
            }
            else if (GUILayout.Button(EditorConstant.armorSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                currentCategory = Category.ARMOR;
                currentEditor = armorEditor;
            }
            else if (GUILayout.Button(EditorConstant.monsterSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                monsterEditor.CreateWeaponChoiceList();
                currentCategory = Category.MONSTER;
                currentEditor = monsterEditor;
            }
            else if (GUILayout.Button(EditorConstant.playerSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                currentCategory = Category.PLAYER;
                currentEditor = playerEditor;
            }
            else if (GUILayout.Button(EditorConstant.spellSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                currentCategory = Category.SPELL;
                currentEditor = spellEditor;
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Load data!"))
            {
                LoadData();
            }

            currentEditor?.DisplayHeaderContent();

            GUILayout.FlexibleSpace();
        }
        
        void OnGUI ()
        {
            DictionaryManager.InitAbility();
            GUILayout.Label("Test custom editor", EditorStyles.boldLabel);

            // INIT VAR
            CheckInitEditor();

            // RESET CONST
            EditorConstant.ResetConst();

            // INIT DISPLAY HEADER
            GUILayout.BeginArea(new Rect(0,0, EditorConstant.WIDTH, EditorConstant.EIGHTH_HEIGHT * 2));
            DisplayHeader();
            GUILayout.EndArea();
            // END DISPLAY HEADER
            
            // INIT FORM DISPLAY
            GUILayout.BeginArea(new Rect(0,EditorConstant.EIGHTH_HEIGHT * 2, EditorConstant.WIDTH, EditorConstant.EIGHTH_HEIGHT * 5));
            
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            currentEditor?.DisplayBodyContent();

            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
            // END FORM DISPLAY

            // INIT DISPLAY FOOTER
            DisplayFooter();
        }

        private void SaveChanges()
        {
            if (DataObject.EquipmentList != null)
            {
                bool weaponWasSaved = false;
                bool armorWasSaved = false;

                foreach (Weapon weapon in DataObject.EquipmentList.weapons)
                {
                    if (!Utils.Tools.IsSimilar(weapon, weaponEditor.originalWeapon[weapon.id]))
                    {
                        Debug.Log("Need to save weapon " + weapon.id);
                        RequestSaveWeapon(weapon, false);
                        weaponWasSaved = true;
                    }
                }
                
                foreach (Armor armor in DataObject.EquipmentList.armors)
                {
                    if (!Utils.Tools.IsSimilar(armor, armorEditor.originalArmor[armor.id]))
                    {
                        Debug.Log("Need to save armor " + armor.id);
                        RequestSaveArmor(armor, false);
                        armorWasSaved = true;
                    }
                }

                if (weaponWasSaved)
                {
                    weaponEditor.CloneWeaponDictionary();
                }

                if (armorWasSaved)
                {
                    armorEditor.CloneArmorDictionary();
                }
            }

            if (DataObject.MonsterList != null)
            {
                bool monsterWasSaved = false;

                foreach (Monster monster in DataObject.MonsterList.monsterList)
                {
                    if (!Utils.Tools.IsSimilar(monster, monsterEditor.origMonsterList[monster.id]))
                    {
                        Debug.Log("Need to save monster " + monster.id);
                        RequestSaveMonster(monster, false);
                        monsterWasSaved = true;
                    }
                }
                
                foreach (GroupsMonster group in DataObject.MonsterList.groupsList)
                {
                    GroupsMonster originalGroup = monsterEditor.origGroupsList[group.id];
                    List<MonsterInGroupTreatment> monsterInGroupTreatments =
                        monsterEditor.GetTreatmentForMonsterInGroup(group.monstersInGroupList,
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
                    monsterEditor.CloneMonsterDictionary();
                }
            }
        }

        public void DisplayFooter()
        {
            GUILayout.FlexibleSpace();

            currentEditor?.DisplayFooterContent();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save", GUILayout.Width(75), GUILayout.Height(25)))
            {
                SaveChanges();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private string GetSpritePath(Texture2D sprite)
        {
            string spritePath = AssetDatabase.GetAssetPath(sprite);
            const string resourcesFolder = "Resources/";

            if (spritePath.Contains(resourcesFolder))
            {
                int indexOfResources = spritePath.IndexOf(resourcesFolder, StringComparison.CurrentCulture);
                spritePath = spritePath.Substring(indexOfResources + resourcesFolder.Length);

                int indexOfExtension = spritePath.IndexOf(".", StringComparison.CurrentCulture);
                if (indexOfExtension != -1)
                {
                    spritePath = spritePath.Substring(0, indexOfExtension);
                }
            }
            else
            {
                Debug.Log("L'image n'est pas dans Resource !");
                spritePath = "";
            }

            return spritePath;
        }

        public void RequestSaveWeapon(Weapon weapon, bool isNew)
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
            form.AddField("model", weapon.modelName);
            form.AddField("equipmentType", (int)weapon.equipmentType);
            form.AddField("spritePath", weapon.sprite != null ? GetSpritePath(weapon.sprite) : "");
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

            void Lambda() => RequestLoadEquipment();

            this.StartCoroutine(SendData(www, Lambda));
        }
        
        public void RequestSaveArmor(Armor armor, bool isNew)
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
            form.AddField("model", armor.modelName);
            form.AddField("equipmentType", (int)armor.equipmentType);
            form.AddField("spritePath", armor.sprite != null ? GetSpritePath(armor.sprite) : "");
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

            void Lambda() => RequestLoadEquipment();

            this.StartCoroutine(SendData(www, Lambda));
        }

        public void RequestSaveMonster(Monster monster, bool isNew)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", monster.id);
            form.AddField("typeWeapon", (int) monster.constraint);
            form.AddField("name", monster.mobName);
            form.AddField("hp", (int) monster.hp);
            form.AddField("def", monster.def);
            form.AddField("att", (int) monster.att);
            form.AddField("speed", (int) monster.speed);
            form.AddField("nbWeapon", monster.nbWeapon);
            form.AddField("onDamageDealt", "");
            form.AddField("onDamageReceive", "");
            form.AddField("model", monster.modelName);
            form.AddField("weaponId", monster.weaponOriginalId);
            form.AddField("attSpeed", (int) monster.attSpeed);
            form.AddField("spritePath", monster.sprite != null ? GetSpritePath(monster.sprite) : "");
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

            void Lambda() => RequestLoadMonster();

            this.StartCoroutine(SendData(www, Lambda));
        }

        public void RequestSaveGroupMonster(GroupsMonster group, bool isNew, List<MonsterInGroupTreatment> monsterInGroupTreatmentsList)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", group.id);
            form.AddField("family", (int) group.family);
            form.AddField("cost", group.cost);
            form.AddField("radius", group.radius);
            form.AddField("groupName", group.name);
            form.AddField("spritePath", group.sprite != null ? GetSpritePath(group.sprite) : "");
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

            void Lambda() => RequestLoadMonster();
            this.StartCoroutine(SendData(www, Lambda));
        }

        IEnumerator SendData(UnityWebRequest www)
        {
            yield return SendData(www, null);
        }
        
        IEnumerator SendData(UnityWebRequest www, Action successEndCallback)
        {
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            
            if (www.responseCode == 201)
            {
                successEndCallback();
                Debug.Log("Request was send");
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("ERROR");
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
            }
        }

        void LoadData()
        {
            RequestLoadEquipment();
            RequestLoadMonster();
        }

        public void RequestLoadEquipment()
        {
            this.StartCoroutine(LoadEquipment());
        }

        public IEnumerator LoadEquipment()
        {
            var www = UnityWebRequest.Get(NetworkingController.PublicURL + "/api/v1/equipment/list");
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            
            if (www.responseCode == 200)
            {
                DataObject.EquipmentList = new EquipmentList(null, www.downloadHandler.text);
                weaponEditor.CloneWeaponDictionary();
                armorEditor.CloneArmorDictionary();
                monsterEditor.CreateWeaponChoiceList();
            }
            else
            {
                Debug.Log("Can't get equipment...");
            }
        }

        public void RequestLoadMonster()
        {
            this.StartCoroutine(LoadMonster());
        }
        
        private IEnumerator LoadMonster()
        {
            var www = UnityWebRequest.Get(NetworkingController.PublicURL + "/services/game/group/list.php");
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 200)
            {
                DataObject.MonsterList = new MonsterList(null, www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Can't get Monsters...");
            }
    
            www = UnityWebRequest.Get("https://towers.heolia.eu/services/game/monster/list.php");
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 200)
            {
                DataObject.MonsterList.InitSpecificMonsterList(www.downloadHandler.text);
                monsterEditor.CloneMonsterDictionary();
            }
            else
            {
                Debug.Log("Can't get Monsters...");
            }
        }
    }
}
#endif