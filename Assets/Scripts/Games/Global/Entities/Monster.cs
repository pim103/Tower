using System;
using System.Collections.Generic;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using UnityEngine;
using Utils;

namespace Games.Global.Entities
{
    public enum MonsterType
    {
        Tank,
        Support,
        Distance,
        Cac
    }

    [Serializable]
    public class Monster: Entity
    {
        public int id { get; set; }
        public string mobName { get; set; }
        public int nbWeapon { get; set; }
        public int weaponOriginalId { get; set; }

        public int family { get; set; }

        private TypeWeapon constraint { get; set; }

        private MonsterPrefab monsterPrefab;

        public Texture2D sprite { get; set; }
        
        public MonsterType monsterType { get; set; }

        public void SetConstraint(TypeWeapon nconstraint)
        {
            constraint = nconstraint;
        }
        
        
        public bool IsFacingProjectile()
        {
            // TODO :  Implementing Projectile Handler (look hard)
            return false;
        }

        public TypeWeapon GetConstraint()
        {
            return constraint;
        }

        public override void BasicAttack()
        {
            if (monsterPrefab.target != null)
            {
                monsterPrefab.PlayBasicAttack();
            }
        }

        public void InitMonster(MonsterPrefab newMonsterPrefab)
        {
            InitEntityList();
            monsterPrefab = newMonsterPrefab;
            monsterPrefab.SetMonster(this);

            if (constraint == TypeWeapon.Distance)
            {
                SetBehaviorType(BehaviorType.Distance);
            } else if (constraint == TypeWeapon.Cac)
            {
                SetBehaviorType(BehaviorType.Melee);
            }

            SetAttackBehaviorType(AttackBehaviorType.Random);
        }

        public bool InitWeapon(int idWeapon)
        {
            Weapon weapon = DataObject.EquipmentList.GetWeaponWithId(idWeapon);

            if (constraint != weapon.type)
            {
                return false;
            }

            monsterPrefab.AddItemInHand(weapon);
            this.weapon = weapon;

            SpellController.CastPassiveSpell(this);

            return false;
        }

        public void InitOriginalWeapon()
        {
            if (nbWeapon == 0)
            {
                return;
            }
            
            Weapon weapon = DataObject.EquipmentList.GetWeaponWithId(weaponOriginalId);

            monsterPrefab.AddItemInHand(weapon);

            this.weapon = weapon;
        }

        public void InitKey(GameObject keyObject)
        {
            monsterPrefab.objectsToLoot.Add(keyObject);
        }
    }
}
