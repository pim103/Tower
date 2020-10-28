using System.Collections.Generic;
using Games.Global;
using Games.Global.Armors;
using UnityEditor;
using UnityEngine;
using Tools = Utils.Tools;

namespace ContentEditor
{
    public class ArmorEditor : IEditorInterface
    {
        public bool createNewArmor;
        public Dictionary<int, Armor> originalArmor = new Dictionary<int, Armor>();
        public ContentGenerationEditor contentGenerationEditor;
        
        private Armor newArmor;

        public void DisplayHeaderContent()
        {
            
        }

        public void DisplayBodyContent()
        {
            if (DataObject.EquipmentList == null)
            {
                return;
            }
            
            if (!createNewArmor)
            {
                DisplayArmorStat();
            }
            else
            {
                DisplayNewArmorEditor();
            }
        }

        public void DisplayFooterContent()
        {
            string buttonLabel = "Créer une nouvelle armure";

            if (createNewArmor)
            {
                if (GUILayout.Button("Reset la nouvelle armure"))
                {
                    newArmor = new Armor();
                }
                
                buttonLabel = "Changer les armures existantes";
            }
            
            if (GUILayout.Button(buttonLabel))
            {
                createNewArmor = !createNewArmor;
            }
        }

        private void DisplayNewArmorEditor()
        {
            if (newArmor == null)
            {
                newArmor = new Armor();
                newArmor.equipmentType = EquipmentType.ARMOR;
            }
            
            GUILayout.FlexibleSpace();
            
            DisplayOneArmorEditor(newArmor);

            if (GUILayout.Button("Sauvegarder la nouvelle armure"))
            {
                contentGenerationEditor.RequestSaveArmor(newArmor, true);
                newArmor = null;
            }

            GUILayout.FlexibleSpace();
        }

        private void DisplayArmorStat()
        {
            EditorGUILayout.BeginHorizontal();
            int loop = 0;

            foreach (Armor armor in DataObject.EquipmentList.armors)
            {
                DisplayOneArmorEditor(armor);

                ++loop;
                if (loop % 6 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DisplayOneArmorEditor(Armor armor)
        {
            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Button(armor.sprite, GUILayout.Width(75), GUILayout.Height(75));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("ID", armor.id);
            EditorGUI.EndDisabledGroup();

            armor.equipmentName = EditorGUILayout.TextField("Name", armor.equipmentName);
            armor.def = EditorGUILayout.IntField("Damage", armor.def);
            armor.armorCategory = (CategoryArmor) EditorGUILayout.EnumPopup("Category", armor.armorCategory);
            armor.cost = EditorGUILayout.IntField("Cost", armor.cost);
            armor.rarity = (Rarity) EditorGUILayout.EnumPopup("Rarity", armor.rarity);
            armor.lootRate = EditorGUILayout.IntField("Loot Rate", armor.lootRate);
            armor.modelName = EditorGUILayout.TextField("Model Name", armor.modelName);
            EditorGUILayout.LabelField("Sprite");
                
            armor.sprite = (Texture2D)EditorGUILayout.ObjectField(armor.sprite, typeof(Texture2D), false);

            EditorGUILayout.EndVertical();
        }

        public void CloneArmorDictionary()
        {
            originalArmor.Clear();
            
            foreach (Armor armor in DataObject.EquipmentList.armors)
            {
                originalArmor.Add(armor.id, Tools.Clone(armor));
            }
        }
    }
}