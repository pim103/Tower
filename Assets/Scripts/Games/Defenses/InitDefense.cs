using Scripts.Games.Transitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Defenses
{
    public class InitDefense : MonoBehaviour
    {
        [SerializeField]
        private TransitionDefenseAttack transitionDefenseAttack;

        // Start is called before the first frame update
        void Start()
        {
            transitionDefenseAttack.StartDefenseCounter();
        }
    }
}