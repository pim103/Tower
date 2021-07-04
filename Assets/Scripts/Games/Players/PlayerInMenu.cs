using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Games.Transitions;
using Menus;
using Networking;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Debug = UnityEngine.Debug;

namespace Games.Players
{
    public class PlayerInMenu : PlayerIntent
    {
        private const int PLAYER_SPEED = 2;

        [SerializeField] private MenuController mc;
        [SerializeField] private GameObject informationLayout;
        [SerializeField] private LayerMask interactWithCamera;

        [SerializeField] public GameObject playerGameObject;
        [SerializeField] public Transform playerTransform;
        [SerializeField] private Rigidbody playerRigidbody;
        
        [SerializeField] public Camera camera;
        [SerializeField] private BoxCollider[] collidersZones;

        public static bool isInMenu = true;
        private bool canGoInCollectionMenu, canGoInCraftMenu, canGoInPlayMenu;

        public void Reset()
        {
            wantToGoBack = false;
            wantToGoForward = false;
            wantToGoLeft = false;
            wantToGoRight = false;
            pressDefenseButton = false;

            Player player = entity as Player;
            player?.ResetStats();
        }
        
        private void Start()
        {

            Player player = new Player();

            entity = player;
            entity.entityPrefab = this;

            wantToGoBack = false;
            wantToGoForward = false;
            wantToGoLeft = false;
            wantToGoRight = false;
            pressDefenseButton = false;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isInMenu = !isInMenu;
                Cursor.lockState = isInMenu ? CursorLockMode.None : CursorLockMode.Locked;
            }
            if (NetworkingController.AuthToken == "")
            {
                return;
            }

            if (isInMenu && informationLayout.activeSelf)
            {
                informationLayout.SetActive(false);
            }

            GetIntentPlayer();
        }

        private void FixedUpdate()
        {
            Movement();
            CameraRotation();

            if (Physics.Raycast(playerTransform.position, (camera.transform.up * -1), 0.1f,
                LayerMask.GetMask("Ground")))
            {
                Debug.Log("TestRayCast");
            }

        }

        public void GetIntentPlayer()
        {
            if (isInMenu)
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (canGoInCraftMenu)
                {
                    mc.ActivateMenu(MenuController.Menu.Craft);
                }
                if (canGoInCollectionMenu)
                {
                    mc.ActivateMenu(MenuController.Menu.DeckManagement);
                }
                if (canGoInPlayMenu)
                {
                    mc.ActivateMenu(MenuController.Menu.Play);
                }
            }

            if (!canMove)
            {
                wantToGoBack = false;
                wantToGoForward = false;
                wantToGoLeft = false;
                wantToGoRight = false;
            }

            animator.SetBool("isWalking", wantToGoBack | wantToGoForward | wantToGoLeft | wantToGoRight);

            mousePosition = Input.mousePosition;
        }

        public void Movement()
        {
            Rigidbody rigidbody = playerRigidbody;
            int jumpForce = 6;

            int horizontalMove = 0;
            int verticalMove = 0;

            if (wantToGoForward)
            {
                verticalMove += 1;
            }
            else if (wantToGoBack)
            {
                verticalMove -= 1;
            }

            if (wantToGoLeft)
            {
                horizontalMove -= 1;
            }
            else if (wantToGoRight)
            {
                horizontalMove += 1;
            }

            float currentSpeed = 10;
            if (entity != null)
            {
                currentSpeed = entity.speed;
            }

            Vector3 locVel;
            if ( (entity.isFeared || entity.isCharmed) && canDoSomething)
            {
                locVel = transform.InverseTransformDirection(forcedDirection);
                locVel *= currentSpeed;
            }
            else
            {
                locVel = transform.InverseTransformDirection(rigidbody.velocity);
                locVel.x = horizontalMove * currentSpeed;
                locVel.z = verticalMove * currentSpeed;
            }
            rigidbody.velocity = transform.TransformDirection(locVel);
        }

        private void CameraRotation()
        {
            if (cameraBlocked)
            {
                return;
            }
            
            if (NetworkingController.AuthToken == "" || isInMenu)
            {
                return;
            }

            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.x = 0;
            eulerAngles.z = 0;
            transform.localEulerAngles = eulerAngles;

            float rotationSpeed = 5;
            
            
            if (isCharging)
            {
                rotationSpeed = 0.1f;
            }
            
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            float vertical = Input.GetAxis("Mouse Y") * rotationSpeed;

            playerGameObject.transform.Rotate(0, horizontal, 0);

            GameObject cameraPoint = camera.gameObject;
            
            vertical = Mathf.Clamp(vertical, -90f, 90f);            
            cameraPoint.transform.Rotate(-vertical, 0, 0, Space.Self);
            
            Quaternion q = cameraPoint.transform.localRotation;
            
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, -75, 55);
            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            cameraPoint.transform.localRotation = q;

            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            if (Physics.Raycast(ray, out hit, 15))
            {
                positionPointed = hit.point;

                if (hit.collider.gameObject.name == "Craft")
                {
                    informationLayout.SetActive(true);
                    canGoInCraftMenu = true;
                }
                else if (hit.collider.gameObject.name == "Collection")
                {
                    informationLayout.SetActive(true);
                    canGoInCollectionMenu = true;
                }
                else if (hit.collider.gameObject.name == "Play")
                {
                    informationLayout.SetActive(true);
                    canGoInPlayMenu = true;
                }
                else if (informationLayout.activeSelf)
                {
                    informationLayout.SetActive(false);
                    canGoInCraftMenu = false;
                    canGoInCollectionMenu = false;
                    canGoInPlayMenu = false;
                }

            }
            else
            {
                positionPointed = Vector3.positiveInfinity;
            }
        }
    }
}