using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This is the ShopButton script, it contains functionality that is specific to the shop button
/// </summary>
public class ShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private Image icon;

    [SerializeField] private Text ressourceName;

    [SerializeField] private Text amount;

    [SerializeField] private Text price;

    public GameObject ConfirmationPurchasePanel;
    public GameObject ErrorPurchasePanel;

    private ShopItem shopItem;

    public void AddItem(ShopItem shopItem)
    {
        this.shopItem = shopItem;

        icon.sprite = shopItem.Resource.Icon;

        ressourceName.text = shopItem.Resource.ResourceName;

        amount.text = shopItem.Amount.ToString();

        if (shopItem.Price > 0)
        {
            price.text = "Prix: " + shopItem.Price.ToString() + "€";
        }
        else
        {
            price.text = string.Empty;
        }

        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if user have the money
        if (AccountManager.MyInstance.AccountMoney  >= shopItem.Price)
        {
            Buy();
        }
        else
        {
            if (ErrorPurchasePanel != null)
            {
                ErrorPurchasePanel.SetActive(true);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.MyInstance.ShowTooltip(shopItem.Resource, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

    /// <summary>
    /// User buy something
    /// </summary>
    private void Buy()
    {
        // Remove the money from the user account
        AccountManager.MyInstance.AccountMoney -= shopItem.Price;

        if (ConfirmationPurchasePanel != null)
        {
            ConfirmationPurchasePanel.SetActive(true);
        }

        // Add the item to the user account
        for (int i = 0; i < shopItem.Amount; i++)
        {
            AccountManager.MyInstance.AddResource(shopItem.Resource.ID);
        }  
    }
}