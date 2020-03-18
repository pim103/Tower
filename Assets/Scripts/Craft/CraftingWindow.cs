﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CraftingRecipeUI recipeUIPrefab;
    [SerializeField] RectTransform recipeUIParent;
    [SerializeField] List<CraftingRecipeUI> craftingRecipeUIs;

    [Header("Public variables")]
    public AccountManager AccountManager;
    public List<CraftingRecipe> CraftingRecipes;

    public event Action<ResourceSlot> ResourceOnPointerEnterEvent;
    public event Action<ResourceSlot> ResourceOnPointerExitEvent;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;

    private void OnValidate()
    {
        Init();
    }

    private void Start()
    {
        Init();

        foreach (CraftingRecipeUI craftingRecipeUI in craftingRecipeUIs)
        {
            craftingRecipeUI.ResourceOnPointerEnterEvent += slot =>
            {
                ResourceOnPointerEnterEvent?.Invoke(slot);
            };
            craftingRecipeUI.ResourceOnPointerExitEvent += slot =>
            {
                ResourceOnPointerExitEvent?.Invoke(slot);
            };

            craftingRecipeUI.OnPointerEnterEvent += slot =>
            {
                OnPointerEnterEvent?.Invoke(slot);
            };
            craftingRecipeUI.OnPointerExitEvent += slot =>
            {
                OnPointerExitEvent?.Invoke(slot);
            };
        }
    }

    private void Init()
    {
        recipeUIParent.GetComponentsInChildren<CraftingRecipeUI>(includeInactive: true, result: craftingRecipeUIs);
        UpdateCraftingRecipes();
    }

    public void UpdateCraftingRecipes()
    {
        for (int i = 0; i < CraftingRecipes.Count; i++)
        {
            if (craftingRecipeUIs.Count == i)
            {
                craftingRecipeUIs.Add(Instantiate(recipeUIPrefab, recipeUIParent, false));
            }
            else if(craftingRecipeUIs[i] == null)
            {
                craftingRecipeUIs[i] = Instantiate(recipeUIPrefab, recipeUIParent, false);
            }

            craftingRecipeUIs[i].AccountManager = AccountManager;
            craftingRecipeUIs[i].CraftingRecipe = CraftingRecipes[i];
        }

        for (int i = CraftingRecipes.Count; i < craftingRecipeUIs.Count; i++)
        {
            craftingRecipeUIs[i].CraftingRecipe = null;
        }
    }
}