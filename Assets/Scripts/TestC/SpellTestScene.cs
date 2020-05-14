using System.Collections;
using System.Collections.Generic;
using Games;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Games.Players;
using Games.Transitions;
using UnityEngine;

namespace TestC
{
    public class SpellTestScene : MonoBehaviour
    {
        [SerializeField] private PlayerPrefab player;

        [SerializeField] private GameObject arrowPrefab;
        
        // Start is called before the first frame update
        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            DataObject.playerInScene.Add(GameController.PlayerIndex, player);

            Identity classe = new Identity();
            classe.classe = Classes.Warrior;

            Identity weapon = new Identity();
            weapon.categoryWeapon = CategoryWeapon.SHORT_SWORD;

            ChooseDeckAndClasse.currentRoleIdentity = classe;
            ChooseDeckAndClasse.currentWeaponIdentity = weapon;

            StartCoroutine(Waiting());
        }

        private IEnumerator Waiting()
        {
//            yield return new WaitForSeconds(3f);
//            TestSpell();
//            
            yield return new WaitForSeconds(5f);
//            OtherSpell();
        }

        private void OtherSpell()
        {
            player.entity.att = 15;


            List<Vector3> positions = new List<Vector3>();
            positions.Add(Vector3.forward);
            positions.Add(Vector3.forward);
            positions.Add(Vector3.forward);
            positions.Add(Vector3.forward);
            positions.Add(Vector3.forward);
            
            AreaOfEffectSpell area = new AreaOfEffectSpell
            {
                startPosition = Vector3.zero,
                scale = Vector3.one * 20,
                onePlay = true,
                damagesOnEnemiesOnInterval = 30.0f,
                geometry = Geometry.Sphere,
                OriginalDirection = OriginalDirection.None,
                OriginalPosition = OriginalPosition.Caster
            };

            Spell newSpell = new Spell
            {
                cost = 0,
                cooldown = 2,
                castTime = 0,
                activeSpellComponent = area
            };

            List<Spell> spells = new List<Spell>();
            spells.Add(newSpell);
            

            SummonSpell summonSpell = new SummonSpell
            {
                duration = 50,
                hp = 50,
//                basicAttack = sword.basicAttack,
                attackDamage = 40,
                attackSpeed = 1,
                damageType = DamageType.Magical,
                canMove = true,
                isTargetable = true,
                idPoolObject = 2,
                moveSpeed = 10,
                summonNumber = 2,
                BehaviorType = BehaviorType.Distance,
                positionPresets = positions,
                isUnique = true,
                nbUseSpells = 2,
                spells = spells,
                AttackBehaviorType = AttackBehaviorType.AllSpellsIFirst
            };

            SpellController.CastSpellComponent(player.entity, summonSpell, player.transform.position, player.entity);
//            Effect repulse = new Effect { typeEffect = TypeEffect.Expulsion, launcher = player.entity, level = 10, directionExpul = DirectionExpulsion.Out, originExpulsion = OriginExpulsion.SrcDamage};
//            List<Effect> effects = new List<Effect>();
//            effects.Add(repulse);
//            player.entity.damageDealExtraEffect.Add(repulse);
        }

        private void TestSpell()
        {
//            AreaOfEffectSpell linked = new AreaOfEffectSpell
//            {
//                startPosition = Vector3.zero,
//                scale = Vector3.one * 5,
//                onePlay = true,
//                damagesOnEnemiesOnInterval = 15.0f,
//                typeSpell = TypeSpell.AreaOfEffect,
//                geometry = Geometry.Sphere,
//                positionToStartSpell = PositionToStartSpell.DynamicPosition
//            };

            Effect repulse = new Effect { typeEffect = TypeEffect.Expulsion, launcher = player.entity, level = 10, directionExpul = DirectionExpulsion.Out, originExpulsion = OriginExpulsion.SrcDamage};
            List<Effect> effects = new List<Effect>();
            effects.Add(repulse);

//            Effect regen = new Effect
//                {launcher = player.entity, level = 2, durationInSeconds = 10, typeEffect = TypeEffect.Regen};
//
//            SpellWithCondition spellWithCondition = new SpellWithCondition { effect = regen, conditionType = ConditionType.IfTargetDies};
//            List<SpellWithCondition> spellWithConditions = new List<SpellWithCondition>();
//            spellWithConditions.Add(spellWithCondition);
//
//
//            Effect resurrect = new Effect
//                {launcher = player.entity, level = 1, durationInSeconds = 10, typeEffect = TypeEffect.Resurrection};
//            List<Effect> effectsOnSelf = new List<Effect>();
//            effectsOnSelf.Add(resurrect);
//
//            BuffSpell buffSpell = new BuffSpell
//            {
//                duration = 5,
//                interval = 0.5f,
//                typeSpell = TypeSpell.Buff,
//                damageOnSelf = 100,
//                effectOnSelf = effectsOnSelf,
//                linkedSpellOnHit = area,
//                needNewPositionOnHit = true,
//                positionToStartSpell = PositionToStartSpell.Himself
//            };
//
//            player.entity.att = 15;
//            SpellController.CastSpellComponent(player.entity, buffSpell, Vector3.positiveInfinity);

//            ProjectileSpell projectileSpell = new ProjectileSpell
//            {
//                prefab = arrowPrefab,
//                duration = 5,
//                speed = 4,
//                initialRotation = player.transform.localEulerAngles,
//                trajectory = player.transform.forward,
//                positionToStartSpell = PositionToStartSpell.Himself,
//                startPosition = player.transform.position + (Vector3.up * 1.5f),
//                effectsOnHit = effects,
//                damages = 10,
//                damageMultiplierOnDistance = 1.5f,
//                linkedSpellOnDisable = area
//            };

            AreaOfEffectSpell area = new AreaOfEffectSpell
            {
                startPosition = Vector3.zero,
                scale = Vector3.one * 2,
                duration = 3,
                interval = 0.05f,
                typeSpell = TypeSpell.AreaOfEffect,
                geometry = Geometry.Sphere,
                damagesOnEnemiesOnInterval = 11.0f,
                effectsOnEnemiesOnInterval = effects,
                wantToFollow = true,
                OriginalPosition = OriginalPosition.Caster,
                OriginalDirection = OriginalDirection.Forward,
                transformToFollow = player.transform
            };
            
            MovementSpell movementSpell = new MovementSpell
            {
                duration = 3f,
                speed = 20,
                isFollowingMouse = true,
                movementSpellType = MovementSpellType.Charge,
                linkedSpellAtTheStart = area,
                OriginalPosition = OriginalPosition.Caster,
                OriginalDirection = OriginalDirection.Forward
            };

            SpellController.CastSpellComponent(player.entity, movementSpell, player.positionPointed, player.target);
        }
    }
}
