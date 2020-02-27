using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    // All materials needed for craft the recipe
    [SerializeField]
    private CraftingMaterial[] materials;

    // What the recipe will be create
    [SerializeField]
    private Item output;

    // How many of this item will be create
    [SerializeField]
    private int outputCount;

    // Recipe description, what it do
    [SerializeField]
    private string description;

    // Highlight the field on the recipe list when selected
    [SerializeField]
    private Image highlight;

    // Check if we have the materials
    public CraftingMaterial[] Materials
    {
        get
        {
            return materials;
        }
    }

    public Item Output
    {
        get
        {
            return output;
        }
    }

    public int OutputCount
    {
        get
        {
            return outputCount;
        }

        set
        {
            outputCount = value;
        }
    }

    public string MyDescription
    {
        get
        {
            return description;
        }
    }

    public string MyTitle
    {
        get
        {
            return output.MyTitle;
        }
    }

    public Sprite MyIcon
    {
        get
        {
            return output.MyIcon;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = output.MyTitle;
    }

    public void Select()
    {
        Color c = highlight.color;
        c.a = .3f;
        highlight.color = c;
    }

    public void Deselect()
    {
        Color c = highlight.color;
        c.a = 0f;
        highlight.color = c;
    }
}
