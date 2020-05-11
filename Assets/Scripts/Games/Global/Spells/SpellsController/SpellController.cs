using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Games.Global.Spells.SpellsController
{
    public class SpellController : MonoBehaviour
    {
        [SerializeField] private BuffController buffController;
        [SerializeField] private AreaOfEffectController areaOfEffectController;
        [SerializeField] private MovementController movementController;
        [SerializeField] private SpecialAttackController specialAttackController;
        [SerializeField] private TargetedAttackController targetedAttackController;
        [SerializeField] private WaveController waveController;
        [SerializeField] private ProjectileController projectileController;
        [SerializeField] private SummonController summonController;
        [SerializeField] private PassiveController passiveController;
        [SerializeField] private TransformationController transformationController;

        public static SpellController instance;

        public static Spell sp = null;

        private void Start()
        {
            instance = this;
        }

        public static void CastSpell(Entity entity, Spell spell, Vector3 startPosition, Entity target = null)
        {
            if (spell == null)
            {
                return;
            }

            if (!spell.isOnCooldown)
            {
                instance.StartCoroutine(PlayCastTime(entity, spell, startPosition, target));
            }
        }

        private static void SetOriginalPosition(SpellComponent spellComponent, Vector3 startPosition, Entity target = null)
        {
            switch (spellComponent.positionToStartSpell)
            {
                case PositionToStartSpell.DynamicPosition:
                    switch (spellComponent.typeSpell)
                    {
                        case TypeSpell.Buff:
                            break;
                        case TypeSpell.Projectile:
                            ProjectileSpell projectileSpell = (ProjectileSpell) spellComponent;
                            projectileSpell.startPosition = startPosition;
                            break;
                        case TypeSpell.Summon:
                            SummonSpell summonSpell = (SummonSpell) spellComponent;
                            summonSpell.startPosition = startPosition;
                            break;
                        case TypeSpell.Transformation:
                            break;
                        case TypeSpell.Wave:
                            WaveSpell wave = (WaveSpell) spellComponent;
                            wave.startPosition = startPosition;
                            break;
                        case TypeSpell.SpecialAttack:
                            break;
                        case TypeSpell.AreaOfEffect:
                            AreaOfEffectSpell area = (AreaOfEffectSpell) spellComponent;
                            area.startPosition = startPosition;
                            area.transformToFollow = target != null ? target.entityPrefab.transform : null;
                            break;
                        case TypeSpell.TargetedAttack:
                            break;
                        case TypeSpell.Movement:
                            MovementSpell movement = (MovementSpell) spellComponent;
                            movement.tpPosition = startPosition;
                            movement.trajectory = startPosition;
                            movement.target = target;
                            break;
                    }

                    break;
            }
        }

        public static void CastSpellComponent(Entity entity, SpellComponent spellComponent, Vector3 startPosition, Entity target = null)
        {
            SetOriginalPosition(spellComponent, startPosition, target);

            ISpellController iSpellController = null;
            switch (spellComponent.typeSpell)
            {
                case TypeSpell.Buff:
                    iSpellController = instance.buffController;
                    break;
                case TypeSpell.Projectile:
                    iSpellController = instance.projectileController;
                    break;
                case TypeSpell.Summon:
                    iSpellController = instance.summonController;
                    break;
                case TypeSpell.Transformation:
                    iSpellController = instance.transformationController;
                    break;
                case TypeSpell.Wave:
                    iSpellController = instance.waveController;
                    break;
                case TypeSpell.SpecialAttack:
                    iSpellController = instance.specialAttackController;
                    break;
                case TypeSpell.AreaOfEffect:
                    iSpellController = instance.areaOfEffectController;
                    break;
                case TypeSpell.TargetedAttack:
                    iSpellController = instance.targetedAttackController;
                    break;
                case TypeSpell.Movement:
                    iSpellController = instance.movementController;
                    break;
            }

            if (iSpellController == null)
            {
                return;
            }

            iSpellController.LaunchSpell(entity, spellComponent);
        }

        public static IEnumerator StartCooldown(Entity entity, Spell spell)
        {
            spell.isOnCooldown = true;
            yield return new WaitForSeconds(spell.cooldown);
            spell.isOnCooldown = false;
        }
        
        public static IEnumerator PlayCastTime(Entity entity, Spell spell, Vector3 startPosition, Entity target = null)
        {
            float duration = spell.castTime;

            while (duration > 0)
            {
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;
            }

            if (spell.activeSpellComponent == null)
            {
                yield break;
            }

            instance.StartCoroutine(StartCooldown(entity, spell));
            CastSpellComponent(entity, spell.activeSpellComponent, startPosition, target);
        }
    }
}