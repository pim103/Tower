using System.Collections.Generic;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using UnityEngine;
using Utils;

namespace SpellEditor
{
    public class RandomSpellGenerator
    {
        public Spell RandomSpell()
        {
            Spell spell = new Spell
            {
                nameSpell = "RandomSpell-" + System.DateTime.Now,
                cooldown = Random.Range(1, 10),
                cost = Random.Range(1, 10),
//                castTime = Random.Range(0, 5),
                activeSpellComponent = GenerateRandomSpellComponent(),
            };

            return spell;
        }

        public SpellComponent GenerateRandomSpellComponent()
        {
            List<TypeSpell> possibleActiveSpell = new List<TypeSpell>
            {
                TypeSpell.Projectile,
                TypeSpell.AreaOfEffect,
                TypeSpell.Wave,
            };

            TypeSpell randomType = possibleActiveSpell[Random.Range(0, possibleActiveSpell.Count)];

            SpellComponent newSpellComponent = null;
            
            switch (randomType)
            {
                case TypeSpell.Projectile:
                    newSpellComponent = InitRandomProjectileSpell();
                    break;
                case TypeSpell.AreaOfEffect:
                    newSpellComponent = InitRandomAreaOfEffectSpell();
                    break;
                case TypeSpell.Wave:
                    newSpellComponent = InitRandomWaveSpell();
                    break;
            }

            return newSpellComponent;
        }

        public ProjectileSpell InitRandomProjectileSpell()
        {
            return new ProjectileSpell
            {
                idPoolObject = 0,
                speed = Random.Range(10, 20),
                damages = Random.Range(1, 50),
                duration = Random.Range(5, 10),
                damageType = (DamageType) Random.Range(0, 2),
                OriginalDirection = OriginalDirection.Forward,
                OriginalPosition = (OriginalPosition) Random.Range(0, 4),
                passingThroughEntity = (Random.Range(0, 2) == 1),
                damageMultiplierOnDistance = Random.Range(1, 2),
                nameSpellComponent = "Projectile-" + System.DateTime.Now.Millisecond,
                needPositionToMidToEntity = true,
                linkedSpellOnDisable = GenerateRandomSpellComponent(),
            };
        }
        
        public AreaOfEffectSpell InitRandomAreaOfEffectSpell()
        {
            int randomScale = Random.Range(1, 20);
            
            return new AreaOfEffectSpell
            {
                duration = Random.Range(2, 10),
                interval = Random.Range(0, 2),
                geometry = Geometry.Sphere,
                scale = new Vector3(randomScale, 1, randomScale),
                originArea = (OriginArea) Random.Range(0, 2),
                damagesOnEnemiesOnInterval = Random.Range(5, 25),
                onePlay = (Random.Range(0, 2) == 1),
                canStopProjectile = (Random.Range(0, 2) == 1),
                wantToFollow = (Random.Range(0, 2) == 1),
                damageType = (DamageType) Random.Range(0, 2),
                OriginalDirection = OriginalDirection.None,
                OriginalPosition = (OriginalPosition) Random.Range(0, 4),
                nameSpellComponent = "AreaOfEffect-" + System.DateTime.Now.Millisecond,
                linkedSpellOnEnd = GenerateRandomSpellComponent(),
            };
        }
        
        public WaveSpell InitRandomWaveSpell()
        {
            return new WaveSpell
            {
                damages = Random.Range(1, 50),
                duration = Random.Range(5, 10),
                damageType = (DamageType) Random.Range(0, 2),
                geometryPropagation = Geometry.Square,
                initialWidth = Random.Range(3, 10),
                speedPropagation = Random.Range(5, 15),
                incrementAmplitudeByTime = Random.Range(2, 6),
                OriginalDirection = OriginalDirection.Forward,
                OriginalPosition = (OriginalPosition) Random.Range(0, 4),
                nameSpellComponent = "Wave-" + System.DateTime.Now.Millisecond,
            };
        }
    }
}