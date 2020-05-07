using System;
using System.Collections.Generic;
using Games.Players;
using UnityEngine;

namespace Games.Defenses.Traps {
  public class EjectionPlateBehavior : MonoBehaviour {
    [SerializeField]
    private GameObject bumper;

    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
        bumper.transform.Rotate(transform.right, 70);

        Vector3 dir = Quaternion.AngleAxis(45, transform.right) * transform.up;
        other.GetComponent<PlayerPrefab>().grounded = false;
        other.GetComponent<PlayerPrefab>().ejected = true;
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.GetComponent<Rigidbody>().AddForce(dir * 500);
      }
    }

    private void OnTriggerExit(Collider other) {
      bumper.transform.Rotate(transform.right, -70);
    }
  }
}
