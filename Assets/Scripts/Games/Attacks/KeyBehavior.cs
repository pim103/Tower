using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Games.Attacks
{
    public class KeyBehavior : MonoBehaviour
    {
        [SerializeField] private ObjectsInScene objectsInScene;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                objectsInScene.endZone.SetActive(true);
                objectsInScene.endDoor.SetActive(false);

                if (objectsInScene.endFx != null)
                {
                    objectsInScene.endFx.SetActive(true);
                }
            }
        }
    }
}