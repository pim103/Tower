using Games.Global.Weapons;
using Scripts.Games;
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

        public void InitWeapon(string name)
        {
            GameObject playerHand = objectsInScene.playerExposer[se.gameController.PlayerIndex].playerHand;
            GameObject weapon = Instantiate(se.weaponList.GetWeaponWithName(name), playerHand.transform);

            float scaleY = weapon.transform.localScale.y;

            weapon.transform.localPosition = new Vector3(0, scaleY, 0);

            weapons[0] = weapon.GetComponent<Weapon>();
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
            InitWeapon("BasicSword");
        }
    }
}