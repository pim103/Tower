using System.Collections;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Transitions;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Games.Players
{
    public class PlayerPrefab : PlayerIntent
    {
        private const int PLAYER_SPEED = 10;

        [SerializeField] private LayerMask interactWithCamera;
        
        [SerializeField] private Slider hpBar;
        [SerializeField] private Slider ressourcesBar;

        [SerializeField] public GameObject playerGameObject;
        [SerializeField] public Transform playerTransform;
        [SerializeField] private Rigidbody playerRigidbody;
        
        [SerializeField] public Camera camera;
        [SerializeField] private SkinnedMeshRenderer[] skins;
        
        private Material[] materialBackUpForSkin;

        [SerializeField] public Text spell1;
        [SerializeField] public Text spell2;
        [SerializeField] public Text spell3;
        [SerializeField] public Text timer1;
        [SerializeField] public Text timer2;
        [SerializeField] public Text timer3;
        [SerializeField] public GameObject bgTimer1;
        [SerializeField] public GameObject bgTimer2;
        [SerializeField] public GameObject bgTimer3;

        [SerializeField] public Text timerAttack;

        [SerializeField] public List<Image> buffCases;

        // Use for some spell
        [SerializeField] private bool isFakePlayer = false;

        public int playerIndex;

        public bool grounded;
        public bool ejected;

        public bool wasConfusing = false;

        private bool doingAttack = false;

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
            if (isFakePlayer)
            {
                return;
            }
            
            Player player = new Player();

            entity = player;
            entity.entityPrefab = this;

            player.SetPlayerPrefab(this);
            player.InitPlayer(ChooseDeckAndClass.currentRoleIdentity.GetIdentityId());

            wantToGoBack = false;
            wantToGoForward = false;
            wantToGoLeft = false;
            wantToGoRight = false;
            pressDefenseButton = false;

            StartCoroutine(NaturalRegen());
        }

        private IEnumerator NaturalRegen()
        {
            float count = 0;
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                count += 0.1f;

                if (entity.ressource < entity.initialRessource1)
                {
                    entity.ressource += 0.1f;
                }

                if (entity.nbCharges < 4 && count >= 0.5f)
                {
                    entity.nbCharges++;
                    count = 0;
                }
            }
        }

        private void Update()
        {
            if (isFakePlayer)
            {
                return;
            }
            GetIntentPlayer();

            float diff = (float) entity.hp / (float) entity.initialHp;
            hpBar.value = diff;

            diff = (float) entity.ressource / (float) entity.initialRessource1;
            ressourcesBar.value = diff;

            UpdateCooldown();
        }

        private void UpdateCooldown()
        {
            int slot = 0;
            foreach (Spell spell in entity.spells)
            {
                GameObject bgTimer = null;
                Text timer = null;
             
                ++slot;
                
                switch (slot)
                {
                    case 1:
                        bgTimer = bgTimer1;
                        timer = timer1;
                        break;
                    case 2:
                        bgTimer = bgTimer2;
                        timer = timer2;
                        break;
                    case 3:
                        bgTimer = bgTimer3;
                        timer = timer3;
                        break;
                }

                if (spell.isOnCooldown)
                {
                    if (timer != null && bgTimer != null)
                    {
                        bgTimer.SetActive(true);
                    }
                    if (timer != null)
                    {
                        timer.text = spell.currentCooldown.ToString();
                    }
                } else if (bgTimer != null && bgTimer.activeSelf)
                {
                    bgTimer.SetActive(false);
                }
            }
        }
        
        private void FixedUpdate()
        {
            if (isFakePlayer)
            {
                Movement();
                return;
            }

            Movement();
            CameraRotation();

            if (Physics.Raycast(playerTransform.position, (camera.transform.up * -1), 0.1f,
                LayerMask.GetMask("Ground")))
            {
                if (grounded)
                {
                    ejected = false;
                }
                grounded = true;
            }
            else
            {
                grounded = false;
            }

        }

        public void GetIntentPlayer()
        {
            if (!canDoSomething || entity.isFeared || entity.isCharmed)
            {
                wantToGoBack = false;
                wantToGoForward = false;
                wantToGoLeft = false;
                wantToGoRight = false;
                return;
            }

            if (wasConfusing != entity.isConfuse)
            {
                bool tempIntent = wantToGoBack;
                wantToGoBack = wantToGoForward;
                wantToGoForward = tempIntent;

                tempIntent = wantToGoLeft;
                wantToGoLeft = wantToGoRight;
                wantToGoRight = tempIntent;

                wasConfusing = entity.isConfuse;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (entity.isConfuse)
                {
                    wantToGoBack = true;
                }
                else
                {
                    wantToGoForward = true;   
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (entity.isConfuse)
                {
                    wantToGoForward = true;
                }
                else
                {
                    wantToGoBack = true;   
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (entity.isConfuse)
                {
                    wantToGoRight = true;
                }
                else
                {
                    wantToGoLeft = true;   
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (entity.isConfuse)
                {
                    wantToGoLeft = true;
                }
                else
                {
                    wantToGoRight = true;   
                }
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                if (entity.isConfuse)
                {
                    wantToGoBack = false;
                }
                else
                {
                    wantToGoForward = false;   
                }
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                if (entity.isConfuse)
                {
                    wantToGoForward = false;
                }
                else
                {
                    wantToGoBack = false;   
                }
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                if (entity.isConfuse)
                {
                    wantToGoRight = false;
                }
                else
                {
                    wantToGoLeft = false;   
                }
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                if (entity.isConfuse)
                {
                    wantToGoLeft = false;
                }
                else
                {
                    wantToGoRight = false;   
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                wantToJump = true;
            } else if (Input.GetKeyUp(KeyCode.Space))
            {
                wantToJump = false;
            }

            if (!canMove)
            {
                wantToGoBack = false;
                wantToGoForward = false;
                wantToGoLeft = false;
                wantToGoRight = false;
            }

            animator.SetBool("isWalking", wantToGoBack | wantToGoForward | wantToGoLeft | wantToGoRight);
            if (wantToGoBack | wantToGoForward | wantToGoLeft | wantToGoRight)
            {
                //audioSource.PlayOneShot(footstepsClip);
                if (footstepsRoutine == null)
                {
                    footstepsRoutine = StartCoroutine(PlayFootsteps());
                }
            }
            else
            {
                if (footstepsRoutine != null)
                {
                    StopCoroutine(footstepsRoutine);
                }

                footstepsRoutine = null;
            }

            if (!intentBlocked)
            {
                if(Input.GetMouseButton(0) && entity.canBasicAttack)
                {
                    doingAttack = true;
                    entity.BasicAttack();
                } 
                else if (doingAttack)
                {
                    doingAttack = false;
                    CancelBasicAttack();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    SpellController.CastSpell(entity, entity.basicDefense);
                    pressDefenseButton = true;
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    SpellController.InterruptSpell(entity.basicDefense);
                    pressDefenseButton = false;
                }

                if (!entity.doingSkill && !entity.isSilence)
                {
                    if (Input.GetKey(KeyCode.Alpha1))
                    {
                    }

                    if (Input.GetKey(KeyCode.Alpha2))
                    {
                    }

                    if (Input.GetKey(KeyCode.Alpha3))
                    {
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        wantToCastSpell1 = true;
                        SpellController.CastSpell(entity, entity.spells[0]);
                    }
                    if (Input.GetKeyUp(KeyCode.Alpha1))
                    {
                        wantToCastSpell1 = false;
                        SpellController.InterruptSpell(entity.spells[0]);
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        wantToCastSpell2 = true;
                        SpellController.CastSpell(entity, entity.spells[1]);
                    }
                    else if (Input.GetKeyUp(KeyCode.Alpha2))
                    {
                        wantToCastSpell2 = false;
                        SpellController.InterruptSpell(entity.spells[1]);
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        wantToCastSpell3 = true;
                        SpellController.CastSpell(entity, entity.spells[2]);
                    }
                    else if (Input.GetKeyUp(KeyCode.Alpha3))
                    {
                        wantToCastSpell3 = false;
                        SpellController.InterruptSpell(entity.spells[2]);
                    }
                }
            }

            mousePosition = Input.mousePosition;
        }

        public void Movement()
        {
            Rigidbody rigidbody = playerRigidbody;
            int jumpForce = 10;

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


            if (grounded && wantToJump)
            {
                locVel.y = jumpForce;
            }

            if (!ejected)
            {
                rigidbody.velocity = transform.TransformDirection(locVel);
            }
        }

        private void CameraRotation()
        {
            if (entity.isFeared)
            {
                Entity launcher = entity.TryGetEffectInUnderEffect(TypeEffect.Fear).launcher;
                transform.LookAt(launcher.entityPrefab.transform);
                transform.Rotate(Vector3.up * 180);
                return;
            }
            
            if(entity.isCharmed)
            {
                Entity launcher = entity.TryGetEffectInUnderEffect(TypeEffect.Charm).launcher;
                transform.LookAt(launcher.entityPrefab.transform);
                return;
            }

            if (cameraBlocked)
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
            if (Physics.Raycast(ray, out hit, 1000, ~LayerMask.GetMask("Player", "Spell", "Weapon")))
            {
                positionPointed = hit.point;

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    EntityPrefab entityPrefab = hit.collider.GetComponent<ColliderEntityExposer>().entityPrefab;
                    if (entityPrefab.entity.GetTypeEntity() == TypeEntity.MOB)
                    {
                        target = entityPrefab.entity;
                    }
                }
            }
            else
            {
                positionPointed = Vector3.positiveInfinity;
            }

            // if (Physics.Raycast(playerTransform.position, (camera.transform.forward * -1), out hit, 6, interactWithCamera))
            // {
            //     camera.transform.position = hit.point;
            //     camera.transform.position += camera.transform.forward * 0.2f;
            // }
            // else
            // {
            //     camera.transform.position = originalCameraPosition.transform.position;
            // }
        }

        public override void SetInvisibility()
        {
            int count = 0;

            materialBackUpForSkin ??= new Material[skins.Length];
            
            foreach (SkinnedMeshRenderer skin in skins)
            {
                if (entity.isInvisible)
                {
                    materialBackUpForSkin[count] = skin.material;
                    skin.material = StaticMaterials.invisibleMaterial;
                }
                else
                {
                    skin.material = materialBackUpForSkin[count];
                }

                count++;
            }
        }
    }
}