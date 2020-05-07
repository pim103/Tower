using System;
using System.Collections;
using UnityEngine;

namespace Games.Defenses {
  public class SpikesBehavior : MonoBehaviour {
    [SerializeField]
    private GameObject spikes;
    private bool isActive;
    private bool playerOnSpikes;
    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
        playerOnSpikes = true;
      }
    }

    private void OnTriggerExit(Collider other) {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
        playerOnSpikes = false;
      }
    }

    private void Update() {
      if (playerOnSpikes && !isActive) {
        StartCoroutine(GoUp());
      }
    }

    private IEnumerator GoUp() {
      isActive = true;
      int trapTimer = 0;
      var spikesPosition = spikes.transform.position;
      spikesPosition =
          new Vector3(spikesPosition.x, spikesPosition.y + 1, spikesPosition.z);
      spikes.transform.position = spikesPosition;
      while (trapTimer < 3) {
        yield return new WaitForSeconds(1f);
        trapTimer += 1;
      }
      spikesPosition = spikes.transform.position;
      spikesPosition =
          new Vector3(spikesPosition.x, spikesPosition.y - 1, spikesPosition.z);
      spikes.transform.position = spikesPosition;
      yield return new WaitForSeconds(1f);
      isActive = false;
    }
  }
}
