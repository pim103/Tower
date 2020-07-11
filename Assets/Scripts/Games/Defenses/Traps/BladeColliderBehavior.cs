using Games.Global.Abilities;
using Games.Global.Spells;
using Games.Players;
using UnityEngine;

namespace Games.Defenses.Traps
{
public class BladeColliderBehavior : MonoBehaviour
{
    [SerializeField] private SpinningPoleBehavior spinningPoleBehavior;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            spinningPoleBehavior.playerEntity = other.gameObject.GetComponent<PlayerPrefab>().entity;
            AbilityParameters abilityParameters = new AbilityParameters();
            abilityParameters.origin = spinningPoleBehavior.entity;
            spinningPoleBehavior.playerEntity = other.gameObject.GetComponent<PlayerPrefab>().entity;
            spinningPoleBehavior.playerEntity.TakeDamage(5.0f, abilityParameters, DamageType.Physical);
        }
    }
}
}
