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

        [SerializeField] private Dropdown spellSelector;
        
        // Start is called before the first frame update
        void Start()
        {
            save.onClick.AddListener(SaveCurrentPanel);
            spellSelector.onValueChanged.AddListener(EditSpellAfterSelectorChoice);
        }

        public void InitSpellPanel()
        {
            List<string> listNames = new List<string>();
            listNames.Add("None");
            listNames.AddRange(ListCreatedElement.SpellComponents.Keys.ToList());

            activeSpellComponent.options.Clear();
            activeSpellComponent.AddOptions(listNames);
            
            passiveSpellComponent.options.Clear();
            passiveSpellComponent.AddOptions(listNames);
            
            duringSpellComponent.options.Clear();
            duringSpellComponent.AddOptions(listNames);
            
            recastSpellComponent.options.Clear();
            recastSpellComponent.AddOptions(listNames);

            listNames = new List<string>();
            listNames.Add("Nouveau spell");
            listNames.AddRange(ListCreatedElement.Spell.Keys.ToList());

            spellSelector.options.Clear();
            spellSelector.AddOptions(listNames);
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

                nameSpell = nameSpell.text,
                cooldown = float.Parse(cooldown.text),
                cost = float.Parse(cost.text),
                castTime = float.Parse(castTime.text),
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

            if (spellSelector.value != 0)
            {
                Debug.Log("WARNING - ERASE DATA");
                string spellChoose = spellSelector.options[spellSelector.value].text;

                NavBar.ModifyExistingComponent(ListCreatedElement.Spell[spellChoose], spell);
            }
            else
            {
                if (ListCreatedElement.Spell.ContainsKey(nameSpell.text))
                {
                    Debug.Log("!!!!!!!! TRY TO CREATE SPELL WITH SAME NAME - PLEASE CHOOSE ANOTHER NAME OR SELECT SPELL !!!!!!!!");
                    return;
                }
                ListCreatedElement.Spell.Add(nameSpell.text, spell);
            }

            ResetCurrentSpell();
        }

        public void ResetCurrentSpell()
        {
            nameSpell.text = "";
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
            spellSelector.value = 0;
        }
        
        public void EditSpellAfterSelectorChoice(int newIndex)
        {
            if (newIndex == 0)
            {
                return;
            }

            string spellChoose = spellSelector.options[newIndex].text;

            if (!ListCreatedElement.Spell.ContainsKey(spellChoose))
            {
                return;
            }

            Spell spellSelected = ListCreatedElement.Spell[spellChoose];
            nameSpell.text = spellSelected.nameSpell;
            cooldown.text = spellSelected.cooldown.ToString();
            cost.text = spellSelected.cost.ToString();
            castTime.text = spellSelected.castTime.ToString();
            nbUse.text = spellSelected.nbUse.ToString();
            canRecast.isOn = spellSelected.canRecast;
            canCastDuringCast.isOn = spellSelected.canCastDuringCast;
            interruptCast.isOn = spellSelected.interruptCurrentCast;
            deactivePassive.isOn = spellSelected.deactivatePassiveWhenActive;

            activeSpellComponent.value = spellSelected.activeSpellComponent != null ? activeSpellComponent.options.FindIndex(option => option.text == spellSelected.activeSpellComponent.nameSpellComponent) : 0;
            passiveSpellComponent.value = spellSelected.passiveSpellComponent != null ? passiveSpellComponent.options.FindIndex(option => option.text == spellSelected.passiveSpellComponent.nameSpellComponent) : 0;
            recastSpellComponent.value = spellSelected.recastSpellComponent != null ? recastSpellComponent.options.FindIndex(option => option.text == spellSelected.recastSpellComponent.nameSpellComponent) : 0;
            duringSpellComponent.value = spellSelected.duringCastSpellComponent != null ? duringSpellComponent.options.FindIndex(option => option.text == spellSelected.duringCastSpellComponent.nameSpellComponent) : 0;
        }
    }
}
