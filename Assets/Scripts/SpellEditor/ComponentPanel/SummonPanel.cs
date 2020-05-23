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
        [SerializeField] private InputField idPoolObject;
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
            idPoolObject.text = "";
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
                spellWhenPlayerCall.value == 0 && linkedSpellOnEnable.value == 0 && spells.selectedIndex.Count == 0 || idPoolObject.text == "")
            {
                return null;
            }

            SummonSpell newSummonSpell = new SummonSpell();
            newSummonSpell.idPoolObject = Int32.Parse(idPoolObject.text);
            newSummonSpell.hp = hp.text != "" ? float.Parse(hp.text) : 1;
            newSummonSpell.duration = duration.text != "" ? float.Parse(duration.text) : 1;
            newSummonSpell.attackDamage = attackDamage.text != "" ? float.Parse(attackDamage.text) : 0;
            newSummonSpell.moveSpeed = speed.text != "" ? float.Parse(speed.text) : 0;
            newSummonSpell.summonNumber = summonNumber.text != "" ? Int32.Parse(summonNumber.text) : 1;
            newSummonSpell.attackSpeed = attackSpeed.text != "" ? float.Parse(attackSpeed.text) : 0;
            newSummonSpell.nbUseSpells = nbUseSpells.text != "" ? Int32.Parse(nbUseSpells.text) : 0;

            newSummonSpell.BehaviorType = (BehaviorType) behaviorType.value;
            newSummonSpell.AttackBehaviorType = (AttackBehaviorType) attackBehaviorType.value;
            newSummonSpell.linkedSpellOnEnable = linkedSpellOnEnable.value != 0
                ? ListCreatedElement.SpellComponents[linkedSpellOnEnable.options[linkedSpellOnEnable.value].text]
                : null;
            newSummonSpell.linkedSpellOnDisapear = linkedSpellOnDisappear.value != 0
                ? ListCreatedElement.SpellComponents[linkedSpellOnDisappear.options[linkedSpellOnDisappear.value].text]
                : null;
            newSummonSpell.basicAttack = basicAttack.value != 0
                ? ListCreatedElement.Spell[basicAttack.options[basicAttack.value].text]
                : null;
            newSummonSpell.spellWhenPlayerCall = spellWhenPlayerCall.value != 0
                ? ListCreatedElement.SpellComponents[spellWhenPlayerCall.options[spellWhenPlayerCall.value].text]
                : null;

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

        public void FillCurrentPanel(SummonSpell summonSpell)
        {
            idPoolObject.text = summonSpell.idPoolObject.ToString();
            hp.text = summonSpell.hp.ToString();
            duration.text = summonSpell.duration.ToString();
            attackDamage.text = summonSpell.attackDamage.ToString();
            speed.text = summonSpell.moveSpeed.ToString();
            summonNumber.text = summonSpell.summonNumber.ToString();
            attackSpeed.text = summonSpell.attackSpeed.ToString();
            nbUseSpells.text = summonSpell.nbUseSpells.ToString();

            behaviorType.value = (int) summonSpell.BehaviorType;
            attackBehaviorType.value = (int) summonSpell.AttackBehaviorType;
            linkedSpellOnEnable.value = summonSpell.linkedSpellOnEnable != null ? linkedSpellOnEnable.options.FindIndex(option => option.text == summonSpell.linkedSpellOnEnable.nameSpellComponent) : 0;
            linkedSpellOnDisappear.value = summonSpell.linkedSpellOnDisapear != null ? linkedSpellOnDisappear.options.FindIndex(option => option.text == summonSpell.linkedSpellOnDisapear.nameSpellComponent) : 0;
            basicAttack.value = summonSpell.basicAttack != null ? basicAttack.options.FindIndex(option => option.text == summonSpell.basicAttack.nameSpell) : 0;
            spellWhenPlayerCall.value = summonSpell.spellWhenPlayerCall != null ? spellWhenPlayerCall.options.FindIndex(option => option.text == summonSpell.spellWhenPlayerCall.nameSpellComponent) : 0;

            isTargetable.isOn = summonSpell.isTargetable;
            isUnique.isOn = summonSpell.isUnique;
            canMove.isOn = summonSpell.canMove;

            spells.InitDropdownWithValue(summonSpell.spells);
        }
    }
}