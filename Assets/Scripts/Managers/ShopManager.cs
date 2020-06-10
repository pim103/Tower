using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the ShopManager script, it contains functionality that is specific
/// to the shop
/// </summary>
public class ShopManager : MonoBehaviour {
  [Header("New Shop items list")]
      // List of items on the shop
      [SerializeField]
      private ShopItem[] newShopItems;

  [Header("Top Shop items list")]
      // List of items on the shop
      [SerializeField]
      private ShopItem[] topShopItems;

  [Header("Ressource Shop items list")]
      // List of items on the shop
      [SerializeField]
      private ShopItem[] ressourceShopItems;

  public ShopItem[] MyNewItems {
    get { return newShopItems; }
  }

  public ShopItem[] MyTopItems {
    get { return topShopItems; }
  }

  public ShopItem[] MyRessourceItems {
    get { return ressourceShopItems; }
  }
}