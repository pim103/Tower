using System;
using System.Collections.Generic;
using Games.Global.Abilities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global.Entities {
  public class Monster : Entity {
    public int id {
      get;
      set;
    }
    public string mobName {
      get;
      set;
    }
    public int nbWeapon {
      get;
      set;
    }
    public string weaponOriginalName {
      get;
      set;
    }

    public Family family {
      get;
      set;
    }

    public TypeWeapon constraint {
      get;
      set;
    }

    private MonsterPrefab monsterPrefab;

    public List<string> spellsName {
      get;
      set;
    }

    public override void BasicAttack() {
      if (monsterPrefab.target != null) {
        monsterPrefab.PlayBasicAttack();
      }
    }

    public override void BasicDefense() {
      throw new System.NotImplementedException();
    }

    public override void DesactiveBasicDefense() {
      throw new NotImplementedException();
    }

    public void InitMonster(MonsterPrefab newMonsterPrefab) {
      monsterPrefab = newMonsterPrefab;
      monsterPrefab.SetMonster(this);

      if (constraint == TypeWeapon.Distance) {
        BehaviorType = BehaviorType.Distance;
      } else if (constraint == TypeWeapon.Cac) {
        BehaviorType = BehaviorType.Melee;
      }

      AttackBehaviorType = AttackBehaviorType.Random;
    }

    public bool InitWeapon(int idWeapon) {
      if (weapons.Count < nbWeapon) {
        Weapon weapon = DataObject.WeaponList.GetWeaponWithId(idWeapon);

        if (constraint != weapon.type) {
          return false;
        }

        monsterPrefab.AddItemInHand(weapon);
        weapons.Add(weapon);

        return true;
      }

      SpellController.CastPassiveSpell(this);

      return false;
    }

    public void InitOriginalWeapon() {
      Weapon weapon =
          DataObject.WeaponList.GetWeaponWithName(weaponOriginalName);

      monsterPrefab.AddItemInHand(weapon);

      weapons.Add(weapon);
    }

    public void InitSpells() {
      foreach (string spellName in spellsName) {
        Spell spell = SpellController.LoadSpellByName(spellName);

        if (spell != null) {
          spells.Add(spell);
        } else {
          Debug.Log("Doesnt find spell " + spellName);
        }
      }
    }

    public void InitKey(GameObject keyObject) {
      monsterPrefab.objectsToLoot.Add(keyObject);
    }
  }
}
