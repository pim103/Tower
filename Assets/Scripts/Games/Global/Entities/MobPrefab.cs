using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace Games.Global.Entities
{
    public class MobPrefab : MonoBehaviour
    {
        [SerializeField] private GameObject hand;

        [SerializeField] private Slider hpBar;

        private Transform cameraTransform;
 
        private Monster monster;

        private void Start()
        {
            ObjectsInScene ois = GameObject.Find("Controller").GetComponent<ObjectsInScene>();
            cameraTransform = ois.playerExposer[GameController.PlayerIndex].playerCamera.transform;
        }

        private void Update()
        {
            float diff = (float) monster.hp / (float) monster.initialHp;
            hpBar.value = diff;
            hpBar.transform.LookAt(cameraTransform);
            hpBar.transform.Rotate(Vector3.up * 180);
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
