using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour {
  public static Managers instance;

  private void Awake() {
    if (instance != null) {
      // Destroy(gameObject);
      Destroy(Managers.instance.gameObject);
    }

    instance = this;
    DontDestroyOnLoad(gameObject);
  }
}