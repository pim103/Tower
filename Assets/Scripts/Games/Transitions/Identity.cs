﻿using UnityEngine;

namespace Games.Transitions
{
    public enum IdentityType
    {
        Role,
        CategoryWeapon
    }

    public class Identity: MonoBehaviour
    {
        public IdentityType identityType;
        private int identityId;

        public void InitIdentityData(IdentityType type, int id)
        {
            identityType = type;
            identityId = id;
        }

        public int GetIdentityId()
        {
            return identityId;
        }
    }
}