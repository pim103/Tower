using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the ShopItem script, it contains functionality that is specific to the shop item
/// </summary>
[System.Serializable]
public class ShopItem
{
    // The resource
    public Resource Resource;

    // The amount of the resource
    [Range(0, 999)]
    public int Amount;

    // The price of the resource
    [Range(0, 999)]
    public int Price;

    // The quantity available of the resource
    [Range(0, 999)]
    public int Quantity;

    // Unlimite the number of usage
    public bool Unlimited;
}