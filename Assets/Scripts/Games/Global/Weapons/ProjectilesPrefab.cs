using System;
using System.Collections;
using UnityEngine;

namespace Games.Global.Weapons
{
    public class ProjectilesPrefab : MonoBehaviour
    {
        public WeaponPrefab weaponOrigin;
        [SerializeField] public Rigidbody rigidbody;

        public IEnumerator TimerBeforeDisapear()
        {
            int timer = 5;
            while (timer > 0)
            {
                yield return new WaitForSeconds(1);
                timer--;
            }

            DesactivePrefab();
        }

        private void Awake()
        {
            StartCoroutine(TimerBeforeDisapear());
        }

        private void OnTriggerEnter(Collider other)
        {
            bool touch = true;
            
            if (other.gameObject.layer != LayerMask.NameToLayer("Wall"))
            {
                touch = weaponOrigin.TouchEntity(other);
            }

            if (touch)
            {
                DesactivePrefab();
            }
        }

        private void DesactivePrefab()
        {
            rigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
