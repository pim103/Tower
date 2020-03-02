using UnityEngine;

namespace Games.Global.Entities
{
    public class MobPrefab : MonoBehaviour
    {
        private Monster monster;
        
        public void SetMonster(Monster monster)
        {
            this.monster = monster;
        }

        public Monster GetMonster()
        {
            return monster;
        }
    }
}
