using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the ShopButton script, it contains functionality that is specific to the shop button
/// </summary>
public class ShopContainer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShopWindow shopWindow;
    [SerializeField] private Image icon;
    [SerializeField] private Text ressourceName;
    [SerializeField] private Text amount;
    [SerializeField] private Text price;

    [HideInInspector]
    public ShopItem shopItem;
    private static ShopItem currentShopItem;

    public void AddItem(ShopItem shopItem)
    {
        this.shopItem = shopItem;

        icon.sprite = shopItem.Resource.Icon;

        ressourceName.text = shopItem.Resource.ResourceName;

        amount.text = shopItem.Amount.ToString();

        if (shopItem.Price > 0)
        {
            price.text = "Prix : " + shopItem.Price.ToString() + "€";
        }
        else
        {
            price.text = string.Empty;
        }

        gameObject.SetActive(true);
    }

    /// <summary>
    /// User want to buy something
    /// </summary>
    public void UserWantToBuy()
    {
        // Check if user have the money
        if (AccountManager.MyInstance.AccountMoney >= shopItem.Price)
        {
            currentShopItem = shopItem;
            //foreach (var item in currentShopItem.GetType().GetFields())
            //{
            //    Debug.Log(item + " : " + item.GetValue(currentShopItem));
            //}

            if (shopWindow.ConfirmePurchasePanel != null)
            {
                shopWindow.ConfirmePurchasePanel.SetActive(true);
                shopWindow.PurchaseDescription.text = currentShopItem.Amount.ToString() + " " + currentShopItem.Resource.ResourceName + " pour " + currentShopItem.Price.ToString() + "€";
            }
        }
        else
        {
            if (shopWindow.ErrorPurchasePanel != null)
            {
                shopWindow.ErrorPurchasePanel.SetActive(true);
            }
        }
    }

    /// <summary>
    /// User buy something
    /// </summary>
    public void Buy()
    {
        // Remove the money from the user account
        AccountManager.MyInstance.AccountMoney -= currentShopItem.Price;

        if (shopWindow.ConfirmationPurchasePanel != null)
        {
            shopWindow.ConfirmationPurchasePanel.SetActive(true);
        }

        // Add achievment
        if (AchievmentManager.Instance.initAchievment == true)
        {
            AchievmentManager.Instance.EarnAchievment("Premier achat");
            AchievmentManager.Instance.EarnAchievment("Commerçant");
        }

        // Add the item to the user account
        if (currentShopItem.Resource is Resource resource)
        {
            Debug.Log("C'est une ressource.");
            for (int i = 0; i < currentShopItem.Amount; i++)
            {
                AccountManager.MyInstance.AddResource(currentShopItem.Resource.ID);  
            }
        }
        else
        {
            Debug.Log("Ce n'est pas une ressource.");
        }
    }
}