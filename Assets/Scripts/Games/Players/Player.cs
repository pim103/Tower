﻿using System.Diagnostics;
using Games.Global;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Games.Transitions;
using Networking.Client;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Debug = UnityEngine.Debug;

namespace Games.Players
{
    public enum Classes
    {
        Warrior,
        Ranger,
        Rogue,
        Mage
    }

    public class Player : Entity
    {
        public HelmetArmor helmetArmor { get; set; }
        public ChestplateArmor chestplateArmor { get; set; }
        public LeggingsArmor leggingsArmor { get; set; }

        private PlayerPrefab playerPrefab;

        public Classes mainClass { get; set; }

        public void SetPlayerPrefab(PlayerPrefab playerPrefab)
        {
            this.playerPrefab = playerPrefab;
        }

        public override void BasicAttack()
        {
            playerPrefab.PlayBasicAttack();
        }

        public override void BasicDefense()
        {
            Effect regen = new Effect { typeEffect = TypeEffect.Regen, launcher = this, level = 1, durationInSeconds = 5f, ressourceCost = 1 };
            Effect invisible = new Effect { typeEffect = TypeEffect.Invisibility, launcher = this, level = 1, durationInSeconds = 5f, ressourceCost = 1 };
            
            switch (mainClass)
            {
//                case Classes.Mage:
//                    EffectController.ApplyEffect(this, regen);
//                    break;
//                case Classes.Rogue:
//                    EffectController.ApplyEffect(this, invisible);
//                    break;
//                case Classes.Ranger:
//                    if (!underEffects.ContainsKey(TypeEffect.MadeADash) && ressource1 > 10)
//                    {
//                        ressource1 -= 10;
//
//                        playerPrefab.PlaySpecialMovement(SpecialMovement.BackDash);
//                        BasicAttack();
//
//                        EffectController.ApplyNewEffectToEntity(this, TypeEffect.MadeADash, 0.2f);
//                    }
//                    break;
//                case Classes.Warrior:
//                    isBlocking = true;
//                    break;
            }
        }

        public override void DesactiveBasicDefense()
        {
//            switch (mainClass)
//            {
//                case Classes.Mage:
//                    EffectController.RemoveEffect(this, TypeEffect.Regen);
//                    break;
//                case Classes.Rogue:
//                    EffectController.RemoveEffect(this, TypeEffect.Invisibility);
//                    break;
//                case Classes.Ranger:
//                    break;
//                case Classes.Warrior:
//                    nbShieldBlock = 0;
//                    isBlocking = false;
//                    break;
//            }
        }

        public void InitPlayerStats(Classes classe)
        {
            mainClass = classe;
            typeEntity = TypeEntity.ALLIES;

            IdEntity = GameController.PlayerIndex;
            isPlayer = true;

            switch(classe)
            {
                case Classes.Mage:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    break;
                case Classes.Warrior:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    break;
                case Classes.Rogue:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    break;
                case Classes.Ranger:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    break;
            }

            initialAtt = att;
            initialDef = def;
            initialHp = hp;
            initialSpeed = speed;
            initialAttSpeed = attSpeed;
            initialRessource1 = ressource1;
            initialRessource2 = ressource2;

            InitEntityList();
            int idWeapon = GetIdWeaponFromCategory(ChooseDeckAndClasse.currentWeaponIdentity.categoryWeapon);
            InitWeapon(idWeapon);

            SpellController.CastPassiveSpell(this);
        }

        private int GetIdWeaponFromCategory(CategoryWeapon categoryWeapon)
        {
            switch(categoryWeapon)
            {
                case CategoryWeapon.SHORT_SWORD:
                    return 1;
                case CategoryWeapon.BOW:
                    return 2;
                case CategoryWeapon.SPEAR:
                    return 3;
                case CategoryWeapon.DAGGER:
                    return 4;
                case CategoryWeapon.STAFF:
                    return 5;
            }

            return 0;
        }

        public void InitWeapon(int idWeapon)
        {
            Weapon weapon = DataObject.WeaponList.GetWeaponWithId(idWeapon);

            playerPrefab.AddItemInHand(weapon);
            weapon.InitPlayerSkill(mainClass);
            // TODO : Add init weapon => change basic attack spell

            weapons.Add(weapon);
        }
    }
}