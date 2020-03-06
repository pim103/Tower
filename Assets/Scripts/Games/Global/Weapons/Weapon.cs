using System.Collections.Generic;
using Games.Global.Patterns;
using Games.Players;
using UnityEngine;
using UnityEngine.Serialization;

namespace Games.Global.Weapons
{
    public enum CategoryWeapon
    {
        SHORT_SWORD,
        LONG_SWORD,
        SPEAR,
        AXE,
        TWO_HAND_AXE,
        HAMMER,
        HALBERD,
        MACE,
        BOW,
        STAFF,
        DAGGER,
        TRIDENT,
        RIFLE,
        CROSSBOW,
        SLING,
        HANDGUN
    };

    public enum TypeWeapon
    {
        Distance,
        Cac
    }

    public abstract class Weapon : Equipement
    {
        public string equipementName;
        public CategoryWeapon category;
        public TypeWeapon type;
        public int damage;
        public float attSpeed;

        public List<TypeEffect> effects;

        public Pattern[] pattern;

        public Skill skill1;
        public Skill skill2;
        public Skill skill3;

        // Basic attack active trigger and play movement
        public void BasicAttack(MovementPatternController movementPatternController, GameObject objectToMove)
        {
            BoxCollider bc = instantiateModel.GetComponent<BoxCollider>();

            if (!bc.enabled)
            {
                bc.enabled = true;
            
                PlayMovement(movementPatternController, attSpeed, objectToMove, bc);

                if (type == TypeWeapon.Distance)
                {
                    // POOL PROJ
                    PoolProjectiles();
                }
            }
        }

        private void PlayMovement(MovementPatternController movementPatternController, float attSpeed, GameObject objectToMove, BoxCollider bc)
        {
            movementPatternController.PlayMovement(pattern, attSpeed, objectToMove, bc);
        }

        private void PoolProjectiles()
        {
            GameObject proj = ObjectPooler.SharedInstance.GetPooledObject(0);
            WeaponPrefab weaponPrefab = instantiateModel.GetComponent<WeaponPrefab>();

            proj.transform.position = weaponPrefab.transform.position;
            
            Vector3 rot = proj.transform.localEulerAngles;
            rot.y = weaponPrefab.transform.parent.eulerAngles.y;
            proj.transform.localEulerAngles = rot;
            proj.SetActive(true);

            ProjectilesPrefab pp = proj.GetComponent<ProjectilesPrefab>();
            pp.rigidbody.AddRelativeForce(Vector3.up * 1000, ForceMode.Acceleration);
            
            pp.weaponOrigin = instantiateModel.GetComponent<WeaponPrefab>();
        }

        public abstract void InitPlayerSkill(Classes classe);
    }
}