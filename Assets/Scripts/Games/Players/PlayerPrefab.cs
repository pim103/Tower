using System.Collections;
using System.Diagnostics;
using Games.Global;
using Games.Global.Weapons;
using Games.Transitions;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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
        
        [SerializeField] public GameObject cameraPoint;
        [SerializeField] public Camera camera;
        [SerializeField] public GameObject cameraGameObject;
        [SerializeField] private GameObject originalCameraPosition;

        // Use for some spell
        [SerializeField] private bool isFakePlayer = false;

        public int playerIndex;

        private Vector3 positionPointed;

        public bool grounded;
        public bool ejected;

        public bool wasConfusing = false;

        private void Awake()
        {
            if (isFakePlayer)
            {
                return;
            }
            
            Player player = new Player();

            entity = player;
            entity.entityPrefab = this;

            player.SetPlayerPrefab(this);
            player.InitPlayerStats(ChooseDeckAndClasse.currentRoleIdentity.classe);

            wantToGoBack = false;
            wantToGoForward = false;
            wantToGoLeft = false;
            wantToGoRight = false;
            pressDefenseButton = false;

            StartCoroutine(NaturalRegen());

            CheckPassiveSpell(entity.weapons[0].skill1);
            CheckPassiveSpell(entity.weapons[0].skill2);
            CheckPassiveSpell(entity.weapons[0].skill3);
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
            if (isFakePlayer)
            {
                return;
            }
            GetIntentPlayer();

            float diff = (float) entity.hp / (float) entity.initialHp;
            hpBar.value = diff;

            diff = (float) entity.ressource1 / (float) entity.initialRessource1;
            ressourcesBar.value = diff;
        }

        private void FixedUpdate()
        {
            if (isFakePlayer)
            {
                Movement();
                return;
            }

            if (!entity.underEffects.ContainsKey(TypeEffect.MadeADash) && !movementBlocked)
            {
                Movement();
            }

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

            if (!canMove)
            {
                wantToGoBack = false;
                wantToGoForward = false;
                wantToGoLeft = false;
                wantToGoRight = false;
            }
            
            animator.SetBool("isWalking", wantToGoBack | wantToGoForward | wantToGoLeft | wantToGoRight);

            if (!intentBlocked)
            {
                if(Input.GetMouseButton(0))
                {
                    entity.BasicAttack();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    entity.BasicDefense();
                    pressDefenseButton = true;
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    entity.DesactiveBasicDefense();
                    pressDefenseButton = false;
                }

                if (!entity.doingSkill && !entity.isSilence)
                {
                    if (Input.GetKey(KeyCode.Alpha1))
                    {
                        TryCastSpell(entity.weapons[0].skill1, true);
                    }

                    if (Input.GetKey(KeyCode.Alpha2))
                    {
                        TryCastSpell(entity.weapons[0].skill2, true);
                    }

                    if (Input.GetKey(KeyCode.Alpha3))
                    {
                        TryCastSpell(entity.weapons[0].skill3, true);
                    }

                    if (Input.GetKeyUp(KeyCode.Alpha1))
                    {
                        TryCastSpell(entity.weapons[0].skill1);
                    }

                    if (Input.GetKeyUp(KeyCode.Alpha2))
                    {
                        TryCastSpell(entity.weapons[0].skill2);
                    }

                    if (Input.GetKeyUp(KeyCode.Alpha3))
                    {
                        TryCastSpell(entity.weapons[0].skill3);
                    }
                }
            }

            mousePosition = Input.mousePosition;
        }

        private void TryCastSpell(Spell spell, bool isPreview = false)
        {
            if (spell.typeSpell == TypeSpell.Active ||
                spell.typeSpell == TypeSpell.Toggle ||
                spell.typeSpell == TypeSpell.ActiveWithPassive ||
                spell.typeSpell == TypeSpell.ToggleWithPassive
            )
            {
                if (spell.cost < entity.ressource1 && spell.canLaunch)
                {
                    if (isPreview)
                    {
                        ActivePreviewSpell(spell);
                    }
                    else
                    {
                        StartCoroutine(Cooldown(spell));
                        StartCoroutine(CastSpell(spell));   
                    }
                }
            }
        }

        private void CheckPassiveSpell(Spell spell)
        {
            if (spell == null)
            {
                return;
            }

            if (spell.typeSpell == TypeSpell.Passive || spell.typeSpell == TypeSpell.ActiveWithPassive)
            {
                foreach (SpellInstruction spellInstruction in spell.spellInstructions)
                {
                    if ((spell.typeSpell == TypeSpell.ActiveWithPassive &&
                        spellInstruction.specificTypeSpell == TypeSpell.Passive) || spell.typeSpell == TypeSpell.Passive)
                    {
                        switch (spellInstruction.TypeSpellInstruction)
                        {
                            case TypeSpellInstruction.SelfEffect:
                                EffectController.ApplyEffect(entity, spellInstruction.effect);
                                break;
                            case TypeSpellInstruction.EffectOnTargetWhenDamageDeal:
                                entity.damageDealExtraEffect.Add(spellInstruction.effect.typeEffect, spellInstruction.effect);
                                break;
                            case TypeSpellInstruction.SelfEffectOnDamageReceive:
                                entity.damageReceiveExtraEffect.Add(spellInstruction.effect.typeEffect, spellInstruction.effect);
                                break;;
                            case TypeSpellInstruction.ChangeBasicAttack:
                                Debug.Log("Change weapon");
                                entity.weapons[0].idPoolProjectile = spellInstruction.idPoolObject;
                                entity.weapons[0].type = spellInstruction.weaponNewStats.typeWeapon;
                                entity.weapons[0].damage *= spellInstruction.weaponNewStats.damageModifier;
                                entity.weapons[0].attSpeed *= spellInstruction.weaponNewStats.attSpeedModifier;
                                break;
                        }
                    }
                }
            }
        }

        private void ActivePreviewSpell(Spell spell)
        {
            foreach (SpellInstruction spellInstruction in spell.spellInstructions)
            {
                if (spellInstruction.TypeSpellInstruction == TypeSpellInstruction.InstantiateSomething)
                {
                    if (spellInstruction.typeSpellObject == TypeSpellObject.GroundArea)
                    {
                        GameObject areaSpell = spell.spellInstantiate;
                        if (areaSpell == null)
                        {
                            areaSpell = ObjectPooler.SharedInstance.GetPooledObject(spellInstruction.idPoolObject);
                            spell.spellInstantiate = areaSpell;
                        }
                    }
                    break;
                }
            }
        }

        private IEnumerator Cooldown(Spell spell)
        {
            spell.canLaunch = false;
            yield return new WaitForSeconds(spell.cooldown);
            spell.canLaunch = true;

            Debug.Log("Peux relancer le spell");
        }
        
        private IEnumerator CastSpell(Spell spell)
        {
            // TODO : make anim
            entity.ressource1 -= spell.cost;

            if (spell.castTime > 0)
            {
                movementBlocked = true;
                intentBlocked = true;
            }
            
            yield return new WaitForSeconds(spell.castTime);

            movementBlocked = false;
            intentBlocked = false;
            
            foreach (SpellInstruction spellInstruction in spell.spellInstructions)
            {
                switch (spellInstruction.TypeSpellInstruction)
                {
                    case TypeSpellInstruction.InstantiateSomething:
                        ActiveSpellObject(spell, spellInstruction);
                        break;
                    case TypeSpellInstruction.SelfEffect:
                        EffectController.ApplyEffect(entity, spellInstruction.effect);
                        break;
                    case TypeSpellInstruction.EffectOnTargetWhenDamageDeal:
                        StartCoroutine(EffectController.EffectControllerInstance.AddDamageDealExtraEffect(entity, spellInstruction.effect, spellInstruction.durationInstruction));
                        break;
                    case TypeSpellInstruction.SelfEffectOnDamageReceive:
                        StartCoroutine(EffectController.EffectControllerInstance.AddDamageReceiveExtraEffect(entity, spellInstruction.effect, spellInstruction.durationInstruction));
                        break;
                    case TypeSpellInstruction.SpecialMovement:
                        PlaySpecialMovement(spellInstruction.specialMovement, spellInstruction.durationInstruction);
                        break;
                }
            }
        }

        private void ActiveSpellObject(Spell spell, SpellInstruction spellInstruction)
        {
            switch (spellInstruction.typeSpellObject)
            {
                case TypeSpellObject.Projectile:
                    GameObject projectileSpell =
                        ObjectPooler.SharedInstance.GetPooledObject(spellInstruction.idPoolObject);

                    Vector3 pos = transform.position;
                    pos.y += 0.5f;
                    projectileSpell.transform.position = pos;
                    
                    float rotX = projectileSpell.transform.localEulerAngles.x;
                    projectileSpell.transform.eulerAngles = camera.transform.eulerAngles + (Vector3.right * rotX);
                    projectileSpell.transform.forward *= 1.5f;

                    projectileSpell.SetActive(true);

                    ProjectilesPrefab projectilesPrefab = projectileSpell.GetComponent<ProjectilesPrefab>();
                    projectilesPrefab.rigidbody.AddForce(transform.forward * 1000, ForceMode.Acceleration);
                    projectilesPrefab.origin = entity;
                    break;
                case TypeSpellObject.GroundArea:
                    GameObject areaSpell = spell.spellInstantiate;

                    break;
                case TypeSpellObject.OnHimself:
                    GameObject objectPooled =
                        ObjectPooler.SharedInstance.GetPooledObject(spellInstruction.idPoolObject);

                    objectPooled.transform.localEulerAngles = transform.localEulerAngles;
                    objectPooled.transform.position = transform.position;
                    objectPooled.SetActive(true);
                    break;
            }
        }

        public void Movement()
        {
            Rigidbody rigidbody = playerRigidbody;

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

            if (grounded && !ejected)
            {
                rigidbody.velocity = transform.TransformDirection(locVel);
            }
        }

        private void CameraRotation()
        {
            if (entity.isFeared)
            {
                Entity launcher = entity.underEffects[TypeEffect.Fear].launcher;
                transform.LookAt(launcher.entityPrefab.transform);
                transform.Rotate(Vector3.up * 180);
                return;
            }
            else if(entity.isCharmed)
            {
                Entity launcher = entity.underEffects[TypeEffect.Charm].launcher;
                transform.LookAt(launcher.entityPrefab.transform);
                return;
            }
            
            int rotationSpeed = 5;
            
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            float vertical = Input.GetAxis("Mouse Y") * rotationSpeed;

            playerGameObject.transform.Rotate(0, horizontal, 0);

            vertical = Mathf.Clamp(vertical, -90f, 90f);            
            cameraPoint.transform.Rotate(-vertical, 0, 0, Space.Self);

            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            if (Physics.Raycast(ray, out hit, 1000, ~LayerMask.GetMask("Player", "Explosion")))
            {
                positionPointed = hit.point;
            }
            else
            {
                positionPointed = Vector3.zero;
            }

            if (Physics.Raycast(playerTransform.position, (camera.transform.forward * -1), out hit, 6, interactWithCamera))
            {
                camera.transform.position = hit.point;
                camera.transform.position += camera.transform.forward * 0.2f;
            }
            else
            {
                camera.transform.position = originalCameraPosition.transform.position;
            }
        }
    }
}