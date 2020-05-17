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

        private List<int> selectedIndex;

        private Texture2D selectColor;
        private Texture2D initialColor;
        private Rect rect;

        private void Start()
        {
            selectedIndex = new List<int>();
            
            dropdown.onValueChanged.AddListener(SelectValue);
            initialColor = new Texture2D(1, 1);
            initialColor.SetPixel(0, 0, Color.white);
            initialColor.Apply();
            
            selectColor = new Texture2D(1, 1);
            selectColor.SetPixel(0, 0, Color.green);
            selectColor.Apply();
            rect = new Rect(0,0, 1,1);
        }

        private void SelectValue(int newIndex)
        {
            if (newIndex == 0)
            {
                return;
            }

            if (selectedIndex.Contains(newIndex))
            {
                Debug.Log("unselect");
                selectedIndex.Remove(newIndex);
                dropdown.options[newIndex].image = Sprite.Create(initialColor, rect, Vector2.zero);
            }
            else
            {
                Debug.Log("select");
                selectedIndex.Add(newIndex);
                dropdown.options[newIndex].image = Sprite.Create(selectColor, rect, Vector2.zero);
            }

            dropdown.value = 0;
        }

        public void InitDropdownMultiSelect()
        {
            List<string> listNames = null;

            switch (selectedObject)
            {
                case SelectedObject.Effect:
                    listNames = ListCreatedElement.Effects.Keys.ToList();
                    Debug.Log(listNames);
                    break;
                case SelectedObject.Spell:
                    listNames = ListCreatedElement.Spell.Keys.ToList();
                    break;
                case SelectedObject.SpellComponent:
                    listNames = ListCreatedElement.SpellComponents.Keys.ToList();
                    break;
                case SelectedObject.SpellWithCondition:
//                    listNames = ListCreatedElement.SpellComponents.Keys.ToList();
                    break;
            }

            if (listNames != null)
            {
                dropdown.AddOptions(listNames);
            }
        }
    }
}
