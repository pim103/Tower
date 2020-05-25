﻿using System;
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

    [Header("Account Management")]
    // Display text of the quantity of resources in the main menu
    [SerializeField] private Text goldBarAmountText;
    [SerializeField] private Text goldNuggetAmountText;
    [SerializeField] private Text stoneOreAmountText;
    // Display text of the amount of money
    [SerializeField] private Text moneyAmountText;

    [Header("Game Settings Management")]
    public Dropdown resolutionDropdown;
    public Dropdown graphicQualityLevelDropdown;
    public Toggle fullScreenToggle;
    public Slider volumeSlider;

    [Header("Key Bind Management")]
    public Text up;
    public Text left;
    public Text down;
    public Text right;
    public Text action_1;
    public Text action_2;
    public Text spell_1;
    public Text spell_2;
    public Text spell_3;

    [Header("Chat Box Management")]
    public GameObject chatPanel;
    public GameObject textObject;
    public InputField chatBox;

    [Header("Social Management")]
    public GameObject SocialPanel;
    public GameObject SocialBoxContent;
    public GameObject usernameObject;
    public GameObject AddSocialPanel;
    public InputField AddSocialPanelInput;

    private void Awake()
    {
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    void Update()
    {
        // Account Management
        goldBarAmountText.text = AccountManager.MyInstance.AccountResources[0].Amount.ToString();
        goldNuggetAmountText.text = AccountManager.MyInstance.AccountResources[1].Amount.ToString();
        stoneOreAmountText.text = AccountManager.MyInstance.AccountResources[2].Amount.ToString();
        moneyAmountText.text = AccountManager.MyInstance.AccountMoney.ToString();
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