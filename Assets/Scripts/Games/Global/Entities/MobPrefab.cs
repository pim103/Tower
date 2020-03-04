using Games.Players;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace Games.Global.Entities
{
    public class MobPrefab : MonoBehaviour
    {
        [SerializeField] private GameObject hand;

        [SerializeField] private Slider hpBar;

        private PlayerExposer playerExposer;
 
        private Monster monster;

        private void Start()
        {
            ObjectsInScene ois = GameObject.Find("Controller").GetComponent<ObjectsInScene>();
            playerExposer = ois.playerExposer[GameController.PlayerIndex];
        }

        private void Update()
        {
            float diff = (float) monster.hp / (float) monster.initialHp;
            hpBar.value = diff;
            hpBar.transform.LookAt(playerExposer.playerCamera.transform);
            hpBar.transform.Rotate(Vector3.up * 180);
            
            monster.instantiateModel.transform.LookAt(playerExposer.playerTransform);
        }

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
