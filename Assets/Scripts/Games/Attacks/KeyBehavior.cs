using System;
using UnityEngine;

namespace Games.Attacks
{
    public class KeyBehavior : MonoBehaviour
    {
        [SerializeField] public GameObject doorPivot;
        [SerializeField] public GameObject endCube;
        [SerializeField] private ObjectsInScene objectsInScene;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //doorPivot.transform.eulerAngles = new Vector3(doorPivot.transform.eulerAngles.x, -90, doorPivot.transform.eulerAngles.z);
                objectsInScene.endZone.SetActive(true);
                objectsInScene.endDoor.SetActive(false);
            }
        }
    }
}
