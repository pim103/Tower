using Scripts;
using Games.Attacks;
using Games.Global;
using Games.Global.Weapons;
using UnityEngine;

namespace Games
{
    public class ScriptsExposer : MonoBehaviour
    {
        [SerializeField]
        public PhotonController photonController;
        
        [SerializeField]
        public InitAttackPhase initAttackPhase;

        [SerializeField]
        public DictionnaryManager dm;

        [SerializeField]
        public GameController gameController;
    }
}
