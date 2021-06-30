﻿using System;
using PathCreation;
using UnityEngine;

namespace Games.Global.Spells
{
    public enum MovementSpellType
    {
        Dash,
        Charge,
        Tp,
        TpWithTarget
    }

    public enum Direction
    {
        UseTrajectory,
        Forward,
        Backward
    }

    [Serializable]
    public class MovementSpell : SpellComponent
    {
        public MovementSpell()
        {
            TypeSpellComponent = TypeSpellComponent.Movement;
        }

        public bool isFollowingMouse { get; set; }
        public MovementSpellType movementSpellType { get; set; }
        public Direction direction { get; set; }

        private float distanceTravelled;
        
        private void DoTpAtTarget(Entity entity, Entity target)
        {
            Vector3 newPosition = target.entityPrefab.transform.position;
            
            entity.entityPrefab.transform.LookAt(target.entityPrefab.transform);
            entity.entityPrefab.transform.forward = target.entityPrefab.transform.forward;
            newPosition -= target.entityPrefab.transform.forward;
     
            entity.entityPrefab.transform.position = newPosition;
        }
        
        private void DoTp(Entity entity)
        {
            Vector3 newPosition = startAtPosition;

            entity.entityPrefab.transform.position = newPosition;
        }

        public override void AtTheStart()
        {
            if (!isFollowingMouse)
            {
                caster.entityPrefab.cameraBlocked = true;
            }
            switch (movementSpellType)
            {
                case MovementSpellType.TpWithTarget:
                    DoTpAtTarget(caster, targetAtCast);
                    break;
                case MovementSpellType.Tp:
                    DoTp(caster);
                    break;
                case MovementSpellType.Charge:
                    caster.entityPrefab.isCharging = true;
                    break;
            }
        }

        public override void DuringInterval()
        {
            if (movementSpellType == MovementSpellType.Dash || movementSpellType == MovementSpellType.Charge)
            {
                if (isFollowingMouse)
                {
                    caster.entityPrefab.transform.position += (caster.entityPrefab.transform.forward);
                } else if (direction == Direction.Forward)
                {
                    caster.entityPrefab.GetComponent<Rigidbody>().AddForce(caster.entityPrefab.transform.forward * 300, ForceMode.Impulse);
                    // caster.entityPrefab.transform.position += (caster.entityPrefab.transform.forward * 10);
                } else if (direction== Direction.Backward)
                {
                    caster.entityPrefab.GetComponent<Rigidbody>().AddForce(-caster.entityPrefab.transform.forward * 300, ForceMode.Impulse);
                    // caster.entityPrefab.transform.position -= (caster.entityPrefab.transform.forward * 10);
                } else if (trajectory.spellPath != null)
                {
                    distanceTravelled += trajectory.speed * spellInterval;
                    caster.entityPrefab.transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
                }
            }
        }

        public override void AtTheEnd()
        {
            caster.entityPrefab.canMove = true;
            caster.entityPrefab.cameraBlocked = false;
            caster.entityPrefab.isCharging = false;
        }
    }
}