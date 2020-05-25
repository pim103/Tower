#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global;
using Games.Global.Spells;
using SpellEditor.PanelUtils;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor.ComponentPanel
{
    public class ProjectilePanel : MonoBehaviour
    {
        [SerializeField] private InputField idPoolObject;
        [SerializeField] private InputField speed;
        [SerializeField] private InputField damage;
        [SerializeField] private InputField duration;
        [SerializeField] private InputField multiplier;

        [SerializeField] private Toggle passingThrough;
        [SerializeField] private Dropdown linkedSpellOnEnable;
        [SerializeField] private Dropdown linkedSpellOnDisable;

        [SerializeField] private DropdownMultiSelector effectMultiSelector;

        public void InitProjectilePanel()
        {
            ResetProjectile();

            List<string> listOptionsSpellComponent = new List<string> {"None"};
            listOptionsSpellComponent.AddRange(ListCreatedElement.SpellComponents.Keys.ToList());

            linkedSpellOnDisable.options.Clear();
            linkedSpellOnEnable.options.Clear();

            linkedSpellOnDisable.AddOptions(listOptionsSpellComponent);
            linkedSpellOnEnable.AddOptions(listOptionsSpellComponent);

            effectMultiSelector.InitDropdownMultiSelect();
        }

        public SpellComponent SaveProjectile()
        {
            if (idPoolObject.text == "")
            {
                return null;
            }

            ProjectileSpell projSpell = new ProjectileSpell
            {
                damages = damage.text != "" ? float.Parse(damage.text) : 0,
                duration = duration.text != "" ? float.Parse(duration.text) : 0,
                speed = speed.text != "" ? float.Parse(speed.text) : 1,
                damageMultiplierOnDistance = multiplier.text != "" ? float.Parse(multiplier.text) : 0,
                idPoolObject = Int32.Parse(idPoolObject.text),
                passingThroughEntity = passingThrough.isOn,
                linkedSpellOnEnable = linkedSpellOnEnable.value != 0
                    ? ListCreatedElement.SpellComponents[linkedSpellOnEnable.options[linkedSpellOnEnable.value].text]
                    : null,
                linkedSpellOnDisable = linkedSpellOnDisable.value != 0
                    ? ListCreatedElement.SpellComponents[linkedSpellOnDisable.options[linkedSpellOnDisable.value].text]
                    : null
            };

            projSpell.effectsOnHit = new List<Effect>();
            foreach (string name in effectMultiSelector.selectedIndex)
            {
                projSpell.effectsOnHit.Add(ListCreatedElement.Effects[name]);
            }

            ResetProjectile();
            return projSpell;
        }

        public void ResetProjectile()
        {
            idPoolObject.text = "";
            speed.text = "";
            damage.text = "";
            duration.text = "";
            multiplier.text = "";
            passingThrough.isOn = false;

            linkedSpellOnEnable.value = 0;
            linkedSpellOnDisable.value = 0;
        }

        public void FillCurrentPanel(ProjectileSpell projectileSpell)
        {
            idPoolObject.text = projectileSpell.idPoolObject.ToString();
            speed.text = projectileSpell.speed.ToString();
            damage.text = projectileSpell.damages.ToString();
            duration.text = projectileSpell.duration.ToString();
            multiplier.text = projectileSpell.damageMultiplierOnDistance.ToString();
            passingThrough.isOn = projectileSpell.passingThroughEntity;

            linkedSpellOnEnable.value = projectileSpell.linkedSpellOnEnable != null
                ? linkedSpellOnEnable.options.FindIndex(option =>
                    option.text == projectileSpell.linkedSpellOnEnable.nameSpellComponent)
                : 0;
            linkedSpellOnDisable.value = projectileSpell.linkedSpellOnDisable != null
                ? linkedSpellOnDisable.options.FindIndex(option =>
                    option.text == projectileSpell.linkedSpellOnDisable.nameSpellComponent)
                : 0;

            effectMultiSelector.InitDropdownWithValue(projectileSpell.effectsOnHit);
        }
    }
}
#endif