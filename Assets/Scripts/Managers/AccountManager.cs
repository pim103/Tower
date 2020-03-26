using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Resources on the account
[Serializable]
public struct AccountResource {
  // The resource
  public Resource Resource;

  // The amount of the resource
  [Range(0, 999)]
  public int Amount;
}

/// <summary>
/// This is the account script, it contains functionality that is specific to
/// the account
/// </summary>
public class AccountManager : MonoBehaviour {
  private static AccountManager instance;
  public static AccountManager MyInstance{
      get{if(instance == null){instance = FindObjectOfType<AccountManager>();
}

return instance;
}
}

[Header("Account")]
    // Resources on the account
    public List<AccountResource>AccountResources;

[Header("Public Variables")]
[SerializeField]
private Text goldBarAmountText;
[SerializeField]
private Text goldNuggetAmountText;
[SerializeField]
private Text stoneOreAmountText;

private void Update() { UpdateResources(); }

// Update account resources
private void UpdateResources() {
  goldBarAmountText.text = AccountResources[0].Amount.ToString();
  goldNuggetAmountText.text = AccountResources[1].Amount.ToString();
  stoneOreAmountText.text = AccountResources[2].Amount.ToString();
}

// Count the number of a resource on the account
public int ResourceCount(string resourceID) {
  int number = 0;

  for (int i = 0; i < AccountResources.Count; i++) {
    Resource resource = AccountResources[i].Resource;
    if (resource != null && resource.ID == resourceID) {
      number += AccountResources[i].Amount;
    }
  }

  return number;
}

// Remove a resource on the account
public void RemoveResource(string resourceID) {
  for (int i = 0; i < AccountResources.Count; i++) {
    Resource resource = AccountResources[i].Resource;
    if (resource != null && resource.ID == resourceID) {
      AccountResource tempResource = AccountResources[i];
      tempResource.Amount--;
      AccountResources[i] = tempResource;
    }
  }
}

// Add item to the account
public void AddItem() {}
}