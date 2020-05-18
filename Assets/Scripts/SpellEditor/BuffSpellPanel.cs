using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global;
using Games.Global.Spells;
using SpellEditor.PanelUtils;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor
{
    public class BuffSpellPanel : MonoBehaviour
    {
        [SerializeField] private InputField interval;
        [SerializeField] private InputField duration;
        [SerializeField] private InputField stack;
        [SerializeField] private InputField damageOnSelf;
        
        [SerializeField] private Toggle disappearOnDamageReceived;
        [SerializeField] private Toggle needNewPositionOnDamageReceived;
        [SerializeField] private Toggle needNewPositionOnHit;
        [SerializeField] private Toggle needNewPositionOnAttack;
        [SerializeField] private Toggle needNewPositionOnInterval;
        [SerializeField] private Toggle triggerInvocationCallOneTime;
        
        [SerializeField] private Dropdown replaceProjectile;
        [SerializeField] private Dropdown linkedSpellOnDamageReceived;
        [SerializeField] private Dropdown linkedSpellOnHit;
        [SerializeField] private Dropdown linkedSpellOnAttack;
        [SerializeField] private Dropdown linkedSpellOnInterval;
        [SerializeField] private Dropdown conditionReduceCharge;
        [SerializeField] private Dropdown effectOnSelfWhenNoCharge;
        [SerializeField] private Dropdown behaviorType;
        [SerializeField] private DropdownMultiSelector effectsOnSelf;
        [SerializeField] private DropdownMultiSelector effectsOnDamageReceived;
        [SerializeField] private DropdownMultiSelector effectOnTargetOnHit;

        private List<Effect> effectsOnSelfList;
        private List<Effect> effectsOnSelfOnDamageReceived;
        private List<Effect> effectsOnTargetOnHit;

        public void InitBuffPanel()
        {
            effectsOnSelfList = new List<Effect>();
            effectsOnSelfOnDamageReceived = new List<Effect>();
            effectsOnTargetOnHit = new List<Effect>();
            List<String> nameList = new List<string> {"None"};
            foreach (var spellComponent in ListCreatedElement.SpellComponents)
            {
                nameList.Add(spellComponent.Key);
            }
            linkedSpellOnDamageReceived.ClearOptions();
            linkedSpellOnDamageReceived.AddOptions(nameList);
            linkedSpellOnHit.ClearOptions();
            linkedSpellOnHit.AddOptions(nameList);
            linkedSpellOnAttack.ClearOptions();
            linkedSpellOnAttack.AddOptions(nameList);
            linkedSpellOnInterval.ClearOptions();
            linkedSpellOnInterval.AddOptions(nameList);
            
            string[] enumNames = Enum.GetNames(typeof(ConditionReduceCharge));
            List<string> listNames = new List<string>(enumNames);
            conditionReduceCharge.ClearOptions();
            conditionReduceCharge.AddOptions(listNames);

            nameList = new List<string> {"None"};
            foreach (var effect in ListCreatedElement.Effects)
            {
                nameList.Add(effect.Key);
            }

            List<string> projectileSpells = new List<string> {"None"};
            projectileSpells = ListCreatedElement.SpellComponents.Select(pair =>
            {
                if (pair.Value.typeSpell == TypeSpell.Projectile)
                {
                    return pair.Key;
                }

                return null;
            }).ToList();
            replaceProjectile.AddOptions(projectileSpells);
            
            effectOnSelfWhenNoCharge.ClearOptions();
            effectOnSelfWhenNoCharge.AddOptions(nameList);
            effectsOnSelf.InitDropdownMultiSelect();
            effectsOnDamageReceived.InitDropdownMultiSelect();
            effectOnTargetOnHit.InitDropdownMultiSelect();
            
            enumNames = Enum.GetNames(typeof(BehaviorType));
            listNames = new List<string>(enumNames);
            behaviorType.ClearOptions();
            behaviorType.AddOptions(listNames);
        }

        private void ResetCurrentPanel()
        {
            interval.text = "";
            duration.text = "";
            stack.text = "";
            damageOnSelf.text = "";

            disappearOnDamageReceived.isOn = false;
            needNewPositionOnDamageReceived.isOn = false;
            needNewPositionOnHit.isOn = false;
            needNewPositionOnAttack.isOn = false;
            needNewPositionOnInterval.isOn = false;
            triggerInvocationCallOneTime.isOn = false;

            linkedSpellOnDamageReceived.value = 0;
            linkedSpellOnHit.value = 0;
            linkedSpellOnAttack.value = 0;
            linkedSpellOnInterval.value = 0;
            replaceProjectile.value = 0;
            conditionReduceCharge.value = 0;
            effectOnSelfWhenNoCharge.value = 0;
            behaviorType.value = 0;
        }

        public BuffSpell SaveCurrentPanel()
        {
            BuffSpell newBuffSpell = new BuffSpell();
            
            foreach (var effect in ListCreatedElement.Effects)
            {
                if (effectsOnSelf.selectedIndex.Contains(effect.Key))
                {
                    effectsOnSelfList.Add(effect.Value);
                }
                if (effectsOnDamageReceived.selectedIndex.Contains(effect.Key))
                {
                    effectsOnSelfOnDamageReceived.Add(effect.Value);
                }
                if (effectOnTargetOnHit.selectedIndex.Contains(effect.Key))
                {
                    effectsOnTargetOnHit.Add(effect.Value);
                }
            }
            //needs spellwithcondition
            if ((duration.text != "" && interval.text == "") || (duration.text == "" && stack.text == "") /*|| (linkedSpellOnDamageReceived.value == 0 && linkedSpellOnAttack.value == 0 && linkedSpellOnHit.value == 0 && linkedSpellOnInterval.value == 0)*/)
            {
                return null;
            }
            newBuffSpell.duration = duration.text == "" ? 0 : Int32.Parse(duration.text);

            newBuffSpell.interval = interval.text == "" ? 0 : Int32.Parse(interval.text);
            newBuffSpell.stack = stack.text == "" ? 0 : Int32.Parse(stack.text);
            
            newBuffSpell.damageOnSelf = damageOnSelf.text == "" ? 0 : Int32.Parse(damageOnSelf.text);
        
            newBuffSpell.disapearOnDamageReceived = disappearOnDamageReceived.isOn;
            newBuffSpell.needNewPositionOnDamageReceived = needNewPositionOnDamageReceived.isOn;
            newBuffSpell.needNewPositionOnHit = needNewPositionOnHit.isOn;
            newBuffSpell.needNewPositionOnAttack = needNewPositionOnAttack.isOn;
            newBuffSpell.needNewPositionOnInterval = needNewPositionOnInterval.isOn;
            newBuffSpell.triggerInvocationCallOneTime = triggerInvocationCallOneTime.isOn;

            if (replaceProjectile.GetType() == typeof(ProjectileSpell))
            {
                newBuffSpell.replaceProjectile = replaceProjectile.value != 0 ? (ProjectileSpell) ListCreatedElement.SpellComponents[replaceProjectile.options[replaceProjectile.value].text] : null;
            }
            
            
            newBuffSpell.linkedSpellOnDamageReceived = linkedSpellOnDamageReceived.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellOnDamageReceived.options[linkedSpellOnDamageReceived.value].text] : null;
            newBuffSpell.linkedSpellOnHit = linkedSpellOnHit.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellOnHit.options[linkedSpellOnHit.value].text] : null;
            newBuffSpell.linkedSpellOnAttack = linkedSpellOnAttack.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellOnAttack.options[linkedSpellOnAttack.value].text] : null;
            newBuffSpell.linkedSpellOnInterval = linkedSpellOnInterval.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellOnInterval.options[linkedSpellOnInterval.value].text] : null;
            newBuffSpell.conditionReduceCharge = (ConditionReduceCharge) conditionReduceCharge.value;
            newBuffSpell.effectOnSelfWhenNoCharge = effectOnSelfWhenNoCharge.value != 0 ? ListCreatedElement.Effects[effectOnSelfWhenNoCharge.options[effectOnSelfWhenNoCharge.value].text] : new Effect();
            newBuffSpell.newPlayerBehaviour = (BehaviorType) behaviorType.value;
            newBuffSpell.effectOnSelf = effectsOnSelfList;
            newBuffSpell.effectOnSelfOnDamageReceived = effectsOnSelfOnDamageReceived;
            newBuffSpell.effectOnTargetOnHit = effectsOnTargetOnHit;
            ResetCurrentPanel();
            
            return newBuffSpell;
        }
    }
}
