﻿#if UNITY_EDITOR_64 || UNITY_EDITOR
using System.Collections.Generic;
using System.Diagnostics;
using ContentEditor.UtilsEditor;
using Games.Global;
using Games.Global.Weapons;
using Games.Players;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Tools = Utils.Tools;

namespace ContentEditor
{
    public class WeaponEditor : IEditorInterface
    {
        public bool createNewWeapon;
        public Dictionary<int, Weapon> originalWeapon = new Dictionary<int, Weapon>();
        public ContentGenerationEditor contentGenerationEditor;
        
        private Dictionary<Weapon, CreateOrSelectComponent<CategoryWeapon>> categoryWeaponSelectors = new Dictionary<Weapon, CreateOrSelectComponent<CategoryWeapon>>();

        private Weapon newWeapon;

        public void DisplayHeaderContent()
        {
            
        }

        public void DisplayBodyContent()
        {
            if (DataObject.EquipmentList == null)
            {
                return;
            }
            
            if (!createNewWeapon)
            {
                DisplayWeaponStat();
            }
            else
            {
                DisplayNewWeaponEditor();
            }
        }
        
        public void DisplayFooterContent()
        {
            string buttonLabel = "Créer une nouvelle arme";

            if (createNewWeapon)
            {
                if (GUILayout.Button("Reset la nouvelle arme"))
                {
                    newWeapon = new Weapon();
                }

                buttonLabel = "Changer les armes existantes";
            }
            
            if (GUILayout.Button(buttonLabel))
            {
                createNewWeapon = !createNewWeapon;
            }
        }

        private void DisplayNewWeaponEditor()
        {
            if (newWeapon == null)
            {
                newWeapon = new Weapon {equipmentType = EquipmentType.WEAPON};
            }
            
            DisplayOneWeaponEditor(newWeapon);

            GUILayout.FlexibleSpace();
        }

        private Vector2 scrollPos;
        private void DisplayWeaponStat()
        {
            EditorGUILayout.BeginHorizontal();

            int loop = 0;

            foreach (Weapon weapon in DataObject.EquipmentList.weapons)
            {
                DisplayOneWeaponEditor(weapon);
                
                ++loop;
                if (loop % 4 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DisplayOneWeaponEditor(Weapon weapon)
        {
            GUILayout.FlexibleSpace();
            Color defaultColor = GUI.color;
            
            GUI.color = Color.blue;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Button(weapon.sprite, GUILayout.Width(75), GUILayout.Height(75));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("ID", weapon.id);
            EditorGUI.EndDisabledGroup();

            weapon.equipmentName = EditorGUILayout.TextField("Name", weapon.equipmentName);
            weapon.damage = EditorGUILayout.IntField("Damage", weapon.damage);
            weapon.attSpeed = EditorGUILayout.FloatField("Attack speed", weapon.attSpeed);
            weapon.type = (TypeWeapon) EditorGUILayout.EnumPopup("Type", weapon.type);

            if (DictionaryManager.hasCategoriesLoad)
            {
                if (!categoryWeaponSelectors.ContainsKey(weapon)) {
                    categoryWeaponSelectors.Add(weapon, new CreateOrSelectComponent<CategoryWeapon>(DataObject.CategoryWeaponList.categories,
                        weapon.category, "Categorie", null));
                }

                weapon.category = categoryWeaponSelectors[weapon].DisplayOptions();
            }

            weapon.cost = EditorGUILayout.IntField("Cost", weapon.cost);
            weapon.rarity = (Rarity) EditorGUILayout.EnumPopup("Rarity", weapon.rarity);
            weapon.lootRate = EditorGUILayout.IntField("Loot Rate", weapon.lootRate);

            EditorGUILayout.LabelField("Model");
            weapon.model = (GameObject)EditorGUILayout.ObjectField(weapon.model, typeof(GameObject), false);

            EditorGUILayout.LabelField("Sprite");
            weapon.sprite = (Texture2D)EditorGUILayout.ObjectField(weapon.sprite, typeof(Texture2D), false);

            if (GUILayout.Button("Instantiate weapon") && UtilEditor.IsTestScene())
            {
                Player player = UtilEditor.GetPlayerFromSceneTest();

                if (player == null)
                {
                    Debug.LogError("Can't find player object in current scene");
                }
                else
                {
                    player.InitWeapon(weapon);
                }
            }
            
            Color currentColor = GUI.color;
            GUI.color = Color.green;
            if (GUILayout.Button("Sauvegarder l'arme'"))
            {
                PrepareSaveRequest.RequestSaveWeapon(weapon, weapon.id == 0);
                newWeapon = null;
            }
            GUI.color = currentColor;

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }

        public void CloneWeaponDictionary()
        {
            originalWeapon.Clear();

            foreach (Weapon weapon in DataObject.EquipmentList.weapons)
            {
                originalWeapon.Add(weapon.id, Tools.Clone(weapon));
            }
        }
    }
}
#endif