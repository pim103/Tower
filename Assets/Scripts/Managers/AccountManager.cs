using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Resources on the account
[Serializable]
public struct AccountResource
{
    // The resource
    public Resource Resource;

    // The amount of the resource
    [Range(0, 999)]
    public int Amount;
}

/// <summary>
/// This is the AccountManager script, it contains functionality that is specific to the account
/// </summary>
public class AccountManager : MonoBehaviour
{
    private static AccountManager instance;
    public static AccountManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AccountManager>();
            }

            return instance;
        }
    }

    [Header("Account")]
    // List of resources on the account
    public List<AccountResource> AccountResources;
    // Amount of money on the account
    public int AccountMoney;

    /// <summary>
    /// Count the number of a resource on the account
    /// </summary>
    public int ResourceCount(string resourceID)
    {
        int number = 0;

        for (int i = 0; i < AccountResources.Count; i++)
        {
            Resource resource = AccountResources[i].Resource;
            if (resource != null && resource.ID == resourceID)
            {
                number += AccountResources[i].Amount;
            }
        }

        return number;
    }

    /// <summary>
    /// Remove a resource on the account
    /// </summary>
    public void RemoveResource(string resourceID)
    {
        for (int i = 0; i < AccountResources.Count; i++)
        {
            Resource resource = AccountResources[i].Resource;
            if (resource != null && resource.ID == resourceID)
            {
                AccountResource tempResource = AccountResources[i];
                tempResource.Amount--;
                AccountResources[i] = tempResource;
            }
        }
    }

    /// <summary>
    /// Add a resource on the account
    /// </summary>
    public void AddResource(string resourceID)
    {
        for (int i = 0; i < AccountResources.Count; i++)
        {
            Resource resource = AccountResources[i].Resource;
            if (resource != null && resource.ID == resourceID)
            {
                AccountResource tempResource = AccountResources[i];
                tempResource.Amount++;
                AccountResources[i] = tempResource;
            }
        }
    }

    /// <summary>
    /// Add an item on the account
    /// </summary>
    public void AddItem()
    {

    }
}