using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the UIManager script, it contains functionality that is specific to the UI
/// </summary>
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

    private void Awake()
    {
        tooltipText = tooltip.GetComponentInChildren<Text>();
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
}