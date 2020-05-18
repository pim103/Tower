using System;
using System.Collections;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Spells;
using SpellEditor;
using SpellEditor.PanelUtils;
using UnityEngine;
using UnityEngine.UI;

public class WavePanel : MonoBehaviour
{
    [SerializeField] private Dropdown geometry;
    [SerializeField] private InputField initialWidth;
    [SerializeField] private InputField duration;
    [SerializeField] private InputField damages;
    [SerializeField] private InputField incrementAmplitudeByTime;
    [SerializeField] private InputField speedPropagation;
    [SerializeField] private DropdownMultiSelector effectsOnHit;

    public void InitWavePanel()
    {
        string[] enumNames = Enum.GetNames(typeof(Geometry));
        List<string> listNames = new List<string>(enumNames);
        geometry.ClearOptions();
        geometry.AddOptions(listNames);
        effectsOnHit.InitDropdownMultiSelect();
    }

    public void ResetCurrentPanel()
    {
        geometry.value = 0;
        initialWidth.text = "";
        duration.text = "";
        damages.text = "";
        incrementAmplitudeByTime.text = "";
        speedPropagation.text = "";
    }

    public WaveSpell SaveCurrentPanel()
    {
        if (effectsOnHit.selectedIndex.Count == 0 && damages.text == "")
        {
            return null;
        }
        
        List<Effect> effectsOnHitList = new List<Effect>();
        
        foreach (var effect in ListCreatedElement.Effects)
        {
            if (effectsOnHit.selectedIndex.Contains(effect.Key))
            {
                effectsOnHitList.Add(effect.Value);
            }
        }

        WaveSpell newWaveSpell = new WaveSpell
        {
            geometryPropagation = (Geometry) geometry.value,
            duration = duration.text == "" ? 1 : Int32.Parse(duration.text),
            initialWidth = initialWidth.text == "" ? 1 : Int32.Parse(initialWidth.text),
            damages = damages.text == "" ? 0 : Int32.Parse(damages.text),
            incrementAmplitudeByTime = incrementAmplitudeByTime.text == "" ? 0.1f : Int32.Parse(incrementAmplitudeByTime.text),
            speedPropagation = speedPropagation.text == "" ? 0.1f : Int32.Parse(speedPropagation.text),
            effectsOnHit = effectsOnHitList
        };

        ResetCurrentPanel();
        return newWaveSpell;
    }
}
