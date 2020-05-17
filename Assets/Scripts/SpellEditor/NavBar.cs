using System;
using System.Diagnostics;
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
                    spellPanel.SetActive(true);
                    break;
                case Panel.SpellComponent:
                    spellComponentPanel.SetActive(true);
                    break;
            }
        }
    }
}
