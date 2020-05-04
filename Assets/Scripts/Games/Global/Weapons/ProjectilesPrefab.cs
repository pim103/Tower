using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Games.Global.Abilities.SpecialSpellPrefab;
using Games.Global.Entities;
using Games.Players;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

        [SerializeField] private SpellScript spellScript;
        
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
                if (explosionArea != null)
                {
                    AreaSpell scriptAreaSpell = explosionArea.GetComponent<AreaSpell>();
                    scriptAreaSpell.origin = origin;

                    explosionArea.SetActive(true);
                }

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
                    weaponOrigin.TouchEntity(entity);
                }

                if (spellScript != null)
                {
                    spellScript.PlaySpecialEffect(origin, entity);
                }

                if (explosionArea != null)
                {
                    AreaSpell scriptAreaSpell = explosionArea.GetComponent<AreaSpell>();
                    scriptAreaSpell.origin = origin;

                    explosionArea.SetActive(true);
                    scriptAreaSpell.EnableAreaEffect();
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

        private void OnCollisionEnter(Collision other)
        {
            if (typeProjectile == TypeProjectile.Grenade && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                AreaSpell scriptAreaSpell = explosionArea.GetComponent<AreaSpell>();
                scriptAreaSpell.origin = origin;

                explosionArea.SetActive(true);
                scriptAreaSpell.EnableAreaEffect();

                StartCoroutine(TimerBeforeDisapear(1));

                rigidbody.velocity = Vector3.zero;
            }
        }

        private void DesactivePrefab()
        {
            rigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
