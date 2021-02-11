using Games.Global;
using Games.Global.Armors;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Games.Transitions;
using UnityEngine;

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
            if (basicDefense != null)
            {
                SpellController.CastSpell(this, basicDefense);
            }
        }

        public void ResetSpellCooldownAndStatus()
        {
            basicAttack.isOnCooldown = false;
            basicDefense.isOnCooldown = false;
            
            foreach (Spell spell in spells)
            {
                spell.isOnCooldown = false;
            }

            ClearUnderEffect();
            damageDealExtraEffect.Clear();
            damageReceiveExtraEffect.Clear();
            activeSpellComponents.Clear();

            SpellController.CastPassiveSpell(this);
        }

        public void ResetStats()
        {
            switch(mainClass)
            {
                case Classes.Mage:
                    att = initialAtt;
                    def = initialDef;
                    speed = initialSpeed;
                    hp = initialHp;
                    ressource1 = initialRessource1;
                    attSpeed = initialAttSpeed;
                    physicalDef = initialPhysicalDef;
                    magicalDef = initialMagicalDef;
                    break;
                case Classes.Warrior:
                    att = initialAtt;
                    def = initialDef;
                    speed = initialSpeed;
                    hp = initialHp;
                    ressource1 = initialRessource1;
                    attSpeed = initialAttSpeed;
                    physicalDef = initialPhysicalDef;
                    magicalDef = initialMagicalDef;
                    break;
                case Classes.Rogue:
                    att = initialAtt;
                    def = initialDef;
                    speed = initialSpeed;
                    hp = initialHp;
                    ressource1 = initialRessource1;
                    attSpeed = initialAttSpeed;
                    physicalDef = initialPhysicalDef;
                    magicalDef = initialMagicalDef;
                    break;
                case Classes.Ranger:
                    att = initialAtt;
                    def = initialDef;
                    speed = initialSpeed;
                    hp = initialHp;
                    ressource1 = initialRessource1;
                    attSpeed = initialAttSpeed;
                    physicalDef = initialPhysicalDef;
                    magicalDef = initialMagicalDef;
                    break;
            }

            ResetSpellCooldownAndStatus();
        }
        
        public void InitPlayerStats(Classes classe)
        {
            mainClass = classe;
            SetTypeEntity(TypeEntity.ALLIES);

            IdEntity = DataObject.nbEntityInScene;
            DataObject.nbEntityInScene++;
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
                    basicDefense = SpellController.LoadSpellByName("BasicDefenseMage");
                    break;
                case Classes.Warrior:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    nbCharges = 4;
                    basicDefense = SpellController.LoadSpellByName("BasicDefenseWarrior");
                    break;
                case Classes.Rogue:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    basicDefense = SpellController.LoadSpellByName("BasicDefenseRogue");
                    break;
                case Classes.Ranger:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    basicDefense = SpellController.LoadSpellByName("BasicDefenseRanger");
                    break;
            }

            initialAtt = att;
            initialDef = def;
            initialHp = hp;
            initialSpeed = speed;
            initialAttSpeed = attSpeed;
            initialRessource1 = ressource1;
            initialRessource2 = ressource2;
            initialMagicalDef = magicalDef;
            initialPhysicalDef = physicalDef;

            InitEntityList();
            Weapon weapon = DataObject.EquipmentList.GetFirstWeaponFromCategory(ChooseDeckAndClass.currentWeaponIdentity.categoryWeapon);
            InitWeapon(weapon);

            SpellController.CastPassiveSpell(this);
        }

        public void InitWeapon(Weapon weapon)
        {
            playerPrefab.AddItemInHand(weapon);
            weapon.InitPlayerSkill(mainClass);
            // TODO : Add init weapon => change basic attack spell

            this.weapon = weapon;
        }
    }
}