using Games.Players;
using UnityEngine;

namespace Games.Global.Entities
{
    public class MonsterBack : MonoBehaviour
    {
        [SerializeField] private MonsterPrefab monsterPrefab;
        
        public void OnTriggerEnter(Collider other)
        {
            int layerPlayer = LayerMask.NameToLayer("Player");

            if (other.gameObject.layer == layerPlayer)
            {
                PlayerPrefab playerPrefab = other.GetComponent<PlayerPrefab>();
                monsterPrefab.entity.playerInBack.Add(playerPrefab.entity.IdEntity);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            int layerPlayer = LayerMask.NameToLayer("Player");

            if (other.gameObject.layer == layerPlayer)
            {
                PlayerPrefab playerPrefab = other.GetComponent<PlayerPrefab>();
                monsterPrefab.entity.playerInBack.Remove(playerPrefab.entity.IdEntity);
            }
        }
    }
}
