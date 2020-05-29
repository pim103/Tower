using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the ShopManager script, it contains functionality that is specific to the shop
/// </summary>
public class ShopManager : MonoBehaviour
{
    [Header("New Shop items list")]
    // List of items on the shop
    [SerializeField] private ShopItem[] shopItems;

    [Header("Top Shop items list")]
    // List of items on the shop
    [SerializeField] private ShopItem[] topShopItems;

    public ShopItem[] MyItems
    {
        get
        {
            return shopItems;
        }
    }

    public ShopItem[] MyTopItems
    {
        get
        {
            return topShopItems;
        }
    }
}