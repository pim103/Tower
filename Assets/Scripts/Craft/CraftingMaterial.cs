using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingMaterial
{
    // What item that will craft
    [SerializeField]
    private Item item;

    // How many item that will craft
    [SerializeField]
    private int count;

    public int MyCount
    {
        get
        {
            return count;
        }
    }

    public Item MyItem
    {
        get
        {
            return item;
        }
    }
}
