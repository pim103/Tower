using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the ShopWindow script, it contains functionality that is specific to the shop window
/// </summary>
public class ShopWindow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private Text pageNumber;
    public GameObject ConfirmePurchasePanel;
    public GameObject ConfirmationPurchasePanel;
    public GameObject ErrorPurchasePanel;
    public Text PurchaseDescription;
    [SerializeField] private bool newShopPage;
    [SerializeField] private bool topShopPage;
    [SerializeField] private bool ressourceShopPage;

    [Header("Shop Containers List")]
    [SerializeField] private ShopContainer[] shopContainers;

    private List<List<ShopItem>> pages = new List<List<ShopItem>>();
    private int pageIndex;

    void Start()
    {
        if (newShopPage)
        {
            CreatePages(shopManager.MyNewItems);
        }
        else if (topShopPage)
        {
            CreatePages(shopManager.MyTopItems);
        }
        else if (ressourceShopPage)
        {
            CreatePages(shopManager.MyRessourceItems);
        }
    }

    public void CreatePages(ShopItem[] items)
    {
        pages.Clear();

        List<ShopItem> page = new List<ShopItem>();

        for (int i = 0; i < items.Length; i++)
        {
            page.Add(items[i]);

            if (page.Count == 8 || i == items.Length - 1)
            {
                pages.Add(page);
                page = new List<ShopItem>();
            }
        }

        AddItems();
    }

    public void AddItems()
    {
        pageNumber.text = pageIndex + 1 + "/" + pages.Count;

        if (pages.Count > 0)
        {
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    shopContainers[i].AddItem(pages[pageIndex][i]);
                }
            }
        }
    }

    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            pageIndex++;
            StartCoroutine(SlideRightAnimation());
        } 
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            StartCoroutine(SlideLeftAnimation());
        }
    }

    private IEnumerator SlideLeftAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            bool isOpen = animator.GetBool("slideLeft");
            animator.SetBool("slideLeft", !isOpen);
        }

        yield return new WaitForSeconds(1);

        ClearButtons();
        AddItems();

        if (animator != null)
        {
            bool isOpen = animator.GetBool("slideLeft");
            animator.SetBool("slideLeft", !isOpen);
        }
    }

    private IEnumerator SlideRightAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            bool isOpen = animator.GetBool("slideRight");
            animator.SetBool("slideRight", !isOpen);
        }

        yield return new WaitForSeconds(1);

        ClearButtons(); 
        AddItems();

        if (animator != null)
        {
            bool isOpen = animator.GetBool("slideRight");
            animator.SetBool("slideRight", !isOpen);
        }
    }

    public void ClearButtons()
    {
        foreach (ShopContainer btn in shopContainers)
        {
            btn.gameObject.SetActive(false);
        }
    }
}