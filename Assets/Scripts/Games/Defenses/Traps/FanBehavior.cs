using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Defenses.Traps
{
public class FanBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject blades;

    private List<Rigidbody> pushedObjects;

    private void Start()
    {
        pushedObjects = new List<Rigidbody>();
    }

    private void FixedUpdate()
    {
        blades.transform.Rotate (0,-420*Time.deltaTime,0);

        foreach (var pushedObject in pushedObjects)
        {
            pushedObject.AddForce(transform.forward*350,ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            pushedObjects.Add(other.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            pushedObjects.Remove(other.GetComponent<Rigidbody>());
        }
    }
}
}
