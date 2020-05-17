using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FullSerializer;
using Games.Global.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor
{
    public enum Panel
    {
        Effect,
        Spell,
        SpellComponent
    }
    
    public class NavBar : MonoBehaviour
    {
        [SerializeField] private SpellPanel spellPanelScript;
        [SerializeField] private Button exportSpells;
        
        [SerializeField] private Button createSpellButton;
        [SerializeField] private Button createSpellComponentButton;
        [SerializeField] private Button createSpellEffectButton;

        [SerializeField] private GameObject spellPanel;
        [SerializeField] private GameObject spellComponentPanel;
        [SerializeField] private GameObject effectPanel;

        private void Start()
        {
            spellPanel.SetActive(false);
            spellComponentPanel.SetActive(false);
            effectPanel.SetActive(false);

            createSpellButton.onClick.AddListener(delegate { SwitchPanel(Panel.Spell); });
            createSpellComponentButton.onClick.AddListener(delegate { SwitchPanel(Panel.SpellComponent); });
            createSpellEffectButton.onClick.AddListener(delegate { SwitchPanel(Panel.Effect); });
            
            exportSpells.onClick.AddListener(ExportSpells);
        }

        private void SwitchPanel(Panel newPanel)
        {
            spellPanel.SetActive(false);
            spellComponentPanel.SetActive(false);
            effectPanel.SetActive(false);

            switch (newPanel)
            {
                case Panel.Effect:
                    effectPanel.SetActive(true);
                    break;
                case Panel.Spell:
                    spellPanelScript.InitSpellPanel();
                    spellPanel.SetActive(true);
                    break;
                case Panel.SpellComponent:
                    spellComponentPanel.SetActive(true);
                    break;
            }
        }

        private void ExportSpells()
        {
            foreach (KeyValuePair<string, Spell> valuePair in ListCreatedElement.Spell)
            {
                fsSerializer serializer = new fsSerializer();
                serializer.TrySerialize(valuePair.Value.GetType(), valuePair.Value, out fsData data);
                File.WriteAllText(Application.dataPath + "/Data/SpellsJson/" + valuePair.Key + ".json", fsJsonPrinter.CompressedJson(data));
            }
        }
    }
}
