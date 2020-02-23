using Games.Global.Weapons;
using Scripts.Games.Global;
using Scripts.Games.Global.Armors;
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
            GameObject playerHand = objectsInScene.playerExposer[se.gameController.PlayerIndex].playerHand;
            Weapon weapon = se.weaponList.GetWeaponWithId(idWeapon);
            
            weapon.InstantiateModel(playerHand.transform);

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