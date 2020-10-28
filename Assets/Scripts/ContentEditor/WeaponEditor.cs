using System.Collections.Generic;
using Games.Global;
using Games.Global.Weapons;
using UnityEditor;
using UnityEngine;
using Tools = Utils.Tools;

namespace ContentEditor
{
    public class WeaponEditor : IEditorInterface
    {
        public bool createNewWeapon;
        public Dictionary<int, Weapon> originalWeapon = new Dictionary<int, Weapon>();
        public ContentGenerationEditor contentGenerationEditor;
        
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
                newWeapon = new Weapon();
                newWeapon.equipmentType = EquipmentType.WEAPON;
            }
            
            GUILayout.FlexibleSpace();
            
            DisplayOneWeaponEditor(newWeapon);

            if (GUILayout.Button("Sauvegarder la nouvelle arme"))
            {
                contentGenerationEditor.RequestSaveWeapon(newWeapon, true);
                newWeapon = null;
            }

            GUILayout.FlexibleSpace();
        }

        private void DisplayWeaponStat()
        {
            EditorGUILayout.BeginHorizontal();

            int loop = 0;

            foreach (Weapon weapon in DataObject.EquipmentList.weapons)
            {
                DisplayOneWeaponEditor(weapon);

                ++loop;
                if (loop % 6 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DisplayOneWeaponEditor(Weapon weapon)
        {
            EditorGUILayout.BeginVertical();

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
            weapon.category = (CategoryWeapon) EditorGUILayout.EnumPopup("Category", weapon.category);
            weapon.cost = EditorGUILayout.IntField("Cost", weapon.cost);
            weapon.rarity = (Rarity) EditorGUILayout.EnumPopup("Rarity", weapon.rarity);
            weapon.lootRate = EditorGUILayout.IntField("Loot Rate", weapon.lootRate);
            weapon.modelName = EditorGUILayout.TextField("Model Name", weapon.modelName);
            EditorGUILayout.LabelField("Sprite");
                
            weapon.sprite = (Texture2D)EditorGUILayout.ObjectField(weapon.sprite, typeof(Texture2D), false);

            EditorGUILayout.EndVertical();
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