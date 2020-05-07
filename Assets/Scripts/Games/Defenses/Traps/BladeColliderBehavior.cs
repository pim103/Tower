using UnityEngine;

namespace Games.Defenses.Traps {
  public class BladeColliderBehavior : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
        Debug.Log("hit");
      }
    }
  }
}
