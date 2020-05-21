using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FullSerializer;
using Games.Global;
using Games.Global.Spells;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor
{
    public enum Panel
    {
        Effect,
        Spell,
        SpellComponent
    }
    
    public class NavBar : MonoBehaviour
    {
        [SerializeField] private SpellPanel spellPanelScript;
        [SerializeField] private EffectPanel effectPanelScript;
        [SerializeField] private SpellComponentPanel spellComponentPanelScript;
        [SerializeField] private Button exportSpells;
        [SerializeField] private Button importSpell;
        
        [SerializeField] private Button createSpellButton;
        [SerializeField] private Button createSpellComponentButton;
        [SerializeField] private Button createSpellEffectButton;

        [SerializeField] private GameObject spellPanel;
        [SerializeField] private GameObject spellComponentPanel;
        [SerializeField] private GameObject effectPanel;

        private void Start()
        {
            spellPanel.SetActive(false);
            spellComponentPanel.SetActive(false);
            effectPanel.SetActive(false);

            createSpellButton.onClick.AddListener(delegate { SwitchPanel(Panel.Spell); });
            createSpellComponentButton.onClick.AddListener(delegate { SwitchPanel(Panel.SpellComponent); });
            createSpellEffectButton.onClick.AddListener(delegate { SwitchPanel(Panel.Effect); });
            
            exportSpells.onClick.AddListener(ExportSpells);
            importSpell.onClick.AddListener(ChooseSpellToImport);
        }

        public static void ModifyExistingComponent(object component, object newComponent)
        {
            if (component.GetType() != newComponent.GetType())
            {
                return;
            }

            foreach (PropertyInfo prop in component.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance))
            {
                prop.SetValue(component, prop.GetValue(newComponent));
            }
        }

        private void SwitchPanel(Panel newPanel)
        {
            spellPanel.SetActive(false);
            spellComponentPanel.SetActive(false);
            effectPanel.SetActive(false);

            switch (newPanel)
            {
                case Panel.Effect:
                    effectPanelScript.InitEffectPanel();
                    effectPanel.SetActive(true);
                    break;
                case Panel.Spell:
                    spellPanelScript.InitSpellPanel();
                    spellPanel.SetActive(true);
                    break;
                case Panel.SpellComponent:
                    spellComponentPanelScript.InitSpellComponentPanel();
                    spellComponentPanel.SetActive(true);
                    break;
            }
        }

        private void ExportSpells()
        {
            foreach (KeyValuePair<string, Spell> valuePair in ListCreatedElement.Spell)
            {
                fsSerializer serializer = new fsSerializer();
                serializer.TrySerialize(valuePair.Value.GetType(), valuePair.Value, out fsData data);
                File.WriteAllText(Application.dataPath + "/Data/SpellsJson/" + valuePair.Key + ".json", fsJsonPrinter.CompressedJson(data));
            }
        }

        private void ChooseSpellToImport()
        {
            string path = EditorUtility.OpenFilePanel("Choose your spell", Application.dataPath + "/Data/SpellsJson", "json");

            if (path == null)
            {
                return;
            }

            string jsonSpell = File.ReadAllText(path);

            ImportSpell(jsonSpell);
        }

        private void ImportSpell(string jsonSpell)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data = fsJsonParser.Parse(jsonSpell);

            Spell spell = null;
            serializer.TryDeserialize(data, ref spell);
            
            ParseSpell(spell);
        }

        private void ParseSpell(Spell spell)
        {
            if (spell == null || ListCreatedElement.Spell.ContainsKey(spell.nameSpell))
            {
                if (spell != null)
                {
                    Debug.Log(spell.nameSpell + " already exist");
                }
                return;
            }

            ListCreatedElement.Spell.Add(spell.nameSpell, spell);
            
            ParsePropertyInfo(spell, spell.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance));
        }

        private void ParseSpellComponent(SpellComponent spellComponent)
        {
            if (spellComponent == null || ListCreatedElement.SpellComponents.ContainsKey(spellComponent.nameSpellComponent))
            {
                return;
            }

            ListCreatedElement.SpellComponents.Add(spellComponent.nameSpellComponent, spellComponent);

            ParsePropertyInfo(spellComponent, spellComponent.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance));
        }

        private void ParsePropertyInfo(object origin, PropertyInfo[] propertyInfos)
        {
            foreach (PropertyInfo prop in propertyInfos)
            {
                if (prop.PropertyType == typeof(Effect) || prop.PropertyType == typeof(List<Effect>))
                {
                    if (prop.PropertyType == typeof(List<Effect>))
                    {
                        List<Effect> effectsProp = (List<Effect>) prop.GetValue(origin);
                        foreach (Effect effect in effectsProp)
                        {
                            if (!ListCreatedElement.Effects.ContainsKey(effect.nameEffect))
                            {
                                ListCreatedElement.Effects.Add(effect.nameEffect, effect);
                            }
                        }
                    }
                    else
                    {
                        Effect effectProp = (Effect) prop.GetValue(origin);

                        if (!ListCreatedElement.Effects.ContainsKey(effectProp.nameEffect))
                        {
                            ListCreatedElement.Effects.Add(effectProp.nameEffect, effectProp);
                        }
                    }
                } else if (prop.PropertyType == typeof(Spell) || prop.PropertyType == typeof(List<Spell>))
                {
                    if (prop.PropertyType == typeof(List<Spell>))
                    {
                        List<Spell> spellsProp = (List<Spell>) prop.GetValue(origin);
                        foreach (Spell spellIn in spellsProp)
                        {
                            ParseSpell(spellIn);
                        }
                    }
                    else
                    {
                        Spell spellProp = (Spell) prop.GetValue(origin);
                        ParseSpell(spellProp);
                    }
                } else if (prop.PropertyType == typeof(SpellComponent) || prop.PropertyType == typeof(List<SpellComponent>))
                {
                    if (prop.PropertyType == typeof(List<SpellComponent>))
                    {
                        List<SpellComponent> spellComponentsProp = (List<SpellComponent>) prop.GetValue(origin);
                        foreach (SpellComponent spellComponentIn in spellComponentsProp)
                        {
                            ParseSpellComponent(spellComponentIn);
                        }
                    }
                    else
                    {
                        SpellComponent spellComponentProp = (SpellComponent) prop.GetValue(origin);
                        ParseSpellComponent(spellComponentProp);
                    }
                }
            }
        }
    }
}
