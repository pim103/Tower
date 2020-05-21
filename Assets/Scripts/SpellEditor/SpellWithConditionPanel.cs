﻿using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor
{
    public class SpellWithConditionPanel : MonoBehaviour
    {
        [SerializeField] private InputField titleNameSpellWithCondition;
        [SerializeField] private InputField level;
        
        [SerializeField] private Dropdown instructionTargeting;
        [SerializeField] private Dropdown conditionType;
        [SerializeField] private Dropdown effect;
        [SerializeField] private Dropdown effectCondition;
        [SerializeField] private Dropdown spellComponent;
        
        [SerializeField] private Button saveButton;

        public void InitSpellWithCondition()
        {
            saveButton.onClick.AddListener(SaveCurrentPanel);
            titleNameSpellWithCondition.text = "";
            level.text = "";
            
            string[] enumNames = Enum.GetNames(typeof(InstructionTargeting));
            List<string> listNames = new List<string>(enumNames);
            instructionTargeting.ClearOptions();
            instructionTargeting.AddOptions(listNames);
            
            enumNames = Enum.GetNames(typeof(ConditionType));
            listNames = new List<string>(enumNames);
            conditionType.ClearOptions();
            conditionType.AddOptions(listNames);
            
            List<String> nameList = new List<string> {"None"};
            foreach (var spellComponent in ListCreatedElement.Effects)
            {
                nameList.Add(spellComponent.Key);
            }
            effect.ClearOptions();
            effect.AddOptions(nameList);
            effectCondition.ClearOptions();
            effectCondition.AddOptions(nameList);
            
            nameList = new List<string> {"None"};
            foreach (var spellComponent in ListCreatedElement.SpellComponents)
            {
                nameList.Add(spellComponent.Key);
            }
            spellComponent.ClearOptions();
            spellComponent.AddOptions(nameList);
        }

        private void ResetCurrentPanel()
        {
            titleNameSpellWithCondition.text = "";
            level.text = "";

            instructionTargeting.value = 0;
            conditionType.value = 0;
            effect.value = 0;
            spellComponent.value = 0;
        }

        private void SaveCurrentPanel()
        {
            if (titleNameSpellWithCondition.text == "" || (effect.value == 0 && effectCondition.value == 0) && spellComponent.value == 0)
            {
                return;
            }

            SpellWithCondition newSpellWithCondition = new SpellWithCondition
            {
                effect = effect.value != 0 ? ListCreatedElement.Effects[effect.options[effect.value].text] : new Effect(),
                spellComponent = spellComponent.value != 0 ? ListCreatedElement.SpellComponents[spellComponent.options[spellComponent.value].text] : null,
                conditionEffect = effectCondition.value != 0 ? ListCreatedElement.Effects[effectCondition.options[effectCondition.value].text] : new Effect(),
                instructionTargeting = (InstructionTargeting) instructionTargeting.value,
                conditionType = (ConditionType) conditionType.value,
                level = level.text == "" ? 0 : Int32.Parse(level.text)
            };
            
            ListCreatedElement.SpellWithCondition.Add(titleNameSpellWithCondition.text,newSpellWithCondition);
            
            ResetCurrentPanel();
        }
    }
}
