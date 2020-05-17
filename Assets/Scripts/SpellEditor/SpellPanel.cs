using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor
{
    public class SpellPanel : MonoBehaviour
    {
        [SerializeField] private Button save;
        [SerializeField] private InputField nameSpell;
        
        [SerializeField] private InputField cooldown;
        [SerializeField] private InputField cost;
        [SerializeField] private InputField castTime;
        [SerializeField] private InputField nbUse;

        [SerializeField] private Toggle deactivePassive;
        [SerializeField] private Toggle canCastDuringCast;
        [SerializeField] private Toggle interruptCast;
        [SerializeField] private Toggle canRecast;

        [SerializeField] private Dropdown activeSpellComponent;
        [SerializeField] private Dropdown passiveSpellComponent;
        [SerializeField] private Dropdown duringSpellComponent;
        [SerializeField] private Dropdown recastSpellComponent;
        
        // Start is called before the first frame update
        void Start()
        {
            save.onClick.AddListener(SaveCurrentPanel);
        }

        public void InitSpellPanel()
        {
            List<string> listNames = ListCreatedElement.SpellComponents.Keys.ToList();

            activeSpellComponent.options.Clear();
            activeSpellComponent.AddOptions(listNames);
            
            passiveSpellComponent.options.Clear();
            passiveSpellComponent.AddOptions(listNames);
            
            duringSpellComponent.options.Clear();
            duringSpellComponent.AddOptions(listNames);
            
            recastSpellComponent.options.Clear();
            recastSpellComponent.AddOptions(listNames);
        }
        
        public void SaveCurrentPanel()
        {
            if (nameSpell.text == "" || (activeSpellComponent.value == 0 && passiveSpellComponent.value == 0))
            {
                return;
            }

            if (cooldown.text == "")
            {
                cooldown.text = "0";
            }
            if (cost.text == "")
            {
                cost.text = "0";
            }
            if (castTime.text == "")
            {
                castTime.text = "0";
            }
            if (nbUse.text == "")
            {
                nbUse.text = "-1";
            }

            Spell spell = new Spell
            {
                cooldown = Int32.Parse(cooldown.text),
                cost = Int32.Parse(cost.text),
                castTime = Int32.Parse(castTime.text),
                nbUse = Int32.Parse(nbUse.text),
                canRecast = canRecast.isOn,
                canCastDuringCast = canCastDuringCast.isOn,
                interruptCurrentCast = interruptCast.isOn,
                deactivatePassiveWhenActive = deactivePassive.isOn,
                activeSpellComponent = activeSpellComponent.value != 0 ? ListCreatedElement.SpellComponents[activeSpellComponent.options[activeSpellComponent.value].text] : null,
                passiveSpellComponent = passiveSpellComponent.value != 0 ? ListCreatedElement.SpellComponents[passiveSpellComponent.options[passiveSpellComponent.value].text] : null,
                recastSpellComponent = recastSpellComponent.value != 0 ? ListCreatedElement.SpellComponents[recastSpellComponent.options[recastSpellComponent.value].text] : null,
                duringCastSpellComponent = duringSpellComponent.value != 0 ? ListCreatedElement.SpellComponents[duringSpellComponent.options[duringSpellComponent.value].text] : null
            };

            ListCreatedElement.Spell.Add(nameSpell.text, spell);

            ResetCurrentSpell();
        }

        public void ResetCurrentSpell()
        {
            cooldown.text = "";
            cost.text = "";
            castTime.text = "";
            nbUse.text = "";
            canRecast.isOn = false;
            canCastDuringCast.isOn = false;
            interruptCast.isOn = false;
            deactivePassive.isOn = false;

            activeSpellComponent.value = 0;
            passiveSpellComponent.value = 0;
            recastSpellComponent.value = 0;
            duringSpellComponent.value = 0;
        }
    }
}
