using System.Diagnostics;
using Games.Global;
using Games.Global.Abilities;
using Games.Global.Armors;
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
        public HelmetArmor helmetArmor;
        public ChestplateArmor chestplateArmor;
        public LeggingsArmor leggingsArmor;

        private PlayerPrefab playerPrefab;

        public Classes mainClass;
        
        /*
         * Specific Warrior
         */
        public int nbShieldBlock = 0;
        public bool isBlocking = false;

        public void SetPlayerPrefab(PlayerPrefab playerPrefab)
        {
            this.playerPrefab = playerPrefab;
        }

        public override void BasicAttack()
        {
            playerPrefab.PlayBasicAttack(weapons[0].weaponPrefab);
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
            typeEntity = TypeEntity.PLAYER;

            IdEntity = GameController.PlayerIndex;

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

            InitEquipementArray();
            int idWeapon = GetIdWeaponFromCategory(ChooseDeckAndClasse.currentWeaponIdentity.categoryWeapon);
            InitWeapon(idWeapon);

//            Effect effect = new Effect { typeEffect = TypeEffect.Link, level = 1, launcher = this, durationInSeconds = 5};
//            damageDealExtraEffect.Add(effect);

//            effect = new Effect { typeEffect = TypeEffect.Bleed, level = 1, launcher = this, durationInSeconds = 5};
//            damageDealExtraEffect.Add(effect);

//            Effect effect = new Effect { typeEffect = TypeEffect.Invisibility, launcher = this, durationInSeconds = 5};
//            EffectController.ApplyEffect(this, effect);
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

            weapons.Add(weapon);
        }

        public override void TakeDamage(float initialDamage, AbilityParameters abilityParameters, bool takeTrueDamage = false)
        {
//            if (isBlocking)
//            {
//                nbShieldBlock++;
//                if (nbShieldBlock > 4)
//                {
//                    EffectController.ApplyNewEffectToEntity(this, TypeEffect.Stun, 3, 1);
//                    DesactiveBasicDefense();
//                }
//
//                return;
//            }

            base.TakeDamage(initialDamage, abilityParameters, takeTrueDamage);
        }
        
        public override void ApplyDamage(float directDamage)
        {
            base.ApplyDamage(directDamage);
            
            if (hp <= 0)
            {
                if (shooldResurrect)
                {
                    hp = initialHp / 2;
                    EffectController.StopCurrentEffect(this, underEffects[TypeEffect.Resurrection]);

                    return;
                }
                
                TowersWebSocket.TowerSender("OTHERS", GameController.staticRoomId, "Player", "SendDeath", null);
                Debug.Log("Vous êtes mort");
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("MenuScene");
            }
        }
    }
}