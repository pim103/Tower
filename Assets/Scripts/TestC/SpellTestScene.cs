using System.Collections;
using System.Collections.Generic;
using Games;
using Games.Global;
using Games.Global.Spells;
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
        
        // Start is called before the first frame update
        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            DataObject.playerInScene.Add(GameController.PlayerIndex, player);

            Identity classe = new Identity();
            classe.classe = Classes.Warrior;

            Identity weapon = new Identity();
            weapon.categoryWeapon = CategoryWeapon.BOW;

            ChooseDeckAndClasse.currentRoleIdentity = classe;
            ChooseDeckAndClasse.currentWeaponIdentity = weapon;

            StartCoroutine(Waiting());
        }

        private IEnumerator Waiting()
        {
            yield return new WaitForSeconds(0.5f);
            TestSpell();
        }

        private void TestSpell()
        {
            AreaOfEffectSpell linked = new AreaOfEffectSpell
            {
                startPosition = Vector3.zero,
                scale = Vector3.one * 20,
                onePlay = true,
                damagesOnEnemiesOnInterval = 30.0f,
                typeSpell = TypeSpell.AreaOfEffect,
                geometry = Geometry.Sphere,
            };

            Effect repulse = new Effect { typeEffect = TypeEffect.Expulsion, launcher = player.entity, level = 1, directionExpul = DirectionExpulsion.Out, originExpulsion = OriginExpulsion.SrcDamage};
            List<Effect> effects = new List<Effect>();
            effects.Add(repulse);
            
            AreaOfEffectSpell area = new AreaOfEffectSpell
            {
                startPosition = Vector3.zero,
                scale = Vector3.one * 20,
                duration = 5,
                interval = 1.5f,
                typeSpell = TypeSpell.AreaOfEffect,
                geometry = Geometry.Sphere,
                canStopProjectile = true,
                randomPosition = true,
                onePlay = true,
                damagesOnEnemiesOnInterval = 20.0f,
                effectOnHitOnStart = repulse,
                linkedSpellOnEnd = linked
            };

            player.entity.att = 15;
            SpellController.CastSpellComponent(player.entity, area);
        }
    }
}
