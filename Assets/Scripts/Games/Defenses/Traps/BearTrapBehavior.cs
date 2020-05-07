using System;
using System.Collections;
using UnityEngine;

namespace Games.Defenses {
  public class BearTrapBehavior : MonoBehaviour {
    [SerializeField]
    private GameObject inactiveState;
    [SerializeField]
    private GameObject activeState;
    private bool hasBeenTriggered;
    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
          !hasBeenTriggered) {
        hasBeenTriggered = true;
        inactiveState.SetActive(false);
        activeState.SetActive(true);
      }
    }
  }
}
