using Games.Transitions;
using UnityEngine;

namespace Games.Defenses
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