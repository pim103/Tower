using System.Collections.Generic;
using System.Linq;
using ContentEditor.SpellEditorComposant;
using Games.Global.Spells;
using UnityEditor;
using UnityEngine;

namespace ContentEditor
{
    public enum SpellEditorCategory
    {
        NONE,
        EDIT_SPELL,
        EDIT_SPELL_COMPONENT,
        EDIT_TRAJECTORY,
        EDIT_SPELL_INSTANCIATION
    }

    public class SpellEditor : IEditorInterface
    {
        private Spell currentSpellEdited;
        private SpellComponent currentSpellComponentEdited;

        private SpellEditorCategory spellEditorCategory;

        private List<Spell> spells = new List<Spell>();
        public static List<SpellComponent> spellComponents = new List<SpellComponent>();
        public static List<string> spellComponentStringList = new List<string>();

        public void DisplayHeaderContent()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Paramètre du sort", GUILayout.Height(50)))
            {
                CreateSpellComponentList();
                spellEditorCategory = SpellEditorCategory.EDIT_SPELL;
            }

            if (GUILayout.Button("Paramètre des composants du sort", GUILayout.Height(50)))
            {
                spellEditorCategory = SpellEditorCategory.EDIT_SPELL_COMPONENT;
            }

            if (GUILayout.Button("Paramètre de trajectoire", GUILayout.Height(50)))
            {
                spellEditorCategory = SpellEditorCategory.EDIT_TRAJECTORY;
            }

            if (GUILayout.Button("Paramètre d'objet en jeu", GUILayout.Height(50)))
            {
                spellEditorCategory = SpellEditorCategory.EDIT_SPELL_INSTANCIATION;
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }

        public static void CreateSpellComponentList()
        {
            spellComponentStringList.Clear();
            spellComponentStringList.Add("Nothing");

            spellComponents.ForEach(spellCompo =>
            {
                spellComponentStringList.Add(spellCompo.nameSpellComponent);
            });
        }

        public void DisplayBodyContent()
        {
            switch (spellEditorCategory)
            {
                case SpellEditorCategory.EDIT_SPELL:
                    DisplaySpellEditor();
                    break;
                case SpellEditorCategory.EDIT_SPELL_COMPONENT:
                    if (currentSpellComponentEdited == null)
                    {
                        currentSpellComponentEdited = new SpellComponent();
                    }

                    SpellComponentEditor.DisplaySpellComponentEditor(currentSpellComponentEdited);
                    break;
                case SpellEditorCategory.EDIT_TRAJECTORY:
                    SpellTrajectoryEditor.DisplaySpellComponentTrajectory(currentSpellComponentEdited);
                    break;
                case SpellEditorCategory.EDIT_SPELL_INSTANCIATION:
                    SpellObjectEditor.DisplaySpellComponentObject(currentSpellComponentEdited);
                    break;
            }
        }

        private void DisplaySpellEditor()
        {
            if (currentSpellEdited == null)
            {
                currentSpellEdited = new Spell();
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Button(currentSpellEdited.sprite, GUILayout.Width(75), GUILayout.Height(75));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            currentSpellEdited.nameSpell = EditorGUILayout.TextField("Name", currentSpellEdited.nameSpell);
            currentSpellEdited.startFrom = (StartFrom) EditorGUILayout.EnumPopup("Le spell part de ", currentSpellEdited.startFrom);
            currentSpellEdited.cooldown = EditorGUILayout.FloatField("Cooldown", currentSpellEdited.cooldown);
            currentSpellEdited.cost = EditorGUILayout.FloatField("Cout en ressource", currentSpellEdited.cost);
            currentSpellEdited.castTime = EditorGUILayout.FloatField("Temps de cast", currentSpellEdited.castTime);
            currentSpellEdited.nbUse = EditorGUILayout.IntField("Nombre d'utilisation", currentSpellEdited.nbUse);

            EditorGUILayout.LabelField("Sprite");    
            currentSpellEdited.sprite = (Texture2D)EditorGUILayout.ObjectField(currentSpellEdited.sprite, typeof(Texture2D), false);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            int selectedActiveSpellComponent = currentSpellEdited.activeSpellComponent != null ? spellComponentStringList.IndexOf(currentSpellEdited.activeSpellComponent.nameSpellComponent) : -1;
            selectedActiveSpellComponent = EditorGUILayout.Popup("SpellComponent à l'activation", selectedActiveSpellComponent == -1 ? 0 : selectedActiveSpellComponent, spellComponentStringList.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                if (selectedActiveSpellComponent == 0)
                {
                    currentSpellEdited.activeSpellComponent = null;
                }
                else
                {
                    currentSpellEdited.activeSpellComponent = spellComponents[selectedActiveSpellComponent - 1];
                }
            }

            EditorGUI.BeginChangeCheck();
            int selectedPassiveSpellComponent = currentSpellEdited.passiveSpellComponent != null ? spellComponentStringList.IndexOf(currentSpellEdited.passiveSpellComponent.nameSpellComponent) : -1;
            selectedPassiveSpellComponent = EditorGUILayout.Popup("SpellComponent Passif", selectedPassiveSpellComponent == -1 ? 0 : selectedPassiveSpellComponent, spellComponentStringList.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                if (selectedPassiveSpellComponent == 0)
                {
                    currentSpellEdited.passiveSpellComponent = null;
                }
                else
                {
                    currentSpellEdited.passiveSpellComponent = spellComponents[selectedPassiveSpellComponent - 1];
                }
            }
            
            EditorGUI.BeginChangeCheck();
            int selectedRecast = currentSpellEdited.recastSpellComponent != null ? spellComponentStringList.IndexOf(currentSpellEdited.recastSpellComponent.nameSpellComponent) : -1;
            selectedRecast = EditorGUILayout.Popup("SpellComponent recast", selectedRecast == -1 ? 0 : selectedRecast, spellComponentStringList.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                if (selectedRecast == 0)
                {
                    currentSpellEdited.recastSpellComponent = null;
                }
                else
                {
                    currentSpellEdited.recastSpellComponent = spellComponents[selectedRecast - 1];
                }
            }

            EditorGUI.BeginChangeCheck();
            int selectedDurignCast = currentSpellEdited.duringCastSpellComponent != null ? spellComponentStringList.IndexOf(currentSpellEdited.duringCastSpellComponent.nameSpellComponent) : -1;
            selectedDurignCast = EditorGUILayout.Popup("SpellComponent during cast", selectedDurignCast == -1 ? 0 : selectedDurignCast, spellComponentStringList.ToArray());

            if (EditorGUI.EndChangeCheck() && selectedDurignCast != 0)
            {
                if (selectedDurignCast == 0)
                {
                    currentSpellEdited.duringCastSpellComponent = null;
                }
                else
                {
                    currentSpellEdited.duringCastSpellComponent = spellComponents[selectedDurignCast - 1];
                }
            }

            GUILayout.FlexibleSpace();
        }
        
        private void DisplaySpellComponentEditor()
        {
            
        }
        
        private void DisplayTrajectoryEditor()
        {
            
        }
        
        private void DisplaySpellInstanciation()
        {
            
        }

        public void DisplayFooterContent()
        {
            if (GUILayout.Button("Importer et modifier un spell", GUILayout.Height(25)))
            {
                
            }
        }
    }
}