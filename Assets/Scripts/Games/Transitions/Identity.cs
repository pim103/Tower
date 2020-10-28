using Games.Global;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;

namespace Games.Transitions
{
    public enum IdentityType
    {
        Role,
        CategoryWeapon
    }
    
    public class Identity: MonoBehaviour
    {
        [SerializeField] public IdentityType identityType;

        [SerializeField] public CategoryWeapon categoryWeapon;
        [SerializeField] public Classes classe;
    }
}