using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using SpellEditor.PanelUtils;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor.ComponentPanel
{
    public class AreaOfEffectPanel : MonoBehaviour
    {
        [SerializeField] private InputField interval;
        [SerializeField] private InputField duration;
        [SerializeField] private InputField scaleX;
        [SerializeField] private InputField scaleY;
        [SerializeField] private InputField scaleZ;
        [SerializeField] private InputField damagesOnEnemiesOnInterval;
        [SerializeField] private InputField damagesOnAlliesOnInterval;
        
        [SerializeField] private Dropdown geometry;
        [SerializeField] private Dropdown linkedSpellOnInterval;
        [SerializeField] private Dropdown effectOnHitOnStart;
        [SerializeField] private Dropdown linkedSpellOnEnd;
        
        [SerializeField] private DropdownMultiSelector effectsOnEnemiesOnInterval;
        [SerializeField] private DropdownMultiSelector effectsOnPlayerOnInterval;
        [SerializeField] private DropdownMultiSelector effectsOnAlliesOnInterval;
        [SerializeField] private DropdownMultiSelector deleteEffectsOnPlayerOnInterval;
        [SerializeField] private DropdownMultiSelector deleteEffectsOnEnemiesOnInterval;
        [SerializeField] private DropdownMultiSelector spellWithConditions;
        
        [SerializeField] private Toggle wantToFollow;
        [SerializeField] private Toggle canStopProjectile;
        [SerializeField] private Toggle randomTargetHit;
        [SerializeField] private Toggle randomPosition;
        [SerializeField] private Toggle onePlay;
        [SerializeField] private Toggle appliesPlayerOnHitEffect;

        public void InitAreaOfEffectPanel()
        {
            string[] enumNames = Enum.GetNames(typeof(Geometry));
            List<string> listNames = new List<string>(enumNames);
            geometry.ClearOptions();
            geometry.AddOptions(listNames);
            
            List<String> nameList = new List<string> {"None"};
            foreach (var spellComponent in ListCreatedElement.SpellComponents)
            {
                nameList.Add(spellComponent.Key);
            }
            linkedSpellOnInterval.ClearOptions();
            linkedSpellOnInterval.AddOptions(nameList);
            linkedSpellOnEnd.ClearOptions();
            linkedSpellOnEnd.AddOptions(nameList);
            
            nameList = new List<string> {"None"};
            foreach (var spellComponent in ListCreatedElement.Effects)
            {
                nameList.Add(spellComponent.Key);
            }
            effectOnHitOnStart.ClearOptions();
            effectOnHitOnStart.AddOptions(nameList);

            effectsOnEnemiesOnInterval.InitDropdownMultiSelect();
            effectsOnPlayerOnInterval.InitDropdownMultiSelect();
            effectsOnAlliesOnInterval.InitDropdownMultiSelect();
            deleteEffectsOnEnemiesOnInterval.InitDropdownMultiSelect();
            deleteEffectsOnPlayerOnInterval.InitDropdownMultiSelect();
            spellWithConditions.InitDropdownMultiSelect();
        }

        public void ResetCurrentPanel()
        {
            interval.text = "";
            duration.text = "";
            scaleX.text = "";
            scaleY.text = "";
            scaleZ.text = "";
            damagesOnEnemiesOnInterval.text = "";
            damagesOnAlliesOnInterval.text = "";
            
            geometry.value = 0;
            linkedSpellOnInterval.value = 0; 
            effectOnHitOnStart.value = 0;
            linkedSpellOnEnd.value = 0;
            
            wantToFollow.isOn = false;
            canStopProjectile.isOn = false;
            randomTargetHit.isOn = false;
            randomPosition.isOn = false;
            onePlay.isOn = false;
            appliesPlayerOnHitEffect.isOn = false;
        }

        public AreaOfEffectSpell SaveCurrentPanel()
        {
            if (duration.text != "" && interval.text == "" || damagesOnEnemiesOnInterval.text == "" && 
                linkedSpellOnInterval.value == 0 && linkedSpellOnEnd.value == 0 && 
                effectOnHitOnStart.value == 0 && effectsOnAlliesOnInterval.selectedIndex.Count == 0 && 
                effectsOnEnemiesOnInterval.selectedIndex.Count == 0 && effectsOnPlayerOnInterval.selectedIndex.Count == 0 && 
                deleteEffectsOnEnemiesOnInterval.selectedIndex.Count == 0 && deleteEffectsOnPlayerOnInterval.selectedIndex.Count == 0)
            {
                return null;
            }

            List<Effect> effectsOnAlliesOnIntervalList = new List<Effect>();
            List<Effect> effectsOnEnemiesOnIntervalList = new List<Effect>();
            List<Effect> effectsOnPlayerOnIntervalList = new List<Effect>();
            foreach (var effect in ListCreatedElement.Effects)
            {
                if (effectsOnAlliesOnInterval.selectedIndex.Contains(effect.Key))
                {
                    effectsOnAlliesOnIntervalList.Add(effect.Value);
                }
                if (effectsOnEnemiesOnInterval.selectedIndex.Contains(effect.Key))
                {
                    effectsOnEnemiesOnIntervalList.Add(effect.Value);
                }
                if (effectsOnPlayerOnInterval.selectedIndex.Contains(effect.Key))
                {
                    effectsOnPlayerOnIntervalList.Add(effect.Value);
                }
            }
            
            List<SpellWithCondition> spellWithConditionsList = new List<SpellWithCondition>();
            foreach (var spellWithCondition in ListCreatedElement.SpellWithCondition)
            {
                if (spellWithConditions.selectedIndex.Contains(spellWithCondition.Key))
                {
                    spellWithConditionsList.Add(spellWithCondition.Value);
                }
            }
            
            List<TypeEffect> deleteEffectsOnEnemiesOnIntervalList = new List<TypeEffect>();
            List<TypeEffect> deleteEffectsOnPlayerOnIntervalList = new List<TypeEffect>();
            
            foreach (var deleteEffect in deleteEffectsOnEnemiesOnInterval.selectedIndex)
            {
                deleteEffectsOnEnemiesOnIntervalList.Add((TypeEffect)Enum.Parse(typeof(TypeEffect),deleteEffect));
            }
            foreach (var deleteEffect in deleteEffectsOnPlayerOnInterval.selectedIndex)
            {
                deleteEffectsOnPlayerOnIntervalList.Add((TypeEffect)Enum.Parse(typeof(TypeEffect),deleteEffect));
            }
            
            AreaOfEffectSpell newAreaOfEffectSpell = new AreaOfEffectSpell();
            newAreaOfEffectSpell.duration = duration.text == "" ? 0 : float.Parse(duration.text);
            newAreaOfEffectSpell.interval = interval.text == "" ? 0 : float.Parse(interval.text);
            newAreaOfEffectSpell.scale = new Vector3(scaleX.text == "" ? 1 : float.Parse(scaleX.text),scaleY.text == "" ? 1 : float.Parse(scaleY.text),scaleX.text == "" ? 1 : float.Parse(scaleZ.text));
            newAreaOfEffectSpell.damagesOnEnemiesOnInterval = damagesOnEnemiesOnInterval.text == "" ? 0 : float.Parse(damagesOnEnemiesOnInterval.text);
            newAreaOfEffectSpell.damagesOnAlliesOnInterval = damagesOnAlliesOnInterval.text == "" ? 0 : float.Parse(damagesOnAlliesOnInterval.text);
            newAreaOfEffectSpell.geometry = (Geometry) geometry.value;
            newAreaOfEffectSpell.linkedSpellOnInterval = linkedSpellOnInterval.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellOnInterval.options[linkedSpellOnInterval.value].text] : null;
            newAreaOfEffectSpell.linkedSpellOnEnd = linkedSpellOnEnd.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellOnEnd.options[linkedSpellOnEnd.value].text] : null;
            newAreaOfEffectSpell.effectOnHitOnStart = effectOnHitOnStart.value != 0 ? ListCreatedElement.Effects[effectOnHitOnStart.options[effectOnHitOnStart.value].text] : new Effect();
            newAreaOfEffectSpell.effectsOnAlliesOnInterval = effectsOnAlliesOnIntervalList;
            newAreaOfEffectSpell.effectsOnEnemiesOnInterval = effectsOnEnemiesOnIntervalList;
            newAreaOfEffectSpell.effectsOnPlayerOnInterval = effectsOnPlayerOnIntervalList;
            newAreaOfEffectSpell.deleteEffectsOnEnemiesOnInterval = deleteEffectsOnEnemiesOnIntervalList;
            newAreaOfEffectSpell.deleteEffectsOnPlayerOnInterval = deleteEffectsOnPlayerOnIntervalList;
            newAreaOfEffectSpell.spellWithConditions = spellWithConditionsList;

            newAreaOfEffectSpell.wantToFollow = wantToFollow.isOn;
            newAreaOfEffectSpell.canStopProjectile = canStopProjectile.isOn;
            newAreaOfEffectSpell.randomTargetHit = randomTargetHit.isOn;
            newAreaOfEffectSpell.randomPosition = randomPosition.isOn;
            newAreaOfEffectSpell.onePlay = onePlay.isOn;
            newAreaOfEffectSpell.appliesPlayerOnHitEffect = appliesPlayerOnHitEffect.isOn;
            
            ResetCurrentPanel();
            return newAreaOfEffectSpell;
        }
    }
}
