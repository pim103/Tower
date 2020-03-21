using System;
using System.Collections;
using System.Collections.Generic;
using Games.Global.Entities;
using Games.Players;
using UnityEngine;

namespace Games.Global.Abilities.SpecialSpellPrefab.Dagger
{
    public class RogueClone : MonoBehaviour
    {
        [SerializeField]
        private PlayerPrefab fakePlayerPrefab;

        private int duration;

        private List<MonsterPrefab> monsterForcedAggro;
        
        private IEnumerator TimeBeforeDisapear()
        {
            yield return new WaitForSeconds(duration);
            
            LeaveAllMob();
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            duration = 5;
            StartCoroutine(TimeBeforeDisapear());

            monsterForcedAggro = new List<MonsterPrefab>();
            fakePlayerPrefab.wantToGoForward = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            int monsterLayer = LayerMask.NameToLayer("Monster");

            if (other.gameObject.layer == monsterLayer)
            {
                MonsterPrefab monsterPrefab = other.GetComponent<MonsterPrefab>();
                monsterPrefab.target = fakePlayerPrefab;
                monsterPrefab.aggroForced = true;
                
                monsterForcedAggro.Add(monsterPrefab);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            int monsterLayer = LayerMask.NameToLayer("Monster");

            if (other.gameObject.layer == monsterLayer)
            {
                MonsterPrefab monsterPrefab = other.GetComponent<MonsterPrefab>();
                LeaveAggro(monsterPrefab);
                monsterForcedAggro.Remove(monsterPrefab);
            }
        }

        private void LeaveAllMob()
        {
            foreach (MonsterPrefab monsterPrefab in monsterForcedAggro)
            {
                LeaveAggro(monsterPrefab);
            }
        }

        private void LeaveAggro(MonsterPrefab monsterPrefab)
        {
            monsterPrefab.target = null;
            monsterPrefab.aggroForced = false;
        }
    }
}
