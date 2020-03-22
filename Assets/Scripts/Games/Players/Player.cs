using Games.Global;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Weapons;
using Networking.Client;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

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
            switch (mainClass)
            {
                case Classes.Mage:
                    ApplyNewEffect(TypeEffect.Regen, 5f, 1, this, 1);
                    break;
                case Classes.Rogue:
                    ApplyNewEffect(TypeEffect.Invisibility, 5f, 1, this, 1);
                    break;
                case Classes.Ranger:
                    if (!underEffects.ContainsKey(TypeEffect.MadeADash) && ressource1 > 10)
                    {
                        ressource1 -= 10;
                        
                        playerPrefab.PlaySpecialMovement(SpecialMovement.BackDash);
                        BasicAttack();
                        
                        ApplyNewEffect(TypeEffect.MadeADash, 0.2f);
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
                    RemoveEffect(TypeEffect.Regen);
                    break;
                case Classes.Rogue:
                    RemoveEffect(TypeEffect.Invisibility);
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

            IdEntity = GameController.PlayerIndex;

            switch(classe)
            {
                case Classes.Mage:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 1;
                    break;
                case Classes.Warrior:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 1;
                    break;
                case Classes.Rogue:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 1;
                    break;
                case Classes.Ranger:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 1;
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
            InitWeapon(2);
        }

        public void InitWeapon(int idWeapon)
        {
            Weapon weapon = DataObject.WeaponList.GetWeaponWithId(idWeapon);

            playerPrefab.AddItemInHand(weapon);
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
                    ApplyNewEffect(TypeEffect.Stun, 3, 1);
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
                TowersWebSocket.TowerSender("OTHERS", GameController.staticRoomId, "Player", "SendDeath", null);
                Debug.Log("Vous êtes mort");
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("MenuScene");
            }
        }
    }
}