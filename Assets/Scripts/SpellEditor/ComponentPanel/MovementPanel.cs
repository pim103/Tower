#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor.ComponentPanel
{
    public class MovementPanel : MonoBehaviour
    {
        [SerializeField] private InputField duration;
        [SerializeField] private InputField speed;
        [SerializeField] private Toggle isFollowingMouse;
        [SerializeField] private Dropdown typeMovement;
        [SerializeField] private Dropdown linkedSpellAtTheStart;
        [SerializeField] private Dropdown linkedSpellAtTheEnd;

        public void InitMovementPanel()
        {
            string[] enumList = Enum.GetNames(typeof(MovementSpellType));
            List<string> enumChoices = enumList.ToList();

            typeMovement.options.Clear();
            typeMovement.AddOptions(enumChoices);

            List<string> listOptionsSpellComponent = new List<string> {"None"};
            listOptionsSpellComponent.AddRange(ListCreatedElement.SpellComponents.Keys.ToList());

            linkedSpellAtTheStart.options.Clear();
            linkedSpellAtTheStart.AddOptions(listOptionsSpellComponent);

            linkedSpellAtTheEnd.options.Clear();
            linkedSpellAtTheEnd.AddOptions(listOptionsSpellComponent);

            ResetCurrentPanel();
        }

        public SpellComponent SaveMovement()
        {
            MovementSpell movementPanel = new MovementSpell
            {
                duration = duration.text != "" ? float.Parse(duration.text) : -1,
                speed = speed.text != "" ? float.Parse(speed.text) : 1,
                isFollowingMouse = isFollowingMouse.isOn,
                movementSpellType = (MovementSpellType) typeMovement.value,
                linkedSpellAtTheStart = linkedSpellAtTheStart.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellAtTheStart.options[linkedSpellAtTheStart.value].text] : null,
                linkedSpellAtTheEnd = linkedSpellAtTheEnd.value != 0 ? ListCreatedElement.SpellComponents[linkedSpellAtTheEnd.options[linkedSpellAtTheEnd.value].text] : null
            };

            ResetCurrentPanel();
            return movementPanel;
        }

        private void ResetCurrentPanel()
        {
            duration.text = "";
            speed.text = "";
            isFollowingMouse.isOn = false;
            typeMovement.value = 0;
            linkedSpellAtTheStart.value = 0;
            linkedSpellAtTheEnd.value = 0;
        }

        public void FillCurrentPanel(MovementSpell movementSpell)
        {
            duration.text = movementSpell.duration.ToString();
            speed.text = movementSpell.speed.ToString();
            isFollowingMouse.isOn = movementSpell.isFollowingMouse;
            typeMovement.value = (int) movementSpell.movementSpellType;
            linkedSpellAtTheStart.value = movementSpell.linkedSpellAtTheStart != null ? linkedSpellAtTheStart.options.FindIndex(option => option.text == movementSpell.linkedSpellAtTheStart.nameSpellComponent) : 0;
            linkedSpellAtTheEnd.value = movementSpell.linkedSpellAtTheEnd != null ? linkedSpellAtTheEnd.options.FindIndex(option => option.text == movementSpell.linkedSpellAtTheEnd.nameSpellComponent) : 0;
        }
    }
}
#endif