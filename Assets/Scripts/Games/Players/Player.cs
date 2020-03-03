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

        public void InitWeapon(int idWeapon)
        {
            GameObject playerHand = objectsInScene.playerExposer[GameController.PlayerIndex].playerHand;
            Weapon weapon = se.dm.weaponList.GetWeaponWithId(idWeapon);

            InstantiateParameters param = new InstantiateParameters();
            param.item = weapon;
            param.type = TypeItem.Weapon;

            weapon.InstantiateModel(param, Vector3.zero, playerHand.transform);

            weapons[0] = weapon;
        }

        public void InitPlayerStats(Classes classe)
        {
            mainClass = classe;

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
            InitWeapon(1);
        }
    }
}