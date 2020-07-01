using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Games.Global.Spells.SpellsController
{
    public enum OriginalPosition
    {
        Caster,
        Target,
        PositionInParameter,
        ClosestEnemyFromCaster
    }

    public enum OriginalDirection
    {
        None,
        Forward,
        Random
    }

    public class SpellController : MonoBehaviour
    {
        [SerializeField] private BuffController buffController;
        [SerializeField] private AreaOfEffectController areaOfEffectController;
        [SerializeField] private MovementController movementController;
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

        public static bool CastSpell(Entity entity, Spell spell, Vector3 startPosition, Entity target = null)
        {
            if (spell == null)
            {
                return false;
            }

            if ((!spell.isOnCooldown && spell.nbUse != 0) || (spell.activeSpellComponent != null && spell.activeSpellComponent.isBasicAttack))
            {
                if (spell.nbUse > 0)
                {
                    spell.nbUse--;
                }

                spell.isOnCooldown = true;
                instance.StartCoroutine(PlayCastTime(entity, spell, startPosition, target));
            }
            else if (spell.canCastDuringCast)
            {
                spell.wantToCastDuringCast = true;
            }
            else if (spell.canRecast && !spell.alreadyRecast && entity.canRecast)
            {
                spell.alreadyRecast = true;
                instance.StartCoroutine(PlayCastTime(entity, spell, startPosition, target));
            }
            else
            {
                return false;
            }

            return true;
        }

        private static void SetOriginalPosition(SpellComponent spellComponent, Vector3 startPosition, Entity caster, Entity target = null)
        {
            switch (spellComponent.OriginalPosition)
            {
                case OriginalPosition.Caster:
                    spellComponent.startPosition = caster.entityPrefab.transform.position;
                    if (spellComponent.needPositionToMidToEntity)
                    {
                        spellComponent.startPosition += Vector3.up * (caster.entityPrefab.transform.localScale.y / 2);
                    }
                    break;
                case OriginalPosition.Target:
                    spellComponent.startPosition = target.entityPrefab.transform.position;
                    if (spellComponent.needPositionToMidToEntity)
                    {
                        spellComponent.startPosition += Vector3.up * (target.entityPrefab.transform.localScale.y / 2);
                    }
                    break;
                case OriginalPosition.PositionInParameter:
                    spellComponent.startPosition = startPosition;
                    break;
            }

            switch (spellComponent.OriginalDirection)
            {
                case OriginalDirection.Forward:
                    spellComponent.initialRotation = caster.entityPrefab.transform.localEulerAngles;
                    spellComponent.trajectoryNormalized = caster.entityPrefab.transform.forward;
                    break;
                case OriginalDirection.Random:
                    Vector3 rand = new Vector3();
                    rand.x = 0;
                    rand.y = Random.Range(0, 360);
                    rand.z = 0;
                    spellComponent.initialRotation = rand;
                    spellComponent.trajectoryNormalized = rand.normalized;
                    break;
            }
        }

        public static void CastSpellComponent(Entity entity, SpellComponent spellComponent, Vector3 startPosition, Entity target = null)
        {
            SetOriginalPosition(spellComponent, startPosition, entity, target);

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
                case TypeSpell.AreaOfEffect:
                    iSpellController = instance.areaOfEffectController;
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
            yield return new WaitForSeconds(spell.cooldown);
            spell.isOnCooldown = false;
            spell.alreadyRecast = false;
        }

        public static IEnumerator PlayCastTime(Entity entity, Spell spell, Vector3 startPosition, Entity target = null)
        {
            float duration = spell.castTime;

            if (spell.duringCastSpellComponent != null)
            {
                spell.canCastDuringCast = true;
            }

            while (duration > 0)
            {
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;

                if (spell.wantToCastDuringCast)
                {
                    spell.wantToCastDuringCast = false;
                    spell.canCastDuringCast = false;
                    
                    CastSpellComponent(entity, spell.duringCastSpellComponent, startPosition, target);
                    if (spell.interruptCurrentCast)
                    {
                        yield break;
                    }
                }
            }

            if (spell.activeSpellComponent == null)
            {
                yield break;
            }

            instance.StartCoroutine(StartCooldown(entity, spell));
            CastSpellComponent(entity, spell.activeSpellComponent, startPosition, target);
        }

        public static void CastPassiveSpell(Entity entity)
        {
            if (entity.spells == null)
            {
                return;
            }

            foreach (Spell spell in entity.spells)
            {
                if (spell.passiveSpellComponent != null)
                {
                    CastSpellComponent(entity, spell.passiveSpellComponent, entity.entityPrefab.positionPointed, entity.entityPrefab.target);
                }
            }
        }
    }
}