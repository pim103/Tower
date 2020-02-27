using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    // Title of the item i want to craft
    [SerializeField]
    private Text title;

    // Description of what i will craft
    [SerializeField]
    private Text description;

    [SerializeField]
    private GameObject materialPrefab;

    [SerializeField]
    private Transform parent;

    private List<GameObject> materials = new List<GameObject>();

    private List<int> amounts = new List<int>();

    // The selected recipe
    [SerializeField]
    private Recipe selectedRecipe;

    // Number of items i can craft
    [SerializeField]
    private Text countTxt;

    // Info about the craft item
    [SerializeField]
    private ItemInfo craftItemInfo;

    // Amount of maximum item i want to craft
    private int maxAmount;

    // Amount of item i want to craft
    private int amount;

    private int MyAmount
    {
        set
        {
            countTxt.text = value.ToString();
            amount = value;
        }
        get
        {
            return amount;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        //InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateMaterialCount);
        ShowDescription(selectedRecipe);
    }

    public void ShowDescription(Recipe recipe)
    {
        if (selectedRecipe != null)
        {
            // Deselect any selected recipe
            selectedRecipe.Deselect();
        }

        this.selectedRecipe = recipe;

        this.selectedRecipe.Select();

        // Clear the materials list
        foreach (GameObject gameObject in materials)
        {
            Destroy(gameObject);
        }
        materials.Clear();

        // Title of the craft
        title.text = recipe.Output.MyTitle;

        // Description of the craft
        description.text = recipe.MyDescription + " " + recipe.Output.MyTitle.ToLower();

        craftItemInfo.Initialize(recipe.Output, 1);

        // Foreach materials we need
        foreach (CraftingMaterial material in recipe.Materials)
        {
            GameObject tmp = Instantiate(materialPrefab, parent);

            tmp.GetComponent<ItemInfo>().Initialize(material.MyItem, material.MyCount);

            materials.Add(tmp);
        }

        UpdateMaterialCount(null);
    }

    private void UpdateMaterialCount(Item item)
    {
        amounts.Sort();

        foreach (GameObject material in materials)
        {
            ItemInfo tmp = material.GetComponent<ItemInfo>();
            tmp.UpdateStackCount();
        }

        if (CanCraft())
        {
            maxAmount = amounts[0];

            if (countTxt.text == "0")
            {

                MyAmount = 1;

            }
            else if (int.Parse(countTxt.text) > maxAmount)
            {
                MyAmount = maxAmount;
            }
        }
        else
        {
            MyAmount = 0;
            maxAmount = 0;
        }
    }

    public void Crafting(bool all)
    {
        if (CanCraft())
        {
            if (all)
            {
                amounts.Sort();
                countTxt.text = maxAmount.ToString();
                //StartCoroutine(CraftRoutine(amounts[0]));
            }
            else
            {
                //StartCoroutine(CraftRoutine(MyAmount));
            }
        }
    }

    private bool CanCraft()
    {
        bool canCraft = true;

        amounts = new List<int>();

        // Check if we have enought materials for craft this item
        foreach (CraftingMaterial material in selectedRecipe.Materials)
        {
            // Count of the materials i have on my inventory
            //int count = InventoryScript.MyInstance.GetItemCount(material.MyItem.MyTitle);
            int count = AccountManager.MyInstance.MyGoldBar;

            if (count >= material.MyCount)
            {
                amounts.Add(count / material.MyCount);
                continue;
            }
            else
            {
                canCraft = false;
                break;
            }
        }

        return canCraft;
    }

    public void ChangeAmount(int i)
    {
        if ((amount + i) > 0 && amount + i <= maxAmount)
        {
            MyAmount += i;
        }
    }

    /*private IEnumerator CraftRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return AccountManager.MyInstance.MyInitRoutine = StartCoroutine(AccountManager.MyInstance.CraftRoutine(selectedRecipe));
        }
    }*/

    public void AdddItemsToInventory()
    {
        // Add craft item to my account inventory
    }
}
