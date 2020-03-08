using Games.Global;
using UnityEngine;

namespace Games.Players
{
    public abstract class PlayerIntent : MonoBehaviour
    {
        public bool wantToGoForward;
        public bool wantToGoBack;
        public bool wantToGoLeft;
        public bool wantToGoRight;
        public Vector3 mousePosition;

        public bool pressDefenseButton;
    }
}