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
        }
        
        private void Awake()
        {
            StartCoroutine(TimerBeforeDisapear());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Wall"))
            {
                weaponOrigin.TouchEntity(other);
            }

            rigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
