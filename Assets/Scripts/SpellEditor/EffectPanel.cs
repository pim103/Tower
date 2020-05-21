using System;
using System.Collections.Generic;
using Games.Global;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor
{
    public class EffectPanel : MonoBehaviour
    {
        [SerializeField] private Button validateButton;

        [SerializeField] private Dropdown typeEffectDropdown;
        [SerializeField] private Dropdown directionDropdown;
        [SerializeField] private Dropdown originDropdown;

        [SerializeField] private InputField nameEffect;
        [SerializeField] private InputField duration;
        [SerializeField] private InputField level;

        // Start is called before the first frame update
        void Start()
        {
            validateButton.onClick.AddListener(SaveCurrentPanel);

            string[] enumNames = Enum.GetNames(typeof(TypeEffect));
            List<string> listNames = new List<string>(enumNames);
            typeEffectDropdown.AddOptions(listNames);

            enumNames = Enum.GetNames(typeof(OriginExpulsion));
            listNames = new List<string>(enumNames);
            originDropdown.AddOptions(listNames);

            enumNames = Enum.GetNames(typeof(DirectionExpulsion));
            listNames = new List<string>(enumNames);
            directionDropdown.AddOptions(listNames);
        }

        public void SaveCurrentPanel()
        {
            if (nameEffect.text == "")
            {
                return;
            }

            if (duration.text == "")
            {
                duration.text = "0";
            }

            if (level.text == "")
            {
                level.text = "1";
            }

            Effect newEffect = new Effect
            {
                nameEffect = nameEffect.text,
                typeEffect = (TypeEffect) typeEffectDropdown.value,
                originExpulsion = (OriginExpulsion) originDropdown.value,
                directionExpul = (DirectionExpulsion) directionDropdown.value,
                level = Int32.Parse(level.text),
                durationInSeconds = Int32.Parse(duration.text),
            };
            
            ListCreatedElement.Effects.Add(nameEffect.text, newEffect);

            ResetCurrentEffect();
        }
    
        public void ResetCurrentEffect()
        {
            typeEffectDropdown.value = 0;
            directionDropdown.value = 0;
            originDropdown.value = 0;

            nameEffect.text = "";
            duration.text = "";
            level.text = "";
        }
    }
}
