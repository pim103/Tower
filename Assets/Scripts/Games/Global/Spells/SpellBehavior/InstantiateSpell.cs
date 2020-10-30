using Games.Global.Spells.SpellParameter;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global.Spells.SpellBehavior
{
    public class InstantiateSpell
    {
        public static void InstantiateNewSpell(SpellComponent spellComponent, Vector3 startPosition)
        {
            SpellToInstantiate spellToInstantiate = spellComponent.spellToInstantiate;

            if (spellToInstantiate == null)
            {
                return;
            }

            GameObject genericSpellPrefab = ObjectPooler.SharedInstance.GetPooledObject(1);
            genericSpellPrefab.transform.localScale = spellToInstantiate.scale;
            // TODO :
            // Position de départ ?
            // Rotation ?
            
            SpellPrefabController spellPrefabController = genericSpellPrefab.GetComponent<SpellPrefabController>();
            spellPrefabController.ActiveCollider(spellToInstantiate.geometry);
            spellPrefabController.SetValues(spellComponent.caster, spellComponent);

            spellComponent.spellPrefabController = spellPrefabController;
        }
    }
}