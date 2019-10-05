﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Players
{
    public abstract class PlayerIntent : MonoBehaviour
    {
        public bool wantToGoForward;
        public bool wantToGoBack;
        public bool wantToGoLeft;
        public bool wantToGoRight;
    }
}