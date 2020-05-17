using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the ShopManager script, it contains functionality that is specific to the shop
/// </summary>
public class ShopManager : MonoBehaviour
{
    [Header("Shop items list")]
    // List of items on the shop
    [SerializeField] private ShopItem[] shopItems;

    public ShopItem[] MyItems
    {
        get
        {
            return shopItems;
        }
    }
}