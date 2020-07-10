using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FullSerializer;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpellEditor
{
public enum Panel
{
    Effect,
    Spell,
    SpellComponent,
    SpellWithCondition
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
    [SerializeField] private Button createSpellWithConditionButton;

    [SerializeField] private GameObject spellPanel;
    [SerializeField] private GameObject spellComponentPanel;
    [SerializeField] private GameObject effectPanel;
    [SerializeField] private GameObject spellWithConditionPanel;
    [SerializeField] private SpellWithConditionPanel spellWithConditionPanelScript;

    [SerializeField] private Button TestSpellButton;

    private void Start()
    {
        spellPanel.SetActive(false);
        spellComponentPanel.SetActive(false);
        effectPanel.SetActive(false);

        createSpellButton.onClick.AddListener(delegate {
            SwitchPanel(Panel.Spell);
        });
        createSpellComponentButton.onClick.AddListener(delegate {
            SwitchPanel(Panel.SpellComponent);
        });
        createSpellEffectButton.onClick.AddListener(delegate {
            SwitchPanel(Panel.Effect);
        });
        createSpellWithConditionButton.onClick.AddListener(delegate {
            SwitchPanel(Panel.SpellWithCondition);
        });

        exportSpells.onClick.AddListener(ExportSpells);
        importSpell.onClick.AddListener(ChooseSpellToImport);

        TestSpellButton.onClick.AddListener(SwitchToTestSpellScene);
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
        spellWithConditionPanel.SetActive(false);

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
        case Panel.SpellWithCondition:
            spellWithConditionPanelScript.InitSpellWithCondition();
            spellWithConditionPanel.SetActive(true);
            break;
        }
    }

    private IEnumerator FixSpell()
    {
        string path = Application.dataPath + "/Data/SpellsJson/";
        string[] files = Directory.GetFiles(path);
        int index = 0;

        while (index < files.Length)
        {
            if (files[index].EndsWith(".meta"))
            {
                index++;
                continue;
            }

            Debug.Log("Try import file " + files[index]);

            yield return new WaitForSeconds(0.5f);

            SpellFix.TryImportSpell(files[index]);
            index++;
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
        string path = null;
#if UNITY_EDITOR
        path = EditorUtility.OpenFilePanel("Choose your spell", Application.dataPath + "/Data/SpellsJson", "json");
#endif


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
                Debug.Log(spell.nameSpell + " already exist (spell)");
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
            if (spellComponent != null)
            {
                Debug.Log(spellComponent.nameSpellComponent + " already exist (spellComponent)");
            }
            return;
        }

        ListCreatedElement.SpellComponents.Add(spellComponent.nameSpellComponent, spellComponent);

        ParsePropertyInfo(spellComponent, spellComponent.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance));
    }

    private void ParseSpellWithCondition(SpellWithCondition spellWithCondition)
    {
        if (spellWithCondition == null || ListCreatedElement.SpellWithCondition.ContainsKey(spellWithCondition.nameSpellWithCondition))
        {
            if (spellWithCondition != null)
            {
                Debug.Log(spellWithCondition.nameSpellWithCondition + " already exist (Spell with condition)");
            }
            return;
        }

        ListCreatedElement.SpellWithCondition.Add(spellWithCondition.nameSpellWithCondition, spellWithCondition);

        ParsePropertyInfo(spellWithCondition, spellWithCondition.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance));
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

                    if (effectsProp == null)
                    {
                        continue;
                    }

                    foreach (Effect effect in effectsProp)
                    {
                        if (effect.nameEffect == null)
                        {
                            continue;
                        }

                        if (!ListCreatedElement.Effects.ContainsKey(effect.nameEffect))
                        {
                            ListCreatedElement.Effects.Add(effect.nameEffect, effect);
                        }
                    }
                }
                else
                {
                    Effect effectProp = (Effect) prop.GetValue(origin);
                    if (effectProp?.nameEffect == null)
                    {
                        continue;
                    }

                    if (!ListCreatedElement.Effects.ContainsKey(effectProp.nameEffect))
                    {
                        ListCreatedElement.Effects.Add(effectProp.nameEffect, effectProp);
                    }
                    else
                    {
                        Debug.Log(effectProp.nameEffect + " already exist (effect)");
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
            } else if (prop.PropertyType == typeof(SpellWithCondition) ||
                       prop.PropertyType == typeof(List<SpellWithCondition>))
            {
                if (prop.PropertyType == typeof(List<SpellWithCondition>))
                {
                    List<SpellWithCondition> spellWithConditionsProp = (List<SpellWithCondition>) prop.GetValue(origin);
                    foreach (SpellWithCondition spellWithConditionIn in spellWithConditionsProp)
                    {
                        ParseSpellWithCondition(spellWithConditionIn);
                    }
                }
                else
                {
                    SpellWithCondition spellWithConditionsProp = (SpellWithCondition) prop.GetValue(origin);
                    ParseSpellWithCondition(spellWithConditionsProp);
                }
            }
        }
    }

    public void SwitchToTestSpellScene()
    {
        SceneManager.LoadScene("TestSpell");
    }
}
}
