using System.Collections;
using Games.Global;
using Games.Global.Patterns;
using Games.Global.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Players
{
    public class PlayerPrefab : PlayerIntent, EffectInterface
    {
        private const int PLAYER_SPEED = 10;

        [SerializeField] private PlayerExposer playerExposer;
        [SerializeField] private Slider hpBar;
        [SerializeField] private Slider ressourcesBar;

        public int playerIndex;
        public bool canMove;

        private void Start()
        {
            Player player = new Player();

            entity = player;
            
            player.SetPlayerExposer(playerExposer);
            player.InitPlayerStats(Classes.Mage);
            player.effectInterface = this;

            wantToGoBack = false;
            wantToGoForward = false;
            wantToGoLeft = false;
            wantToGoRight = false;
            pressDefenseButton = false;

            StartCoroutine(NaturalRegen());
        }

        private IEnumerator NaturalRegen()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (entity.ressource1 < entity.initialRessource1)
                {
                    entity.ressource1 += 0.1f;
                }
            }
        }

        private void Update()
        {
            GetIntentPlayer();

            float diff = (float) entity.hp / (float) entity.initialHp;
            hpBar.value = diff;

            diff = (float) entity.ressource1 / (float) entity.initialRessource1;
            ressourcesBar.value = diff;
        }

        private void FixedUpdate()
        {
            Movement();
        }

        public void GetIntentPlayer()
        {
            if (entity.underEffects.ContainsKey(TypeEffect.Stun) || entity.underEffects.ContainsKey(TypeEffect.Sleep))
            {
                wantToGoBack = false;
                wantToGoForward = false;
                wantToGoLeft = false;
                wantToGoRight = false;
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                wantToGoForward = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                wantToGoBack = true;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                wantToGoLeft = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                wantToGoRight = true;
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                wantToGoForward = false;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                wantToGoBack = false;
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                wantToGoLeft = false;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                wantToGoRight = false;
            }

            if(Input.GetMouseButton(0))
            {
                entity.BasicAttack();
            }
            else if (Input.GetMouseButton(1))
            {
                entity.BasicDefense();
                pressDefenseButton = true;
            } 
            else if (pressDefenseButton)
            {
                entity.DesactiveBasicDefense();
                pressDefenseButton = false;
            }

            mousePosition = Input.mousePosition;
        }
        
        public void Movement()
        {
            Rigidbody rigidbody = playerExposer.playerRigidbody;

            int horizontalMove = 0;
            int verticalMove = 0;

            if(wantToGoForward)
            {
                verticalMove += 1;
            }
            else if(wantToGoBack)
            {
                verticalMove -= 1;
            }

            if(wantToGoLeft)
            {
                horizontalMove -= 1;
            }
            else if(wantToGoRight)
            {
                horizontalMove += 1;
            }

            Vector3 movement = rigidbody.velocity;
            movement.x = horizontalMove * PLAYER_SPEED;
            movement.z = verticalMove * PLAYER_SPEED;

            rigidbody.velocity = movement;

            Camera playerCamera = playerExposer.playerCamera.GetComponent<Camera>();
            RaycastHit hit;
            Ray cameraRay = playerCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(cameraRay, out hit))
            {
                Vector3 point = hit.point;
                point.y = 0;
                Transform playerTransform = playerExposer.playerTransform;
                playerTransform.LookAt(point);
                playerTransform.localEulerAngles = Vector3.up * playerTransform.localEulerAngles.y;
            }
        }
    }
}