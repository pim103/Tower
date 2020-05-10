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

        private void Start()
        {
            instance = this;
        }

        public static void CastSpell(Entity entity, Spell spell)
        {
            instance.StartCoroutine(PlayCastTime(entity, spell));
        }

        private static void SetOriginalPosition(SpellComponent spellComponent, Vector3 startPosition)
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
                            break;
                        case TypeSpell.TargetedAttack:
                            break;
                        case TypeSpell.Movement:
                            break;
                    }

                    break;
            }
        }
        
        public static void CastSpellComponent(Entity entity, SpellComponent spellComponent, Vector3 startPosition)
        {
            SetOriginalPosition(spellComponent, startPosition);

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
        
        public static IEnumerator PlayCastTime(Entity entity, Spell spell)
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

            StartCooldown(entity, spell);
            CastSpellComponent(entity, spell.activeSpellComponent, Vector3.positiveInfinity);
        }
    }
}