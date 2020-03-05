using System;
using UnityEngine;

namespace Games.Global.Weapons
{
    public class ProjectilesPrefab : MonoBehaviour
    {
        public WeaponPrefab weaponOrigin;
        [SerializeField] public Rigidbody rigidbody;

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
