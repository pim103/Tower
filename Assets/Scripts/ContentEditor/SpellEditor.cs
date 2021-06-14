#if UNITY_EDITOR || UNITY_EDITOR_64

using System;
using System.Collections.Generic;
using System.IO;
using ContentEditor.SpellEditorComposant;
using ContentEditor.UtilsEditor;
using FullSerializer;
using Games.Global;
using Games.Global.Spells;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

namespace ContentEditor
{
    public enum SpellEditorCategory
    {
        EDIT_SPELL,
        EDIT_SPELL_COMPONENT,
        EDIT_SPELL_INSTANCIATION
    }

    [Serializable]
    public class SpellEditor : IEditorInterface
    {
        [SerializeField]
        private static Spell currentSpellEdited;
        [SerializeField]
        private static SpellComponent currentSpellComponentEdited;

        private static SpellEditor instance;

        private SpellEditorCategory spellEditorCategory;
        private TypeSpell typeSpellSelected;

        public static Dictionary<string, Spell> spellsToExport = new Dictionary<string, Spell>();

        [SerializeField] 
        private List<Spell> spells { get; set; }= new List<Spell>();

        public static List<SpellComponent> spellComponents = new List<SpellComponent>();
        public static Dictionary<string, SpellComponent> spellComponentsInEditor = new Dictionary<string, SpellComponent>();

        [SerializeField]
        private static string fileNameSpell;

        private static bool editedSpell;

        private CreateOrSelectComponent<SpellComponent> firstCreateOrSelectComponentSelect;
        private CreateOrSelectComponent<SpellComponent> afterHoldingCreateOrSelectComponentSelect;
        private CreateOrSelectComponent<SpellComponent> passiveCreateOrSelectComponentSelect;

        public void InitSpellEditor()
        {
        }

        public static List<Spell> GetSpells()
        {
            return instance.spells;
        }

        public void DisplayHeaderContent()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            instance ??= this;
            
            if (GUILayout.Button("Paramètre du sort", GUILayout.Height(50)))
            {
                spellEditorCategory = SpellEditorCategory.EDIT_SPELL;
            }
            
            if (GUILayout.Button("Paramètre des composants du sort", GUILayout.Height(50)))
            {
                StartSpellComponentEditor();
            }

            if (currentSpellComponentEdited != null && GUILayout.Button("Paramètre d'objet en jeu", GUILayout.Height(50)))
            {
                spellEditorCategory = SpellEditorCategory.EDIT_SPELL_INSTANCIATION;
                SpellObjectEditor.OpenAndInitScene(currentSpellComponentEdited);
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }

        public void StartSpellComponentEditor()
        {
            spellEditorCategory = SpellEditorCategory.EDIT_SPELL_COMPONENT;
            SpellComponentEditor.InitSpellComponentEditor();
        }

        public void DisplayBodyContent()
        {
            GUILayout.FlexibleSpace();

            switch (spellEditorCategory)
            {
                case SpellEditorCategory.EDIT_SPELL:
                    if (currentSpellEdited == null)
                    {
                        DisplaySpellInit();
                        break;
                    }
                    
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
                case SpellEditorCategory.EDIT_SPELL_INSTANCIATION:
                    SpellObjectEditor.DisplaySpellComponentObject(currentSpellEdited, currentSpellComponentEdited);
                    break;
            }
            
            GUILayout.FlexibleSpace();
        }
        
        private void DisplaySpellInit()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            foreach (TypeSpell typeSpell in Enum.GetValues(typeof(TypeSpell)))
            {
                if (GUILayout.Button(typeSpell.ToString(), GUILayout.Height(50)))
                {
                    typeSpellSelected = typeSpell;
                    currentSpellEdited = new Spell {TypeSpell = typeSpellSelected};

                    InitTypeSpellParameters();
                    editedSpell = false;
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            if (DataObject.SpellList != null)
            {
                foreach (SpellList.SpellInfo spellInfo in DataObject.SpellList.SpellInfos)
                {
                    Spell spell = spellInfo.spell;

                    if (GUILayout.Button(spell.nameSpell, GUILayout.Height(50)))
                    {
                        currentSpellEdited = spell;
                        typeSpellSelected = spell.TypeSpell;
                        InitTypeSpellParameters();
                        editedSpell = true;

                        fileNameSpell = spellInfo.filename;
                        ParseSpell(currentSpellEdited);
                    }
                }
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void OnCreate(CreateOrSelectComponent<SpellComponent> createOrSelectComponent)
        {
            StartSpellComponentEditor();
            createOrSelectComponent.ResetChoice();
        }

        private void InitTypeSpellParameters()
        {
            firstCreateOrSelectComponentSelect = new CreateOrSelectComponent<SpellComponent>(spellComponents, currentSpellEdited.spellComponentFirstActivation, "First component", OnCreate);

            switch (typeSpellSelected)
            {
                case TypeSpell.Holding:
                    afterHoldingCreateOrSelectComponentSelect = new CreateOrSelectComponent<SpellComponent>(spellComponents, currentSpellEdited.spellComponentAfterHolding, "After holding component", OnCreate);
                    break;
                case TypeSpell.Passive:
                    passiveCreateOrSelectComponentSelect = new CreateOrSelectComponent<SpellComponent>(spellComponents, currentSpellEdited.passiveSpellComponent, "Passive", OnCreate);
                    break;
                case TypeSpell.ActivePassive:
                    passiveCreateOrSelectComponentSelect = new CreateOrSelectComponent<SpellComponent>(spellComponents, currentSpellEdited.passiveSpellComponent, "Passive", OnCreate);
                    break;
                case TypeSpell.MultipleActivation:
                    currentSpellEdited.spellComponents ??= new List<SpellComponentWithTimeCast>();
                    multipleComponentChooseOrSelects = new Dictionary<SpellComponentWithTimeCast, CreateOrSelectComponent<SpellComponent>>();

                    int idx = 0;
                    currentSpellEdited.spellComponents?.ForEach(action => multipleComponentChooseOrSelects.Add(action,
                        new CreateOrSelectComponent<SpellComponent>(spellComponents, action.spellComponent, "Component " + ++idx, OnCreate)));
                    break;
                case TypeSpell.UniqueActivation:
                    break;
                case TypeSpell.HoldThenRelease:
                    afterHoldingCreateOrSelectComponentSelect = new CreateOrSelectComponent<SpellComponent>(spellComponents, currentSpellEdited.spellComponentAfterHolding, "After holding component", OnCreate);
                    break;
            }
        }

        private Dictionary<SpellComponentWithTimeCast, CreateOrSelectComponent<SpellComponent>> multipleComponentChooseOrSelects;
        private void DisplaySpellEditor()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Button(currentSpellEdited.sprite, GUILayout.Width(75), GUILayout.Height(75));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            fileNameSpell = EditorGUILayout.TextField("Nom du fichier", fileNameSpell);
            currentSpellEdited.nameSpell = EditorGUILayout.TextField("Name", currentSpellEdited.nameSpell);
            currentSpellEdited.spellTag = (SpellTag) EditorGUILayout.EnumPopup("Tag", currentSpellEdited.spellTag);
            currentSpellEdited.startFrom = (StartFrom) EditorGUILayout.EnumPopup("Le spell part de ", currentSpellEdited.startFrom);
            currentSpellEdited.cooldown = EditorGUILayout.FloatField("Cooldown", currentSpellEdited.cooldown);
            currentSpellEdited.cost = EditorGUILayout.FloatField("Cout en ressource", currentSpellEdited.cost);
            currentSpellEdited.castTime = EditorGUILayout.FloatField("Temps de cast", currentSpellEdited.castTime);
            currentSpellEdited.nbUse = EditorGUILayout.IntField("Nombre d'utilisation", currentSpellEdited.nbUse);

            EditorGUILayout.LabelField("Sprite");    
            currentSpellEdited.sprite = (Texture2D)EditorGUILayout.ObjectField(currentSpellEdited.sprite, typeof(Texture2D), false);

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            Color defaultColor = GUI.color;
            GUI.color = Color.blue;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;
            
            EditorGUILayout.LabelField("Spécifique à " + typeSpellSelected);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            switch (typeSpellSelected)
            {
                case TypeSpell.Holding:
                    DisplayHoldingOptions();
                    break;
                case TypeSpell.Passive:
                    DisplayPassiveOptions();
                    break;
                case TypeSpell.ActivePassive:
                    DisplayActivePassiveOptions();
                    break;
                case TypeSpell.MultipleActivation:
                    DisplayMultipleActivationOptions();
                    break;
                case TypeSpell.UniqueActivation:
                    DisplayUniqueActivationOptions();
                    break;
                case TypeSpell.HoldThenRelease:
                    DisplayHoldThenReleaseOptions();
                    break;
            }

            EditorGUILayout.EndVertical();
            if (GUILayout.Button("Sauvegarder le spell"))
            {
                SaveSpell();
            }
            GUILayout.FlexibleSpace();
        }

        public void DisplayHoldingOptions()
        {
            currentSpellEdited.spellComponentFirstActivation = firstCreateOrSelectComponentSelect.DisplayOptions();
            currentSpellEdited.spellComponentAfterHolding = afterHoldingCreateOrSelectComponentSelect.DisplayOptions();

            currentSpellEdited.holdingCost = EditorGUILayout.FloatField("Holding cost", currentSpellEdited.holdingCost);
            currentSpellEdited.maximumTimeHolding = EditorGUILayout.FloatField("Maximum time hold", currentSpellEdited.maximumTimeHolding);
        }
        
        public void DisplayPassiveOptions()
        {
            currentSpellEdited.passiveSpellComponent = passiveCreateOrSelectComponentSelect.DisplayOptions();
        }

        public void DisplayActivePassiveOptions()
        {
            currentSpellEdited.spellComponentFirstActivation = firstCreateOrSelectComponentSelect.DisplayOptions();
            currentSpellEdited.passiveSpellComponent = passiveCreateOrSelectComponentSelect.DisplayOptions();
            currentSpellEdited.deactivatePassiveWhenActive =
                EditorGUILayout.Toggle("Desactive le passive à l'activation",
                    currentSpellEdited.deactivatePassiveWhenActive);
        }

        public void DisplayMultipleActivationOptions()
        {
            currentSpellEdited.spellComponentFirstActivation = firstCreateOrSelectComponentSelect.DisplayOptions();

            if (GUILayout.Button("Add component"))
            {
                SpellComponentWithTimeCast spellComponentWithTimeCast = new SpellComponentWithTimeCast();
                multipleComponentChooseOrSelects.Add(spellComponentWithTimeCast, 
                    new CreateOrSelectComponent<SpellComponent>(spellComponents, null, "Component " + currentSpellEdited.spellComponents.Count, OnCreate));
                currentSpellEdited.spellComponents.Add(spellComponentWithTimeCast);
            }

            EditorGUILayout.BeginHorizontal();

            foreach (SpellComponentWithTimeCast spellComponentWithTimeCast in currentSpellEdited.spellComponents)
            {
                spellComponentWithTimeCast.timeBeforeReset =
                    EditorGUILayout.FloatField("Time before reset",
                        spellComponentWithTimeCast.timeBeforeReset);
                spellComponentWithTimeCast.spellComponent = multipleComponentChooseOrSelects[spellComponentWithTimeCast].DisplayOptions();
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        public void DisplayUniqueActivationOptions()
        {
            currentSpellEdited.spellComponentFirstActivation = firstCreateOrSelectComponentSelect.DisplayOptions();
        }

        public void DisplayHoldThenReleaseOptions()
        {
            currentSpellEdited.spellComponentFirstActivation = firstCreateOrSelectComponentSelect.DisplayOptions();
            currentSpellEdited.spellComponentAfterHolding = afterHoldingCreateOrSelectComponentSelect.DisplayOptions();

            currentSpellEdited.holdingCost = EditorGUILayout.FloatField("Holding cost", currentSpellEdited.holdingCost);
            currentSpellEdited.maximumTimeHolding = EditorGUILayout.FloatField("Maximum time hold", currentSpellEdited.maximumTimeHolding);
        }
        
        private void DisplayChosenSpellComponentType()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
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
            
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            foreach (SpellComponent spellComponent in spellComponents)
            {
                if (GUILayout.Button(spellComponent.nameSpellComponent, GUILayout.Height(50)))
                {
                    currentSpellComponentEdited = spellComponent;
                }
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        public static void SaveSpellComponent()
        {
            Debug.Log("Le spellComponent a été sauvegardé");

            if (!spellComponentsInEditor.ContainsKey(currentSpellComponentEdited.nameSpellComponent))
            {
                spellComponents.Add(currentSpellComponentEdited);
            }
            
            currentSpellComponentEdited = null;
        }

        public static void SaveSpell()
        {
            if (string.IsNullOrEmpty(currentSpellEdited.nameSpell))
            {
                Debug.LogError("Le spell n'a pas de nom");
                return;
            }

            if (!fileNameSpell.IsNullOrEmpty())
            {
                if (spellsToExport.ContainsKey(fileNameSpell) && !editedSpell)
                {
                    Debug.LogError("Le nom du fichier existe déjà");
                    return;
                }

                Debug.Log("Le spell a été sauvegardé");
                GetSpells().Add(currentSpellEdited);
                spellsToExport.Add(fileNameSpell, currentSpellEdited);
            }
            
            currentSpellEdited = null;
            fileNameSpell = "";
        }

        public void DisplayFooterContent()
        {
            if (GUILayout.Button("Exporter les spells enregistré", GUILayout.Height(25)))
            {
                PrepareSaveRequest.SaveSpell(spellsToExport);
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

                string extension = ".json";
                string initialPath = Application.dataPath + "/Data/SpellsJson/";
                int indexOfInitialPath = path.IndexOf(initialPath, StringComparison.Ordinal);
                int indexOfExtension = path.IndexOf(extension, StringComparison.Ordinal);

                if (indexOfInitialPath != -1 && indexOfExtension != -1)
                {
                    fileNameSpell = path.Substring(indexOfInitialPath + initialPath.Length, indexOfExtension - (indexOfInitialPath + initialPath.Length));
                    spellsToExport.Add(fileNameSpell, spell);
                }

                spellEditorCategory = SpellEditorCategory.EDIT_SPELL;
            }
            if (GUILayout.Button("Clear le panel des spells (Toute données non sauvegardé sera perdu)", GUILayout.Height(25)))
            {
                spellsToExport.Clear();
                spells.Clear();
                spellComponents.Clear();
                spellComponentsInEditor.Clear();

                currentSpellEdited = null;
                currentSpellComponentEdited = null;
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

            ParseSpellComponent(spell.spellComponentFirstActivation);
            ParseSpellComponent(spell.spellComponentAfterHolding);
            ParseSpellComponent(spell.passiveSpellComponent);

            spell.spellComponents?.ForEach(cast => ParseSpellComponent(cast.spellComponent));
        }

        public void ParseSpellComponent(SpellComponent spellComponent)
        {
            if (spellComponent != null && !spellComponentsInEditor.ContainsKey(spellComponent.nameSpellComponent))
            {
                spellComponents.Add(spellComponent);
                spellComponentsInEditor.Add(spellComponent.nameSpellComponent, spellComponent);
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

            if (spellComponent.TypeSpellComponent == TypeSpellComponent.Transformation)
            {
                TransformationSpell transformationSpell = spellComponent as TransformationSpell;
                foreach (ReplaceSpell spell in transformationSpell.newSpells)
                {
                    ParseSpell(spell.newSpell);
                }

                ParseSpell(transformationSpell.newBasicAttack);
                ParseSpell(transformationSpell.newBasicDefense);
            }

            if (spellComponent.TypeSpellComponent == TypeSpellComponent.Passive)
            {
                PassiveSpell passiveSpell = spellComponent as PassiveSpell;
                ParseSpellComponent(passiveSpell.permanentSpellComponent);
            }
        }
    }
}

#endif