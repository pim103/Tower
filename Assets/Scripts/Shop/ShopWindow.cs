using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the ShopWindow script, it contains functionality that is specific to
/// the shop window
/// </summary>
public class ShopWindow : MonoBehaviour {
  [Header("References")]
  [SerializeField]
  private ShopButton[] shopButtons;
  [SerializeField]
  private Text pageNumber;
  [SerializeField]
  private ShopManager shopManager;

  private List<List<ShopItem>>pages = new List<List<ShopItem>>();
  private int pageIndex;

  void Start() { CreatePages(shopManager.MyItems); }

  public void CreatePages(ShopItem[] items) {
    pages.Clear();

    List<ShopItem>page = new List<ShopItem>();

    for (int i = 0; i < items.Length; i++) {
      page.Add(items[i]);

      if (page.Count == 12 || i == items.Length - 1) {
        pages.Add(page);
        page = new List<ShopItem>();
      }
    }

    AddItems();
  }

  public void AddItems() {
    pageNumber.text = pageIndex + 1 + "/" + pages.Count;

    if (pages.Count > 0) {
      for (int i = 0; i < pages[pageIndex].Count; i++) {
        if (pages [pageIndex]
            [i] != null) {
          shopButtons[i].AddItem(pages [pageIndex]
                                 [i]);
        }
      }
    }
  }

  public void NextPage() {
    if (pageIndex < pages.Count - 1) {
      ClearButtons();
      pageIndex++;
      AddItems();
    }
  }

  public void PreviousPage() {
    if (pageIndex > 0) {
      ClearButtons();
      pageIndex--;
      AddItems();
    }
  }

  public void ClearButtons() {
    foreach(ShopButton btn in shopButtons) { btn.gameObject.SetActive(false); }
  }
}