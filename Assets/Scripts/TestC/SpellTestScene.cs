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
            yield return new WaitForSeconds(1f);
            OtherSpell();
        }

        private void OtherSpell()
        {
            player.entity.att = 15;

            Sword sword = new Sword();

            List<Vector3> positions = new List<Vector3>();
            positions.Add(Vector3.forward);
            positions.Add(Vector3.forward);
            positions.Add(Vector3.forward);
            positions.Add(Vector3.forward);
            positions.Add(Vector3.forward);

            SummonSpell summonSpell = new SummonSpell
            {
                duration = 10,
                hp = 50,
                basicAttack = sword.basicAttack,
                attackDamage = 40,
                attackSpeed = 1,
                damageType = DamageType.Physical,
                canMove = true,
                isTargetable = true,
                idPoolObject = 2,
                moveSpeed = 10,
                summonNumber = 5,
                BehaviorType = BehaviorType.Melee,
                positionPresets = positions
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

//            WaveSpell waveSpell = new WaveSpell
//            {
//                typeSpell = TypeSpell.Wave,
//                duration = 10, 
//                geometryPropagation = Geometry.Square, 
//                initialRotation = player.transform.localEulerAngles,
//                initialWidth = 10, 
//                speedPropagation = 5,
//                incrementAmplitudeByTime = 5,
//                positionToStartSpell = PositionToStartSpell.Himself,
//                startPosition = player.transform.position,
//                effectsOnHit = effects,
//                damages = 15
//            };

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
                OriginalDirection = OriginalDirection.Forward
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

            SpellController.CastSpellComponent(player.entity, movementSpell, player.positionPointed);
        }
    }
}
