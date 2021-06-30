#if UNITY_EDITOR_64 || UNITY_EDITOR
using System.Collections.Generic;
using ContentEditor.UtilsEditor;
using Games.Global;
using Games.Global.Armors;
using Games.Players;
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
            
            DisplayOneArmorEditor(newArmor);

            if (GUILayout.Button("Sauvegarder la nouvelle armure"))
            {
                PrepareSaveRequest.RequestSaveArmor(newArmor, true);
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
                if (loop % 4 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DisplayOneArmorEditor(Armor armor)
        {
            Color defaultColor = GUI.color;
            
            GUI.color = Color.blue;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;

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

            EditorGUILayout.LabelField("Model");
            armor.model = (GameObject)EditorGUILayout.ObjectField(armor.model, typeof(GameObject), false);

            EditorGUILayout.LabelField("Sprite");
            armor.sprite = (Texture2D)EditorGUILayout.ObjectField(armor.sprite, typeof(Texture2D), false);
            
            if (GUILayout.Button("Instantiate armor") && UtilEditor.IsTestScene())
            {
                Player player = UtilEditor.GetPlayerFromSceneTest();

                if (player == null)
                {
                    Debug.LogError("Can't find player object in current scene");
                }
                else
                {
                    Debug.Log("NEED TO IMPLEMENT ARMOR LOAD");
                }
            }

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
#endif