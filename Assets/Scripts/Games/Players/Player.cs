using Games.Global;
using Games.Global.Armors;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Games.Transitions;
using UnityEngine;

namespace Games.Players
{
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
            if (basicAttack != null)
            {
                basicAttack.isOnCooldown = false;
            }

            if (basicDefense != null)
            {
                basicDefense.isOnCooldown = false;
            }
    
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
            Debug.Log("You play a " + mainClass.name);
            
            if (mainClass != null)
            {
                initialAtt = mainClass.att;
                initialDef = mainClass.def;
                initialSpeed = mainClass.speed;
                initialHp = mainClass.hp;
                initialAttSpeed = mainClass.attSpeed;
                initialRessource1 = mainClass.ressource;

                basicDefense = SpellController.LoadSpellByName(mainClass.defenseSpell);
            }

            initialMagicalDef = magicalDef;
            initialPhysicalDef = physicalDef;
            ResetSpellCooldownAndStatus();
        }

        private void InitClasses(int idClasses)
        {
            Classes classe = DataObject.ClassesList.GetClassesFromId(idClasses);
            InitClasses(classe);
        }

        public void InitClasses(Classes classes)
        {
            mainClass = classes;
            
            ResetStats();
        }
        
        public void InitPlayer(int idClasses)
        {
            InitEntityList();

            InitClasses(idClasses);

            SetTypeEntity(TypeEntity.ALLIES);

            IdEntity = DataObject.nbEntityInScene;
            DataObject.nbEntityInScene++;
            isPlayer = true;

            Weapon weapon = DataObject.EquipmentList.GetFirstWeaponFromIdCategory(ChooseDeckAndClass.currentWeaponIdentity.GetIdentityId());
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