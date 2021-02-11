using System;
using System.Collections.Generic;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using UnityEngine;
using Utils;

namespace Games.Global.Entities
{
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

        public List<SpellList> spellsName { get; set; }

        public Texture2D sprite { get; set; }

        public void SetConstraint(TypeWeapon nconstraint)
        {
            constraint = nconstraint;
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
        
        public override void BasicDefense()
        {
            throw new System.NotImplementedException();
        }

        public override void DesactiveBasicDefense()
        {
            throw new NotImplementedException();
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

        public void InitSpells()
        {
            foreach (SpellList spellName in spellsName)
            {
                Spell spell = SpellController.LoadSpellByName(spellName.name);

                if (spell != null)
                {
                    spells.Add(spell);
                }
                else
                {
//                    Debug.Log("Doesnt find spell " + spellName.name);
                }
            }
        }

        public void InitKey(GameObject keyObject)
        {
            monsterPrefab.objectsToLoot.Add(keyObject);
        }
    }
}
