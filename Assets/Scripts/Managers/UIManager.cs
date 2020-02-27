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

    [SerializeField]
    private GameObject tooltip;

    private Text tooltipText;

    [SerializeField]
    private RectTransform tooltipRect;

    private void Awake()
    {
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Updates the stacksize on a clickable slot
    /// </summary>
    /// <param name="clickable"></param>
    /*public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1) //If our slot has more than one item on it
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.enabled = true;
            clickable.MyIcon.enabled = true;
        }
        else //If it only has 1 item on it
        {
            clickable.MyStackText.enabled = false;
            clickable.MyIcon.enabled = true;
        }
        if (clickable.MyCount == 0) //If the slot is empty, then we need to hide the icon
        {
            clickable.MyStackText.enabled = false;
            clickable.MyIcon.enabled = false;
        }
    }*/

    /*public void ClearStackCount(IClickable clickable)
    {
        clickable.MyStackText.enabled = false;
        clickable.MyIcon.enabled = true;
    }*/

    /// <summary>
    /// Shows the tooltip
    /// </summary>
    public void ShowTooltip(Vector2 pivot, Vector3 position, IDescribable description)
    {
        tooltipRect.pivot = pivot;
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltipText.text = description.GetDescription();
    }

    /// <summary>
    /// Hides the tooltip
    /// </summary>
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void RefreshTooltip(IDescribable description)
    {
        tooltipText.text = description.GetDescription();
    }
}
