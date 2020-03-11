using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform arrowParent;
    [SerializeField] ResourceSlot[] resourceSlots;
    [SerializeField] BaseItemSlot[] itemSlots;

    [Header("Public Variables")]
    public AccountManager AccountManager;

    private CraftingRecipe craftingRecipe;
    public CraftingRecipe CraftingRecipe
    {
        get
        {
            return craftingRecipe;
        }
        set
        {
            SetCraftingRecipe(value);
        }
    }

    public event Action<ResourceSlot> ResourceOnPointerEnterEvent;
    public event Action<ResourceSlot> ResourceOnPointerExitEvent;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;

    private void OnValidate()
    {
        resourceSlots = GetComponentsInChildren<ResourceSlot>(includeInactive: true);
        itemSlots = GetComponentsInChildren<BaseItemSlot>(includeInactive: true);
    }

    private void Start()
    {
        foreach (ResourceSlot resourceSlot in resourceSlots)
        {
            resourceSlot.OnPointerEnterEvent += slot => ResourceOnPointerEnterEvent(slot);
            resourceSlot.OnPointerExitEvent += slot => ResourceOnPointerExitEvent(slot);
        }

        foreach (BaseItemSlot itemSlot in itemSlots)
        {
            itemSlot.OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
            itemSlot.OnPointerExitEvent += slot => OnPointerExitEvent(slot);
        }
    }

    public void OnCraftButtonClick()
    {
        // Verify that we have a recipe and accountmanager data
        if (craftingRecipe != null && AccountManager != null)
        {
            craftingRecipe.Craft(AccountManager);
        }
    }

    private void SetCraftingRecipe(CraftingRecipe newCraftingRecipe)
    {
        craftingRecipe = newCraftingRecipe;

        if (craftingRecipe != null)
        {
            int resourceSlotIndex = 0;
            int itemSlotIndex = 0;

            resourceSlotIndex = SetResourceSlots(craftingRecipe.RecipeResources, resourceSlotIndex);
            arrowParent.SetSiblingIndex(resourceSlotIndex);
            itemSlotIndex = SetItemSlots(craftingRecipe.RecipeResults, itemSlotIndex);

            for (int i = resourceSlotIndex; i < resourceSlots.Length; i++)
            {
                resourceSlots[i].transform.parent.gameObject.SetActive(false);
            }

            for (int i = itemSlotIndex; i < itemSlots.Length; i++)
            {
                itemSlots[i].transform.parent.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private int SetResourceSlots(IList<RecipeResource> resourceAmountList, int resourceSlotIndex)
    {
        for (int i = 0; i < resourceAmountList.Count; i++, resourceSlotIndex++)
        {
            RecipeResource resourceAmount = resourceAmountList[i];
            ResourceSlot resourceSlot = resourceSlots[resourceSlotIndex];

            resourceSlot.Resource = resourceAmount.Resource;
            resourceSlot.Amount = resourceAmount.Amount;
            resourceSlot.transform.parent.gameObject.SetActive(true);
        }

        return resourceSlotIndex;
    }

    private int SetItemSlots(IList<RecipeResult> itemAmountList, int itemSlotIndex)
    {
        for (int i = 0; i < itemAmountList.Count; i++, itemSlotIndex++)
        {
            RecipeResult itemAmount = itemAmountList[i];
            BaseItemSlot itemSlot = itemSlots[itemSlotIndex];

            itemSlot.Item = itemAmount.Item;
            itemSlot.Amount = itemAmount.Amount;
            itemSlot.transform.parent.gameObject.SetActive(true);
        }

        return itemSlotIndex;
    }
}