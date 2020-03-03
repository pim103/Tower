using Games.Attacks;
using Games.Global;
using UnityEngine;

namespace Games
{
    public class ScriptsExposer : MonoBehaviour
    {
        [SerializeField]
        public InitAttackPhase initAttackPhase;

        [SerializeField]
        public DictionnaryManager dm;

        [SerializeField]
        public GameController gameController;
    }
}
