using Games.Global;
using Games.Global.Armors;
using Games.Global.Weapons;
using UnityEngine;

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

        public override void InitWeapon(int idWeapon)
        {
            GameObject playerHand = objectsInScene.playerExposer[GameController.PlayerIndex].playerHand;
            Weapon weapon = DataObject.WeaponList.GetWeaponWithId(idWeapon);

            InstantiateParameters param = new InstantiateParameters();
            param.item = weapon;
            param.type = TypeItem.Weapon;
            param.wielder = this;

            weapon.InstantiateModel(param, Vector3.zero, playerHand.transform);

            weapons.Add(weapon);
        }

        public override void BasicAttack()
        {
            weapons[0].BasicAttack(movementPatternController, objectsInScene.playerExposer[GameController.PlayerIndex].playerHand);
        }

        public void InitPlayerStats(Classes classe)
        {
            mainClass = classe;
            typeEntity = TypeEntity.PLAYER;

            switch(classe)
            {
                case Classes.MAGE:
                    att = 10;
                    def = 10;
                    speed = 10;
                    hp = 100;
                    break;
                case Classes.WARRIOR:
                    att = 10;
                    def = 10;
                    speed = 10;
                    hp = 100;
                    break;
                case Classes.ROGUE:
                    att = 10;
                    def = 10;
                    speed = 10;
                    hp = 100;
                    break;
                case Classes.RANGER:
                    att = 10;
                    def = 10;
                    speed = 10;
                    hp = 100;
                    break;
                default:
                    break;
            }

            InitEquipementArray();
            InitWeapon(2);
        }
    }
}