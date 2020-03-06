using System.Diagnostics;
using System.Linq;
using Games.Global;
using UnityEngine;

namespace Games.Players
{
    public class PlayerMovement : PlayerIntent
    {
        private const int PLAYER_SPEED = 10;

        [SerializeField]
        private Player player;

        [SerializeField] private PlayerExposer pe;

        public int playerIndex;
        public bool canMove;

        private void Start()
        {
            wantToGoBack = false;
            wantToGoForward = false;
            wantToGoLeft = false;
            wantToGoRight = false;
        }

        private void Update()
        {
            GetIntentPlayer();
        }

        private void FixedUpdate()
        {
            Movement();
        }

        public void GetIntentPlayer()
        {
            if (player.underEffects.ContainsKey(TypeEffect.Stun))
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
                player.BasicAttack();
            } 
            else if (Input.GetMouseButtonDown(1))
            {
                player.BasicDefense();
            } 
            else if (Input.GetMouseButtonUp(1))
            {
                player.DesactiveBasicDefense();
            }

            mousePosition = Input.mousePosition;
        }
        
        public void Movement()
        {
            Rigidbody rigidbody = pe.playerRigidbody;

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

            Camera playerCamera = pe.playerCamera.GetComponent<Camera>();
            RaycastHit hit;
            Ray cameraRay = playerCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(cameraRay, out hit))
            {
                Vector3 point = hit.point;
                point.y = 0;
                Transform playerTransform = pe.playerTransform;
                playerTransform.LookAt(point);
                playerTransform.localEulerAngles = Vector3.up * playerTransform.localEulerAngles.y;
            }
        }

        // OLD PUN RPC
        public void CheckMovementRPC(bool wantToGoForward, bool wantToGoBack, bool wantToGoLeft, bool wantToGoRight, Vector3 mousePosition)
        {
            this.wantToGoBack = wantToGoBack;
            this.wantToGoForward = wantToGoForward;
            this.wantToGoLeft = wantToGoLeft;
            this.wantToGoRight = wantToGoRight;
            this.mousePosition = mousePosition;
        }
    }
}