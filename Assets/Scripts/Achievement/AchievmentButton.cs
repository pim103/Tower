using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the AchievmentButton script, it contains functionality that is
/// specific to the Achievment
/// </summary>
public class AchievmentButton : MonoBehaviour {
  public GameObject achievmentList;
  public Sprite neutral;
  public Sprite highlight;

  private Image sprite;

  void Awake() { sprite = GetComponent<Image>(); }

  public void Click() {
    if (sprite.sprite == neutral) {
      sprite.sprite = highlight;
      achievmentList.SetActive(true);
    } else {
      sprite.sprite = neutral;
      achievmentList.SetActive(false);
    }
  }
}