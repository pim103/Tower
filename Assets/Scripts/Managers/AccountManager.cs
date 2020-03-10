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
    [Range(1, 999)]
    public int Amount;
}

/// <summary>
/// This is the account script, it contains functionality that is specific to the account
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

    // Resources on the account
    public List<AccountResource> AccountResources;

    // Count the number of a resource on the account
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

    // Remove a resource on the account
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
}