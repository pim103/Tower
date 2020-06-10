using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is the ShopInfo script, it contains functionality that is specific to
/// the shop info
/// </summary>
public class ShopInfo : MonoBehaviour,
                        IPointerEnterHandler,
                        IPointerExitHandler {
  [Header("References")]
  [SerializeField]
  private ShopContainer shopContainer;

  public void OnPointerEnter(PointerEventData eventData) {
    RectTransform container = (RectTransform) transform;
    UIManager.MyInstance.ShowTooltip(shopContainer.shopItem.Resource,
                                     transform.position, container);
  }

  public void OnPointerExit(PointerEventData eventData) {
    UIManager.MyInstance.HideTooltip();
  }
}