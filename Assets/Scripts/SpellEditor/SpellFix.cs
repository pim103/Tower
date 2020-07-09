using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FullSerializer;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using UnityEngine;

namespace SpellEditor
{
    public class SpellFix
    {
        public static void TryImportSpell(string file)
        {
            string jsonSpell = File.ReadAllText(file);

            ImportSpell(file, jsonSpell);
        }

        public static void ImportSpell(string file, string jsonSpell)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;
            Spell spell = null;

            try
            {
                data = fsJsonParser.Parse(jsonSpell);
            }
            catch (Exception e)
            {
                Debug.Log("Error when parse jsonSpell for path " + file);
                Debug.Log(e.Message);
                return;
            }

            if (data == null)
            {
                Debug.Log("Data is null for " + file);
                return;
            }
            
            try
            {
                serializer.TryDeserialize(data, ref spell);
            }
            catch (Exception e)
            {
                Debug.Log("Error when deserialize jsonSpell for path " + file);
                Debug.Log(e.Message);
                return;
            }

            ParseSpell(spell);
            
            Debug.Log("Save " + spell.nameSpell);
            serializer.TrySerialize(spell.GetType(), spell, out data);
            File.WriteAllText(Application.dataPath + "/Data/SpellsJson/" + spell.nameSpell + ".json", fsJsonPrinter.CompressedJson(data));
        }

        private static void ParseSpell(Spell spell)
        {
            if (spell == null)
            {
                return;
            }
            
            ParsePropertyInfoForFix(spell, spell.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance));
        }

        private static void ParseSpellComponent(SpellComponent spellComponent)
        {
            if (spellComponent == null)
            {
                return;
            }

            spellComponent.currentCoroutine = null;
            ParseSpecificSpellComponent(spellComponent);
            ParsePropertyInfoForFix(spellComponent, spellComponent.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance));
        }

        private static void ParseSpellWithCondition(SpellWithCondition spellWithCondition)
        {
            if (spellWithCondition == null)
            {
                return;
            }
            
            if (spellWithCondition.effect != null && string.IsNullOrEmpty(spellWithCondition.effect.nameEffect))
            {
                spellWithCondition.effect = null;
            }
            
            if (spellWithCondition.conditionEffect != null && string.IsNullOrEmpty(spellWithCondition.conditionEffect.nameEffect))
            {
                spellWithCondition.conditionEffect = null;
            }
            
            ParsePropertyInfoForFix(spellWithCondition, spellWithCondition.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance));
        }

        private static void ParsePropertyInfoForFix(object origin, PropertyInfo[] propertyInfos)
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
                            effect.launcher = null;
                        }
                    }
                    else
                    {
                        Effect effectProp = (Effect) prop.GetValue(origin);
                        if (effectProp?.nameEffect == null)
                        {
                            continue;
                        }

                        effectProp.launcher = null;
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

        private static void ParseSpecificSpellComponent(SpellComponent spellComponent)
        {
            if (spellComponent == null)
            {
                return;
            }

            switch (spellComponent.typeSpell)
            {
                case TypeSpell.Buff:
                    BuffSpell origb = spellComponent as BuffSpell;
                    if (origb.effectOnSelfWhenNoCharge != null && string.IsNullOrEmpty(origb.effectOnSelfWhenNoCharge.nameEffect))
                    {
                        origb.effectOnSelfWhenNoCharge = null;
                    }
                    break;
                case TypeSpell.Projectile:
                    ProjectileSpell origp = spellComponent as ProjectileSpell;
                    origp.objectPooled = null;
                    origp.prefabPooled = null;
                    break;
                case TypeSpell.Summon:
                    SummonSpell origs = spellComponent as SummonSpell;
                    origs.prefabsSummon = null;
                    break;
                case TypeSpell.Wave:
                    WaveSpell origw = spellComponent as WaveSpell;
                    origw.objectPooled = null;
                    break;
                case TypeSpell.AreaOfEffect:
                    AreaOfEffectSpell origaoe = spellComponent as AreaOfEffectSpell;
                    origaoe.objectPooled = null;
                    
                    if (origaoe.effectOnHitOnStart != null && string.IsNullOrEmpty(origaoe.effectOnHitOnStart.nameEffect))
                    {
                        origaoe.effectOnHitOnStart = null;
                    }
                    break;
            }
        }
    }
}