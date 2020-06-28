using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor.PanelUtils
{
    public enum SelectedObject
    {
        Effect,
        Spell,
        SpellComponent,
        SpellWithCondition,
        TypeEffects,
        GeometryShader,
        ParticleEffect,
        AddedMesh
    }

    public class DropdownMultiSelector : MonoBehaviour
    {
        [SerializeField] private Dropdown dropdown;
        [SerializeField] private SelectedObject selectedObject;

        public List<string> selectedIndex;

        private Texture2D selectColor;
        private Texture2D initialColor;
        private Rect rect;

        [SerializeField] private Button showSelected;
        [SerializeField] private GameObject panelSelected;
        [SerializeField] private ShowPanel showPanel;
        [SerializeField] private SpellComponentPanel spellComponentPanel;
        
        private void Start()
        {   
            dropdown.onValueChanged.AddListener(SelectValue);
            showSelected.onClick.AddListener(ShowPanel);
            
            initialColor = new Texture2D(1, 1);
            initialColor.SetPixel(0, 0, Color.white);
            initialColor.Apply();
            
            selectColor = new Texture2D(1, 1);
            selectColor.SetPixel(0, 0, Color.green);
            selectColor.Apply();
            rect = new Rect(0,0, 1,1);
        }

        private void ShowPanel()
        {
            panelSelected.SetActive(true);
            showPanel.FillData(selectedIndex);
        }
        
        private void SelectValue(int newIndex)
        {
            if (newIndex == 0)
            {
                return;
            }

            string value = dropdown.options[newIndex].text;

            if (selectedIndex.Contains(value))
            {
                selectedIndex.Remove(value);
                dropdown.options[newIndex].image = Sprite.Create(initialColor, rect, Vector2.zero);
            }
            else
            {
                selectedIndex.Add(value);
                dropdown.options[newIndex].image = Sprite.Create(selectColor, rect, Vector2.zero);
            }

            dropdown.value = 0;
        }

        public void InitDropdownMultiSelect()
        {
            selectedIndex = new List<string>();
            List<string> listNames = new List<string>();
            dropdown.options.Clear();

            listNames.Add("Choose your " + selectedObject);

            switch (selectedObject)
            {
                case SelectedObject.Effect:
                    listNames.AddRange(ListCreatedElement.Effects.Keys.ToList());
                    break;
                case SelectedObject.Spell:
                    listNames.AddRange(ListCreatedElement.Spell.Keys.ToList());
                    break;
                case SelectedObject.SpellComponent:
                    listNames.AddRange(ListCreatedElement.SpellComponents.Keys.ToList());
                    break;
                case SelectedObject.SpellWithCondition:
                    listNames.AddRange(ListCreatedElement.SpellWithCondition.Keys.ToList());
                    break;
                case SelectedObject.TypeEffects:
                    string[] enumNames = Enum.GetNames(typeof(TypeEffect));
                    listNames.AddRange(enumNames.ToList());
                    break;
                case SelectedObject.GeometryShader:
                    foreach (Material material in spellComponentPanel.geometryShaderMaterials)
                    {
                        listNames.Add(material.name);
                    }
                    break;
                case SelectedObject.ParticleEffect:
                    foreach (GameObject particleObject in spellComponentPanel.particleObjects)
                    {
                        listNames.Add(particleObject.name);
                    }
                    break;
                case SelectedObject.AddedMesh:
                    foreach (GameObject additionalMesh in spellComponentPanel.additionalMeshs)
                    {
                        listNames.Add(additionalMesh.name);
                    }
                    break;
            }

            dropdown.AddOptions(listNames);
        }
        
        public void InitDropdownWithValue<T>(List<T> genericObjects)
        {
            if (genericObjects == null || genericObjects.Count == 0)
            {
                return;
            }

            foreach (T obj in genericObjects)
            {
                if (obj == null)
                {
                    continue;
                }

                switch (selectedObject)
                {
                    case SelectedObject.Effect:
                        Effect effect = obj as Effect;
                        selectedIndex.Add(effect.nameEffect);
                        break;
                    case SelectedObject.Spell:
                        Spell spell = obj as Spell;
                        selectedIndex.Add(spell.nameSpell);
                        break;
                    case SelectedObject.SpellComponent:
                        SpellComponent spellComponent = obj as SpellComponent;
                        selectedIndex.Add(spellComponent.nameSpellComponent);
                        break;
                    case SelectedObject.SpellWithCondition:
                        SpellWithCondition spellWithCondition = obj as SpellWithCondition;
                        selectedIndex.Add(spellWithCondition.nameSpellWithCondition);
                        break;
                    case SelectedObject.GeometryShader:
                        Material geometryMaterial = obj as Material;
                        selectedIndex.Add(geometryMaterial.name);
                        break;
                    case SelectedObject.ParticleEffect:
                        GameObject particleObject = obj as GameObject;
                        selectedIndex.Add(particleObject.name);
                        break;
                    case SelectedObject.AddedMesh:
                        GameObject addedMesh = obj as GameObject;
                        selectedIndex.Add(addedMesh.name);
                        break;
                }
            }
        }

        public void InitDropdownWithValueFromEnum(List<TypeEffect> typeEffectList)
        {
            foreach (TypeEffect type in typeEffectList)
            {
                selectedIndex.Add(type.ToString());
            }
        }

        public List<int> NamesToIndex()
        {
            List<int> indexList = new List<int>();
            foreach (var selectedString in selectedIndex)
            {
                indexList.Add(dropdown.options.FindIndex((i) => i.text.Equals(selectedString)));
            }

            return indexList;
        }
    }
}
