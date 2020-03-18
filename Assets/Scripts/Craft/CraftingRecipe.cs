using System;
using System.Collections.Generic;
using UnityEngine;

// What we need for crafting
[Serializable]
public struct RecipeResource
{
    // The Resource
    public Resource Resource;

    // The amount of the Resource
    [Range(1, 999)]
    public int Amount;
}

// What crafting will result
[Serializable]
public struct RecipeResult
{
    // The item
    public Item Item;

    // The amount of the item
    [Range(1, 999)]
    public int Amount;
}

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    [Header("Resources")]
    // Resources required for the recipe
    public List<RecipeResource> RecipeResources;

    [Header("Result")]
    // What the recipe will give
    public List<RecipeResult> RecipeResults;

    // Craft function
    public void Craft(AccountManager accountManager)
    {
        // Verify that we can craft this recipe
        if (CanCraft(accountManager))
        {
            // Remove resources we used for craft this recipe from the account
            RemoveResources(accountManager);

            // Add the recipe result to the account
            AddResults(accountManager);

            Debug.Log("Craft success");
        }
        else
        {
            Debug.Log("Craft faillure");
        }
    }

    // Verify that we can craft this recipe
    public bool CanCraft(AccountManager accountManager)
    {
        // Verify if the account have the required resources for the craft
        return HasResources(accountManager);
    }

    // Verify if the account have the required resources for the craft
    private bool HasResources(AccountManager accountManager)
    {
        foreach (RecipeResource recipeResource in RecipeResources)
        {
            if (accountManager.ResourceCount(recipeResource.Resource.ID) < recipeResource.Amount)
            {
                Debug.LogWarning("You don't have the required resources.");
                return false;
            }
        }

        return true;
    }

    // Remove resources we used for craft this recipe from the account
    private void RemoveResources(AccountManager accountManager)
    {
        foreach (RecipeResource recipeResource in RecipeResources)
        {
            for (int i = 0; i < recipeResource.Amount; i++)
            {
                accountManager.RemoveResource(recipeResource.Resource.ID);
            }
        }
    }

    // Add the recipe result to the account
    private void AddResults(AccountManager accountManager)
    { 
        foreach (RecipeResult recipeResult in RecipeResults)
        {
            for (int i = 0; i < recipeResult.Amount; i++)
            {
                // TODO : Add the result to the account
            }
        }
    }
}