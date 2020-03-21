using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [Header("Tooltip Management")]
    [SerializeField] private GameObject tooltip;
    [SerializeField] private RectTransform tooltipRect;
    private Text tooltipText;

    /// <summary>
    /// A reference to all the keybind buttons on the menu
    /// </summary>
    private GameObject[] keybindButtons;

    private void Awake()
    {
        tooltipText = tooltip.GetComponentInChildren<Text>();
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    /// <summary>
    /// Shows the tooltip
    /// </summary>
    public void ShowTooltip(Resource resource, Vector3 position)
    {
        tooltip.SetActive(true);
        tooltipText.text = resource.ResourceName + "\n" + resource.GetDescription();

        Vector3 mousePosition = position;
        Vector2 pivot = new Vector2(
            mousePosition.x > Screen.width / 2.0f ? 1f : 0f,
            mousePosition.y > Screen.height / 2.0f ? 1f : 0f);
        tooltipRect.pivot = pivot;

        Vector2 sizeDelta = tooltipRect.sizeDelta;
        tooltipRect.transform.position = new Vector2(
            mousePosition.x > Screen.width / 2.0f
                ? mousePosition.x - sizeDelta.x / 4
                : mousePosition.x + sizeDelta.x / 4,
            mousePosition.y > Screen.height / 2.0f
                ? mousePosition.y - sizeDelta.y / 4
                : mousePosition.y + sizeDelta.x / 4);
    }

    /// <summary>
    /// Hides the tooltip
    /// </summary>
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    /// <summary>
    /// Updates the text on a keybind button after the key has been changed
    /// </summary>
    /// <param name="key"></param>
    /// <param name="code"></param>
    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }
}