using Games.Global;
using UnityEngine;

namespace Games.Players
{
    public abstract class PlayerIntent : EntityPrefab
    {
        public bool wantToGoForward;
        public bool wantToGoBack;
        public bool wantToGoLeft;
        public bool wantToGoRight;
        public bool wantToJump;
        public Vector3 mousePosition;

        public bool pressDefenseButton;
    }
}