﻿using System;
using UnityEngine;

namespace Games.Attacks {
  public class KeyBehavior : MonoBehaviour {
    [SerializeField]
    private GameObject doorPivot;
    [SerializeField]
    private GameObject endCube;

    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
        // doorPivot.transform.eulerAngles = new
        // Vector3(doorPivot.transform.eulerAngles.x, -90,
        // doorPivot.transform.eulerAngles.z);
        doorPivot.SetActive(false);
        endCube.SetActive(true);
      }
    }
  }
}
