using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor.ComponentPanel
{
    public class PassivePanel : MonoBehaviour
    {
        [SerializeField] private InputField interval;
        [SerializeField] private Dropdown linkedSpellOnInterval;
        [SerializeField] private Dropdown newDefensiveSpell;
        [SerializeField] private Dropdown permanentLinkedEffect;

        public void InitPassivePanel()
        {
            List<string> listOptionsSpellComponent = new List<string> {"None"};
            listOptionsSpellComponent.AddRange(ListCreatedElement.SpellComponents.Keys.ToList());

            List<string> listOptionsSpell = new List<string> {"None"};
            listOptionsSpell.AddRange(ListCreatedElement.Spell.Keys.ToList());

            linkedSpellOnInterval.options.Clear();
            linkedSpellOnInterval.AddOptions(listOptionsSpellComponent);

            permanentLinkedEffect.options.Clear();
            permanentLinkedEffect.AddOptions(listOptionsSpellComponent);

            newDefensiveSpell.options.Clear();
            newDefensiveSpell.AddOptions(listOptionsSpell);

            ResetPassivePanel();
        }

        private void ResetPassivePanel()
        {
            newDefensiveSpell.value = 0;
            permanentLinkedEffect.value = 0;
            linkedSpellOnInterval.value = 0;
        }

        public SpellComponent SavePassive()
        {
            if (newDefensiveSpell.value == 0 && linkedSpellOnInterval.value == 0  && permanentLinkedEffect.value == 0)
            {
                return null;
            }

            if (linkedSpellOnInterval.value != 0 && interval.text == "")
            {
                return null;
            }

            PassiveSpell passiveSpell = new PassiveSpell
            {
                interval = interval.text != "" ? float.Parse(interval.text) : 0,
                linkedEffectOnInterval = linkedSpellOnInterval.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellOnInterval.options[linkedSpellOnInterval.value].text] : null,
                permanentLinkedEffect = permanentLinkedEffect.value != 0 ? ListCreatedElement.SpellComponents[permanentLinkedEffect.options[permanentLinkedEffect.value].text] : null,
                newDefensiveSpell = newDefensiveSpell.value != 0 ? ListCreatedElement.Spell[newDefensiveSpell.options[newDefensiveSpell.value].text] : null
            };

            ResetPassivePanel();
            return passiveSpell;
        }
    }
}
