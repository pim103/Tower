﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using SpellEditor.ComponentPanel;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor
{
    public class SpellComponentPanel : MonoBehaviour
    {
        [SerializeField] private InputField nameSpellComponent;
        [SerializeField] private Button save;
        
        [SerializeField] private Dropdown typeSpell;
        [SerializeField] private Dropdown typeDamage;
        [SerializeField] private Dropdown position;
        [SerializeField] private Dropdown direction;

        [SerializeField] private Toggle isBasicAttack;
        [SerializeField] private Toggle positionMidEntity;
        [SerializeField] private Toggle castByPassive;

        [SerializeField] private GameObject[] panelsTypeSpell;

        private Dictionary<string, GameObject> panels;

        [SerializeField] private ProjectilePanel projectilePanel;

        // Start is called before the first frame update
        void Start()
        {
            save.onClick.AddListener(SaveCurrentPanel);

            panels = new Dictionary<string, GameObject>();
            foreach (GameObject go in panelsTypeSpell)
            {
                panels.Add(go.name, go);
            }
            
            string[] enumNames = Enum.GetNames(typeof(TypeSpell));
            List<string> listNames = new List<string>(enumNames);
            typeSpell.AddOptions(listNames);

            enumNames = Enum.GetNames(typeof(DamageType));
            listNames = new List<string>(enumNames);
            typeDamage.AddOptions(listNames);

            enumNames = Enum.GetNames(typeof(OriginalPosition));
            listNames = new List<string>(enumNames);
            position.AddOptions(listNames);
            
            enumNames = Enum.GetNames(typeof(OriginalDirection));
            listNames = new List<string>(enumNames);
            direction.AddOptions(listNames);
            
            typeSpell.onValueChanged.AddListener(ChangeTypeSpell);
            ChangeTypeSpell(typeSpell.value);
        }

        private void DeactivateAllPanel()
        {
            foreach (GameObject go in panelsTypeSpell)
            {
                go.SetActive(false);
            }
        }
        
        public void ChangeTypeSpell(int newIndex)
        {
            TypeSpell newTypeSpell = (TypeSpell) newIndex;

            DeactivateAllPanel();

            switch (newTypeSpell)
            {
                case TypeSpell.Buff:
                    panels["BuffPanel"].SetActive(true);
                    break;
                case TypeSpell.Movement:
                    panels["MovementPanel"].SetActive(true);
                    break;
                case TypeSpell.Passive:
                    panels["PassivePanel"].SetActive(true);
                    break;
                case TypeSpell.Projectile:
                    panels["ProjectilePanel"].SetActive(true);
                    projectilePanel.InitProjectilePanel();
                    break;
                case TypeSpell.Summon:
                    panels["SummonPanel"].SetActive(true);
                    break;
                case TypeSpell.Transformation:
                    panels["TransformationPanel"].SetActive(true);
                    break;
                case TypeSpell.Wave:
                    panels["WavePanel"].SetActive(true);
                    break;
                case TypeSpell.AreaOfEffect:
                    panels["AreaOfEffectPanel"].SetActive(true);
                    break;
            }
        }

        public void SaveCurrentPanel()
        {
            SpellComponent spellComponentToSave = null;

            if (nameSpellComponent.text == "")
            {
                return;
            }

            switch ((TypeSpell) typeSpell.value)
            {
                case TypeSpell.Buff:
                    break;
                case TypeSpell.Movement:
                    break;
                case TypeSpell.Passive:
                    break;
                case TypeSpell.Projectile:
                    spellComponentToSave = projectilePanel.SaveProjectile();
                    break;
                case TypeSpell.Summon:
                    break;
                case TypeSpell.Transformation:
                    break;
                case TypeSpell.Wave:
                    break;
                case TypeSpell.AreaOfEffect:
                    break;
            }

            if (spellComponentToSave == null)
            {
                return;
            }

            spellComponentToSave.damageType = (DamageType) typeDamage.value;
            spellComponentToSave.typeSpell = (TypeSpell) typeSpell.value;
            spellComponentToSave.OriginalDirection = (OriginalDirection) direction.value;
            spellComponentToSave.OriginalPosition = (OriginalPosition) position.value;

            spellComponentToSave.isBasicAttack = isBasicAttack.isOn;
            spellComponentToSave.needPositionToMidToEntity = positionMidEntity.isOn;
            spellComponentToSave.castByPassive = castByPassive.isOn;

            ListCreatedElement.SpellComponents.Add(nameSpellComponent.text, spellComponentToSave);

            ResetCurrentSpellComponent();
        }

        private void ResetCurrentSpellComponent()
        {
            nameSpellComponent.text = "";
            typeDamage.value = 0;
            typeSpell.value = 0;
            position.value = 0;
            direction.value = 0;

            isBasicAttack.isOn = false;
            positionMidEntity.isOn = false;
            castByPassive.isOn = false;
        }
    }
}