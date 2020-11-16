using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ContentEditor.SpellEditorComposant;
using FullSerializer;
using Games.Global.Spells;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

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

    [Serializable]
    public class SpellEditor : IEditorInterface
    {
        [SerializeField]
        private static Spell currentSpellEdited;
        [SerializeField]
        private static SpellComponent currentSpellComponentEdited;
        
        [SerializeField]
        private SpellEditorCategory spellEditorCategory;
        
        [SerializeField]
        public static Dictionary<string, Spell> spellsToExport = new Dictionary<string, Spell>();
        [SerializeField]
        public static List<Spell> spells = new List<Spell>();
        [SerializeField]
        public static List<string> spellStringList = new List<string>();
        
        [SerializeField]
        public static List<SpellComponent> spellComponents = new List<SpellComponent>();
        [SerializeField]
        public static List<string> spellComponentStringList = new List<string>();

        [SerializeField]
        private static string fileNameSpell;
        [SerializeField]
        private static int selectedSpellIndex;

        public void DisplayHeaderContent()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Paramètre du sort", GUILayout.Height(50)))
            {
                CreateSpellComponentList();
                spellEditorCategory = SpellEditorCategory.EDIT_SPELL;
            }

            if (GUILayout.Button("Paramètre des composants du sort", GUILayout.Height(50)))
            {
                CreateSpellComponentList();
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
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (spellEditorCategory == SpellEditorCategory.EDIT_SPELL)
            {
                EditorGUI.BeginChangeCheck();
                selectedSpellIndex = currentSpellEdited != null ? spellStringList.IndexOf(currentSpellEdited.nameSpell) : -1;
                selectedSpellIndex = EditorGUILayout.Popup("Editer un spell existant", selectedSpellIndex == -1 ? 0 : selectedSpellIndex, spellStringList.ToArray());

                if (EditorGUI.EndChangeCheck())
                {
                    if (selectedSpellIndex == 0)
                    {
                        fileNameSpell = "";
                        currentSpellEdited = null;
                    }
                    else
                    {
                        currentSpellEdited = spells[selectedSpellIndex - 1];

                        if (spellsToExport.ContainsValue(currentSpellEdited))
                        {
                            fileNameSpell = spellsToExport.First(spell => spell.Value == currentSpellEdited).Key;
                        }
                    }
                }
            }
            else if (spellEditorCategory == SpellEditorCategory.EDIT_SPELL_COMPONENT)
            {
                EditorGUI.BeginChangeCheck();
                int selectedSpellComponentIndex = currentSpellComponentEdited != null ? spellComponentStringList.IndexOf(currentSpellComponentEdited.nameSpellComponent) : -1;
                selectedSpellComponentIndex = EditorGUILayout.Popup("Editer un spellComponent existant", selectedSpellComponentIndex == -1 ? 0 : selectedSpellComponentIndex, spellComponentStringList.ToArray());

                if (EditorGUI.EndChangeCheck())
                {
                    if (selectedSpellComponentIndex == 0)
                    {
                        currentSpellComponentEdited = null;
                    }
                    else
                    {
                        currentSpellComponentEdited = spellComponents[selectedSpellComponentIndex - 1];
                    }
                }
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }

        public static void CreateSpellComponentList()
        {
            spellComponentStringList.Clear();
            spellComponentStringList.Add("Nothing");

            spellComponents.ForEach(spellCompo =>
            {
                spellComponentStringList.Add(spellCompo.nameSpellComponent);
            });
            
            spellStringList.Clear();
            spellStringList.Add("Nothing");
            spells.ForEach(spell =>
            {
                spellStringList.Add(spell.nameSpell);
            });
        }

        public void DisplayBodyContent()
        {
            GUILayout.FlexibleSpace();

            switch (spellEditorCategory)
            {
                case SpellEditorCategory.EDIT_SPELL:
                    DisplaySpellEditor();
                    break;
                case SpellEditorCategory.EDIT_SPELL_COMPONENT:
                    if (currentSpellComponentEdited == null)
                    {
                        DisplayChosenSpellComponentType();
                        break;
                    }

                    SpellComponentEditor.DisplaySpellComponentEditor(currentSpellComponentEdited);
                    if (GUILayout.Button("Reset spellComponent", GUILayout.Height(50)))
                    {
                        currentSpellComponentEdited = null;
                    }
                    break;
                case SpellEditorCategory.EDIT_TRAJECTORY:
                    SpellTrajectoryEditor.DisplaySpellComponentTrajectory(currentSpellComponentEdited);
                    break;
                case SpellEditorCategory.EDIT_SPELL_INSTANCIATION:
                    SpellObjectEditor.DisplaySpellComponentObject(currentSpellComponentEdited);
                    break;
            }
            
            GUILayout.FlexibleSpace();
        }

        private void DisplayChosenSpellComponentType()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Classic", GUILayout.Height(50)))
            {
                currentSpellComponentEdited = new SpellComponent();
            }
            if (GUILayout.Button("Movement", GUILayout.Height(50)))
            {
                currentSpellComponentEdited = new MovementSpell();
            }
            if (GUILayout.Button("Transformation", GUILayout.Height(50)))
            {
                currentSpellComponentEdited = new TransformationSpell();
            }
            if (GUILayout.Button("Passive", GUILayout.Height(50)))
            {
                currentSpellComponentEdited = new PassiveSpell();
            }
            if (GUILayout.Button("Basic attack", GUILayout.Height(50)))
            {
                currentSpellComponentEdited = new BasicAttackSpell();
            }
            if (GUILayout.Button("Summon", GUILayout.Height(50)))
            {
                currentSpellComponentEdited = new SummonSpell();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
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

            fileNameSpell = EditorGUILayout.TextField("Nom du fichier", fileNameSpell);
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

            if (GUILayout.Button("Sauvegarder le spell"))
            {
                SaveSpell();
            }
            GUILayout.FlexibleSpace();
        }

        public static void SaveSpellComponent()
        {
            Debug.Log("Le spellComponent a été sauvegardé");
            spellComponents.Add(currentSpellComponentEdited);
            currentSpellComponentEdited = null;
        }

        public static void SaveSpell()
        {
            if (currentSpellEdited.activeSpellComponent == null &&
                currentSpellEdited.passiveSpellComponent == null &&
                currentSpellEdited.recastSpellComponent == null &&
                currentSpellEdited.duringCastSpellComponent == null)
            {
                Debug.LogError("Le spell n'a pas d'effet au lancement");
                return;
            }

            if (string.IsNullOrEmpty(currentSpellEdited.nameSpell))
            {
                Debug.LogError("Le spell n'a pas de nom");
                return;
            }

            if (!fileNameSpell.IsNullOrEmpty())
            {
                if (spellsToExport.ContainsKey(fileNameSpell) && selectedSpellIndex == 0)
                {
                    Debug.LogError("Le nom du fichier existe déjà");
                    return;
                }
                else 
                {
                    if (selectedSpellIndex != 0)
                    {
                        Debug.Log("Ecrasement des données déjà existantes");
                    }
                    
                    spellsToExport.Add(fileNameSpell, currentSpellEdited);
                }
            }
            
            Debug.Log("Le spell a été sauvegardé");
            spells.Add(currentSpellEdited);
            currentSpellEdited = null;
            fileNameSpell = "";
        }

        public void DisplayFooterContent()
        {
            if (GUILayout.Button("Exporter les spells enregistré", GUILayout.Height(25)))
            {
                foreach (KeyValuePair<string, Spell> spellData in spellsToExport)
                {
                    fsSerializer serializer = new fsSerializer();
                    serializer.TrySerialize(spellData.Value.GetType(), spellData.Value, out fsData data);
                    File.WriteAllText(Application.dataPath + "/Data/SpellsJson/" + spellData.Key + ".json", fsJsonPrinter.CompressedJson(data));
                    
                    
                }
            }
            if (GUILayout.Button("Importer et modifier un spell", GUILayout.Height(25)))
            {
                string path = null;
                path = EditorUtility.OpenFilePanel("Choose your spell", Application.dataPath + "/Data/SpellsJson/",
                    "json");
                
                if (path == null)
                {
                    return;
                }

                string jsonSpell = File.ReadAllText(path);

                Spell spell = null;
                fsSerializer serializer = new fsSerializer();
                fsData data = fsJsonParser.Parse(jsonSpell);
                serializer.TryDeserialize(data, ref spell);

                if (spell != null)
                {
                    ParseSpell(spell);
                }

                currentSpellEdited = spell;

                string extension = ".json";
                string initialPath = Application.dataPath + "/Data/SpellsJson/";
                int indexOfInitialPath = path.IndexOf(initialPath, StringComparison.Ordinal);
                int indexOfExtension = path.IndexOf(extension, StringComparison.Ordinal);

                if (indexOfInitialPath != -1 && indexOfExtension != -1)
                {
                    fileNameSpell = path.Substring(indexOfInitialPath + initialPath.Length, indexOfExtension - (indexOfInitialPath + initialPath.Length));
                    spellsToExport.Add(fileNameSpell, spell);
                }
                
                CreateSpellComponentList();
                spellEditorCategory = SpellEditorCategory.EDIT_SPELL;
            }
            if (GUILayout.Button("Clear le panel des spells (Toute données non sauvegardé sera perdu)", GUILayout.Height(25)))
            {
                spellsToExport.Clear();
                spells.Clear();
                spellComponents.Clear();

                currentSpellEdited = null;
                currentSpellComponentEdited = null;
                
                CreateSpellComponentList();
            }
        }

        public void ParseSpell(Spell spell)
        {
            if (!spells.Contains(spell))
            {
                spells.Add(spell);
            }
            else
            {
                return;
            }
            
            ParseSpellComponent(spell.activeSpellComponent);
            ParseSpellComponent(spell.duringCastSpellComponent);
            ParseSpellComponent(spell.recastSpellComponent);
            ParseSpellComponent(spell.passiveSpellComponent);
        }

        public void ParseSpellComponent(SpellComponent spellComponent)
        {
            if (spellComponent != null && !spellComponents.Contains(spellComponent))
            {
                spellComponents.Add(spellComponent);
            }
            else
            {
                return;
            }
            
            foreach (KeyValuePair<Trigger, List<ActionTriggered>> action in spellComponent.actions)
            {
                foreach (ActionTriggered actionTriggered in action.Value)
                {
                    if (actionTriggered.spellComponent != null)
                    {
                        ParseSpellComponent(actionTriggered.spellComponent);
                    }
                }
            }

            if (spellComponent.typeSpell == TypeSpell.Transformation)
            {
                TransformationSpell transformationSpell = spellComponent as TransformationSpell;
                foreach (ReplaceSpell spell in transformationSpell.newSpells)
                {
                    ParseSpell(spell.newSpell);
                }
            }

            if (spellComponent.typeSpell == TypeSpell.Passive)
            {
                PassiveSpell passiveSpell = spellComponent as PassiveSpell;
                ParseSpellComponent(passiveSpell.permanentSpellComponent);
            }
        }
    }
}