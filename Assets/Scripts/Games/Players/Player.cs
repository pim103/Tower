using System.Collections.Generic;
using Games.Global;
using Games.Global.Armors;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Games.Transitions;
using UnityEngine;
using UnityEngine.UI;

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
            entityPrefab = playerPrefab;
            this.playerPrefab = playerPrefab;
        }

        public override void BasicAttack()
        {
            playerPrefab.PlayBasicAttack();
        }

        public void ResetSpellCooldownAndStatus()
        {
            hasDivineShield = false;

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
                att = initialAtt = mainClass.att;
                def = initialDef = mainClass.def;
                speed = initialSpeed = mainClass.speed;
                hp = initialHp = mainClass.hp;
                attSpeed = initialAttSpeed = mainClass.attSpeed;
                ressource = initialRessource1 = mainClass.ressource;

                basicDefense = mainClass.defenseSpell;
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

            if (weapon != null)
            {
                InitSpellFromClassesAndWeapons(weapon.category.id);
            }
            
            ResetStats();
        }
        
        public void InitPlayer(int idClasses)
        {
            InitEntityList();

            if (ChooseDeckAndClass.currentWeaponIdentity)
            {
                int idCategoryWeapon = ChooseDeckAndClass.currentWeaponIdentity.GetIdentityId();
                Weapon weapon = DataObject.EquipmentList.GetFirstWeaponFromIdCategory(idCategoryWeapon);
                InitWeapon(weapon);
            }

            InitClasses(idClasses);

            SetTypeEntity(TypeEntity.ALLIES);

            IdEntity = DataObject.nbEntityInScene;
            DataObject.nbEntityInScene++;
            isPlayer = true;

            SpellController.CastPassiveSpell(this);
        }

        public void InitSpellFromClassesAndWeapons(int idCategory)
        {
            CategoryWeapon categoryWeapon = DataObject.CategoryWeaponList.GetCategoryFromId(idCategory);
            spells.AddRange(DataObject.ClassesList.GetSpellForClassesAndCategory(categoryWeapon, mainClass));

            int loop = 0;
            Text currentText;
            foreach (Spell spell in spells)
            {
                currentText = playerPrefab.spell1;
                if (loop == 1)
                {
                    currentText = playerPrefab.spell2;
                } else if (loop == 2)
                {
                    currentText = playerPrefab.spell3;
                }

                ++loop;
                currentText.text = spell.nameSpell;
            }
        }
        
        public void InitWeapon(Weapon weapon)
        {
            if (weapon == null)
            {
                return;
            }
            playerPrefab.AddItemInHand(weapon);
            this.weapon = weapon;
        }
    }
}