using System;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private Dropdown effectSelector;

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
            
            effectSelector.onValueChanged.AddListener(EditEffectAfterSelectorChoice);
        }

        public void InitEffectPanel()
        {
            List<string> listNames = new List<string>();
            listNames.Add("Nouvel effet");
            listNames.AddRange(ListCreatedElement.Effects.Keys.ToList());

            effectSelector.options.Clear();
            effectSelector.AddOptions(listNames);
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
                durationInSeconds = float.Parse(duration.text),
            };
        
            if (effectSelector.value != 0)
            {
                Debug.Log("WARNING - ERASE DATA");
                string effectChoose = effectSelector.options[effectSelector.value].text;

                NavBar.ModifyExistingComponent(ListCreatedElement.Effects[effectChoose], newEffect);
            }
            else
            {
                if (ListCreatedElement.Effects.ContainsKey(nameEffect.text))
                {
                    Debug.Log("!!!!!!!! TRY TO CREATE EFFECT WITH SAME NAME - PLEASE CHOOSE ANOTHER NAME OR SELECT EFFECT !!!!!!!!");
                    return;
                }
                ListCreatedElement.Effects.Add(nameEffect.text, newEffect);
            }

            ResetCurrentEffect();
        }
    
        public void ResetCurrentEffect()
        {
            typeEffectDropdown.value = 0;
            directionDropdown.value = 0;
            originDropdown.value = 0;
            effectSelector.value = 0;

            nameEffect.text = "";
            duration.text = "";
            level.text = "";
        }

        public void EditEffectAfterSelectorChoice(int newIndex)
        {
            if (newIndex == 0)
            {
                return;
            }

            string effectChoose = effectSelector.options[newIndex].text;

            if (!ListCreatedElement.Effects.ContainsKey(effectChoose))
            {
                return;
            }

            Effect effectSelected = ListCreatedElement.Effects[effectChoose];

            typeEffectDropdown.value = (int) effectSelected.typeEffect;
            directionDropdown.value = (int) effectSelected.directionExpul;
            originDropdown.value = (int) effectSelected.originExpulsion;

            nameEffect.text = effectSelected.nameEffect;
            duration.text = effectSelected.durationInSeconds.ToString();
            level.text = effectSelected.level.ToString();
        }
    }
}
