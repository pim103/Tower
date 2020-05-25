#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor.PanelUtils
{
    public class ShowPanel : MonoBehaviour
    {
        [SerializeField] private Button close;
        [SerializeField] private Text text;

        private void Start()
        {
            close.onClick.AddListener(delegate { gameObject.SetActive(false); });
        }

        public void FillData(List<string> listIndex)
        {
            string newText = "";

            foreach (string index in listIndex)
            {
                newText += index + "\n";
            }

            text.text = newText;
        }
    }
}
#endif