using System.Diagnostics;
using Games.Global;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Weapons;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Games.Players
{
    public enum Classes
    {
        WARRIOR,
        RANGER,
        ROGUE,
        MAGE
    }

    public class Player : Entity
    {
        [SerializeField]
        public ObjectsInScene objectsInScene;

        [SerializeField]
        public ScriptsExposer se;

        public HelmetArmor helmetArmor;
        public ChestplateArmor chestplateArmor;
        public LeggingsArmor leggingsArmor;

        private Classes mainClass;
        
        /*
         * Specific Warrior
         */
        public int nbShieldBlock = 0;
        public bool isBlocking = false;

        public override bool InitWeapon(int idWeapon)
        {
            GameObject playerHand = objectsInScene.playerExposer[GameController.PlayerIndex].playerHand;
            Weapon weapon = DataObject.WeaponList.GetWeaponWithId(idWeapon);

            InstantiateParameters param = new InstantiateParameters();
            param.item = weapon;
            param.type = TypeItem.Weapon;
            param.wielder = this;

            weapon.InstantiateModel(param, Vector3.zero, playerHand.transform);
            weapon.InitPlayerSkill(mainClass);

            weapons.Add(weapon);

            return true;
        }

        public override void BasicAttack()
        {
            weapons[0].BasicAttack(movementPatternController, objectsInScene.playerExposer[GameController.PlayerIndex].playerHand);
        }

        public override void BasicDefense()
        {
            switch (mainClass)
            {
                case Classes.MAGE:
                    break;
                case Classes.ROGUE:
                    break;
                case Classes.RANGER:
                    break;
                case Classes.WARRIOR:
                    isBlocking = true;
                    break;
            }
        }

        public void DesactiveBasicDefense()
        {
            switch (mainClass)
            {
                case Classes.MAGE:
                    break;
                case Classes.ROGUE:
                    break;
                case Classes.RANGER:
                    break;
                case Classes.WARRIOR:
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
                case Classes.MAGE:
                    att = 10;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    break;
                case Classes.WARRIOR:
                    att = 10;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    break;
                case Classes.ROGUE:
                    att = 10;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    break;
                case Classes.RANGER:
                    att = 10;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    break;
                default:
                    break;
            }

            InitEquipementArray();
            InitWeapon(2);
        }

        public override void TakeDamage(int initialDamage, AbilityParameters abilityParameters)
        {
            if (isBlocking)
            {
                nbShieldBlock++;

                if (nbShieldBlock == 4)
                {
                    Debug.Log("Stun");
                    nbShieldBlock = 0;
                    isBlocking = false;
                }

                return;
            }

            base.TakeDamage(initialDamage, abilityParameters);

            if (hp <= 0)
            {
                //TODO : Destroy player
            }
        }
    }
}