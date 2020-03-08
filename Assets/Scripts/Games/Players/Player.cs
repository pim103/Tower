﻿using Games.Global;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Weapons;

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
        public HelmetArmor helmetArmor;
        public ChestplateArmor chestplateArmor;
        public LeggingsArmor leggingsArmor;

        private PlayerExposer playerExposer;

        public Classes mainClass;
        
        /*
         * Specific Warrior
         */
        public int nbShieldBlock = 0;
        public bool isBlocking = false;

        public void SetPlayerExposer(PlayerExposer playerExposer)
        {
            this.playerExposer = playerExposer;
        }

        public override void BasicAttack()
        {
            playerExposer.playerPrefab.PlayBasicAttack(weapons[0].weaponPrefab);
        }
        
        public override void BasicDefense()
        {
            switch (mainClass)
            {
                case Classes.Mage:
                    ApplyEffect(TypeEffect.Regen, 0.5f, 1, this, 1);
                    break;
                case Classes.Rogue:
                    break;
                case Classes.Ranger:
                    if (!underEffects.ContainsKey(TypeEffect.MadeADash))
                    {
                        playerExposer.playerPrefab.PlaySpecialMovement(SpecialMovement.BackDash);
                        BasicAttack();

                        ApplyEffect(TypeEffect.MadeADash, 0.5f);
                    }
                    break;
                case Classes.Warrior:
                    isBlocking = true;
                    break;
            }
        }

        public override void DesactiveBasicDefense()
        {
            switch (mainClass)
            {
                case Classes.Mage:
                    break;
                case Classes.Rogue:
                    break;
                case Classes.Ranger:
                    break;
                case Classes.Warrior:
                    nbShieldBlock = 0;
                    isBlocking = false;
                    break;
            }
        }

        public void InitPlayerStats(Classes classe)
        {
            mainClass = classe;
            typeEntity = TypeEntity.PLAYER;

            switch(classe)
            {
                case Classes.Mage:
                    att = 10;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    break;
                case Classes.Warrior:
                    att = 10;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    break;
                case Classes.Rogue:
                    att = 10;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    break;
                case Classes.Ranger:
                    att = 10;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    break;
            }

            initialAtt = att;
            initialDef = def;
            initialHp = hp;
            initialSpeed = speed;
            initialRessource1 = ressource1;
            initialRessource2 = ressource2;

            InitEquipementArray();
            InitWeapon(2);
        }

        public void InitWeapon(int idWeapon)
        {
            Weapon weapon = DataObject.WeaponList.GetWeaponWithId(idWeapon);

            playerExposer.playerPrefab.AddItemInHand(weapon);
            weapon.InitPlayerSkill(mainClass);

            weapons.Add(weapon);
        }

        public override void TakeDamage(float initialDamage, AbilityParameters abilityParameters)
        {
            if (isBlocking)
            {
                nbShieldBlock++;
                if (nbShieldBlock > 4)
                {
                    ApplyEffect(TypeEffect.Stun, 3, 1);
                    DesactiveBasicDefense();
                }

                return;
            }

            base.TakeDamage(initialDamage, abilityParameters);
        }
        
        public override void ApplyDamage(float directDamage)
        {
            base.ApplyDamage(directDamage);

            if (hp <= 0)
            {
                // TODO : destroy player
            }
        }
    }
}