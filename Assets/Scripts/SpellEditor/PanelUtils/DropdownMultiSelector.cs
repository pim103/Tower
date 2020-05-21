using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor.PanelUtils
{
    public enum SelectedObject
    {
        Effect,
        Spell,
        SpellComponent,
        SpellWithCondition
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
                    break;
            }

            dropdown.AddOptions(listNames);
        }
    }
}
