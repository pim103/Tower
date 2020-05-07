using System;
using System.Collections;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Defenses {
  public class ArrowTrapBehavior : MonoBehaviour {
    private GameObject target;
    private bool followTarget;
    private Coroutine projTimerCoroutine;

    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
        target = other.gameObject;
        followTarget = true;
        projTimerCoroutine = StartCoroutine(ProjTimer());
      }
    }

    private void OnTriggerExit(Collider other) {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
        StopCoroutine(projTimerCoroutine);
        followTarget = false;
        target = null;
      }
    }

    private void Update() {
      if (followTarget) {
        transform.LookAt(target.transform.position + Vector3.up);
        transform.Rotate(Vector3.right, -90);
      }
    }

    IEnumerator ProjTimer() {
      while (true) {
        PoolProjectiles();
        yield return new WaitForSeconds(.5f);
      }
    }

    private void PoolProjectiles() {
      GameObject proj = ObjectPooler.SharedInstance.GetPooledObject(0);

      proj.transform.position = transform.position;

      proj.transform.localEulerAngles =
          transform.eulerAngles /*+ (Vector3.right * rotX)*/;
      proj.SetActive(true);

      proj.GetComponent<Rigidbody>().AddForce(-transform.up * 1000,
                                              ForceMode.Acceleration);
    }
  }
}
