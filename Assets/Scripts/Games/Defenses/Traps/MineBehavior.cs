using Games.Global;
using Games.Global.Abilities;
using Games.Global.Spells;
using Games.Players;
using UnityEngine;

namespace Games.Defenses.Traps {
  public class MineBehavior : MonoBehaviour {
    private Entity playerEntity;
    private Entity entity;
    private void Start() {
      entity = new Entity{
          entityPrefab = gameObject.AddComponent<EntityPrefab>(),
          BehaviorType = BehaviorType.Player, typeEntity = TypeEntity.MOB};
      entity.entityPrefab.entity = entity;
    }
    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
        AbilityParameters abilityParameters = new AbilityParameters();
        abilityParameters.origin = entity;
        playerEntity = other.gameObject.GetComponent<PlayerPrefab>().entity;
        playerEntity.TakeDamage(20.0f, abilityParameters, DamageType.Physical);
        gameObject.SetActive(false);
      }
    }
  }
}
