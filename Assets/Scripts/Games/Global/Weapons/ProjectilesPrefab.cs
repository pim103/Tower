using System.Collections;
using Games.Global.Entities;
using Games.Players;
using UnityEngine;

namespace Games.Global.Weapons
{
    public enum TypeProjectile
    {
        Arrow,
        Grenade
    }
    
    public class ProjectilesPrefab : MonoBehaviour
    {
        // Use when we havn't weapon
        public Entity origin;
        
        public WeaponPrefab weaponOrigin;
        [SerializeField] public Rigidbody rigidbody;

        [SerializeField] public TypeProjectile typeProjectile;

        [SerializeField] private GameObject explosionArea;

        [SerializeField] private bool disapearOnHitEntity = true;

        public IEnumerator TimerBeforeDisapear(float duration)
        {
            yield return new WaitForSeconds(duration);

            if (typeProjectile == TypeProjectile.Grenade)
            {
                explosionArea.SetActive(false);
            }

            DesactivePrefab();
        }

        private void OnEnable()
        {
            if (typeProjectile == TypeProjectile.Arrow)
            {
                StartCoroutine(TimerBeforeDisapear(5));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            bool touch = false;

            int monsterLayer = LayerMask.NameToLayer("Monster");
            int playerLayer = LayerMask.NameToLayer("Player");

            Entity entity;
            Entity wielder;

            if (weaponOrigin != null)
            {
                wielder = weaponOrigin.GetWielder();
            }
            else
            {
                wielder = origin;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {

                touch = true;
            } else if (other.gameObject.layer != LayerMask.NameToLayer("Wall") && wielder != null)
            {
                if (other.gameObject.layer == monsterLayer && wielder.typeEntity != TypeEntity.MOB)
                {
                    MonsterPrefab monsterPrefab = other.GetComponent<MonsterPrefab>();
                    entity = monsterPrefab.GetMonster();
                } else if (other.gameObject.layer == playerLayer && wielder.typeEntity != TypeEntity.PLAYER)
                {
                    PlayerPrefab playerPrefab = other.GetComponent<PlayerPrefab>();
                    entity = playerPrefab.entity;
                }
                else
                {
                    return;
                }
                
                if (entity.IdEntity == wielder.IdEntity &&
                    ((other.gameObject.layer == monsterLayer && wielder.typeEntity == TypeEntity.MOB) ||
                     (other.gameObject.layer == playerLayer && wielder.typeEntity == TypeEntity.PLAYER))
                )
                {
                    return;
                }

                if (weaponOrigin != null)
                {
                    weaponOrigin.TouchEntity(entity, transform.forward);
                }

                if (disapearOnHitEntity)
                {
                    touch = true;
                }
            }
            else
            {
                touch = true;
            }

            if (touch)
            {
                if (explosionArea == null)
                {
                    DesactivePrefab();
                }
                else
                {
                    StartCoroutine(TimerBeforeDisapear(1));
                }
            }
        }

        private void DesactivePrefab()
        {
            rigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
