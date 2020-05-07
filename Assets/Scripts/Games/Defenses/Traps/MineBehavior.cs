using UnityEngine;

namespace Games.Defenses.Traps {
  public class MineBehavior : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
        Debug.Log("explosion");
        gameObject.SetActive(false);
      }
    }
  }
}
