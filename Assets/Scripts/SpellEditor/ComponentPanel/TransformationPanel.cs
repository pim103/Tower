using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Spells;
using SpellEditor.PanelUtils;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor.ComponentPanel
{
    public class TransformationPanel : MonoBehaviour
    {
        [SerializeField] private InputField duration;
        [SerializeField] private InputField idPoolObject;
        [SerializeField] private DropdownMultiSelector newSpellList;

        public void InitTransformationPanel()
        {
            ResetCurrentPanel();
        }

        public SpellComponent SaveTransformation()
        {
            if (newSpellList.selectedIndex.Count == 0)
            {
                return null;
            }

            List<Spell> newSpells = ListCreatedElement.Spell.Select(pair =>
            {
                if (newSpellList.selectedIndex.Contains(pair.Key))
                {
                    return pair.Value;
                }

                return null;
            }).ToList();
            
            TransformationSpell transformationSpell = new TransformationSpell
            {
                duration = duration.text != "" ? Int32.Parse(duration.text) : -1,
                idPoolPrefab = idPoolObject.text != "" ? Int32.Parse(idPoolObject.text) : -1,
                newSpells = newSpells
            };

            ResetCurrentPanel();
            return transformationSpell;
        }

        private void ResetCurrentPanel()
        {
            newSpellList.InitDropdownMultiSelect();
            duration.text = "";
            idPoolObject.text = "";
        }
    }
}
