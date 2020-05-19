using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Spells;
using SpellEditor.PanelUtils;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor.ComponentPanel
{
    public class SummonPanel : MonoBehaviour
    {
        [SerializeField] private InputField hp;
        [SerializeField] private InputField duration;
        [SerializeField] private InputField attackDamage;
        [SerializeField] private InputField speed;
        [SerializeField] private InputField summonNumber;
        [SerializeField] private InputField attackSpeed;
        [SerializeField] private InputField nbUseSpells;
        
        [SerializeField] private Dropdown behaviorType;
        [SerializeField] private Dropdown attackBehaviorType;
        [SerializeField] private Dropdown linkedSpellOnEnable;
        [SerializeField] private Dropdown linkedSpellOnDisappear;
        [SerializeField] private Dropdown basicAttack;
        [SerializeField] private Dropdown spellWhenPlayerCall;
        
        [SerializeField] private Toggle isTargetable;
        [SerializeField] private Toggle isUnique;
        [SerializeField] private Toggle canMove;
        
        [SerializeField] private DropdownMultiSelector spells;

        public void InitSummonPanel()
        {
            string[] enumNames = Enum.GetNames(typeof(BehaviorType));
            List<string> listNames = new List<string>(enumNames);
            behaviorType.ClearOptions();
            behaviorType.AddOptions(listNames);
            
            enumNames = Enum.GetNames(typeof(AttackBehaviorType));
            listNames = new List<string>(enumNames);
            attackBehaviorType.ClearOptions();
            attackBehaviorType.AddOptions(listNames);
            
            List<String> nameList = new List<string> {"None"};
            foreach (var spellComponent in ListCreatedElement.SpellComponents)
            {
                nameList.Add(spellComponent.Key);
            }
            linkedSpellOnEnable.ClearOptions();
            linkedSpellOnEnable.AddOptions(nameList);
            linkedSpellOnDisappear.ClearOptions();
            linkedSpellOnDisappear.AddOptions(nameList);
            
            nameList = new List<string> {"None"};
            foreach (var spellComponent in ListCreatedElement.Spell)
            {
                nameList.Add(spellComponent.Key);
            }
            basicAttack.ClearOptions();
            basicAttack.AddOptions(nameList);
            spellWhenPlayerCall.ClearOptions();
            spellWhenPlayerCall.AddOptions(nameList);
            
            spells.InitDropdownMultiSelect();
        }

        public void ResetCurrentPanel()
        {
            hp.text = "";
            duration.text = "";
            attackDamage.text = "";
            speed.text = "";
            summonNumber.text = "";
            attackSpeed.text = "";
            nbUseSpells.text = "";

            behaviorType.value = 0;
            attackBehaviorType.value = 0;
            linkedSpellOnEnable.value = 0;
            linkedSpellOnDisappear.value = 0;
            basicAttack.value = 0;
            spellWhenPlayerCall.value = 0;

            isTargetable.isOn = false;
            isUnique.isOn = false;
            canMove.isOn = false;
        }

        public SummonSpell SaveCurrentPanel()
        {
            if (linkedSpellOnEnable.value == 0 && linkedSpellOnDisappear.value == 0 && basicAttack.value == 0 &&
                spellWhenPlayerCall.value == 0 && linkedSpellOnEnable.value == 0 && spells.selectedIndex.Count == 0)
            {
                return null;
            }

            SummonSpell newSummonSpell = new SummonSpell();
            newSummonSpell.hp = hp.text != "" ? Int32.Parse(hp.text) : 1;
            newSummonSpell.duration = duration.text != "" ? Int32.Parse(duration.text) : 1;
            newSummonSpell.attackDamage = attackDamage.text != "" ? Int32.Parse(attackDamage.text) : 0;
            newSummonSpell.moveSpeed = speed.text != "" ? Int32.Parse(speed.text) : 0;
            newSummonSpell.summonNumber = summonNumber.text != "" ? Int32.Parse(summonNumber.text) : 1;
            newSummonSpell.attackSpeed = attackSpeed.text != "" ? Int32.Parse(attackSpeed.text) : 0;
            newSummonSpell.nbUseSpells = nbUseSpells.text != "" ? Int32.Parse(nbUseSpells.text) : 0;

            newSummonSpell.BehaviorType = (BehaviorType) behaviorType.value;
            newSummonSpell.AttackBehaviorType = (AttackBehaviorType) attackBehaviorType.value;
            newSummonSpell.linkedSpellOnEnable = linkedSpellOnEnable.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellOnEnable.options[linkedSpellOnEnable.value].text] : null;
            newSummonSpell.linkedSpellOnDisapear = linkedSpellOnDisappear.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellOnDisappear.options[linkedSpellOnDisappear.value].text] : null;
            newSummonSpell.basicAttack = basicAttack.value != 0 ? ListCreatedElement.Spell[basicAttack.options[basicAttack.value].text] : null;
            newSummonSpell.spellWhenPlayerCall = spellWhenPlayerCall.value != 0 ? ListCreatedElement.SpellComponents[spellWhenPlayerCall.options[spellWhenPlayerCall.value].text] : null;

            newSummonSpell.isTargetable = isTargetable.isOn;
            newSummonSpell.isUnique = isUnique.isOn;
            newSummonSpell.canMove = canMove.isOn;
            
            List<Spell> spellsList = new List<Spell>();
            foreach (var spell in ListCreatedElement.Spell)
            {
                if (spells.selectedIndex.Contains(spell.Key))
                {
                    spellsList.Add(spell.Value);
                }
            }
            newSummonSpell.spells = spellsList;

            ResetCurrentPanel();
            return newSummonSpell;
        }
    }
}
