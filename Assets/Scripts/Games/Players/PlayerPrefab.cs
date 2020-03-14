using System.Collections;
using System.Diagnostics;
using Games.Global;
using Games.Global.Patterns;
using Games.Global.Weapons;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Games.Players
{
    public class PlayerPrefab : PlayerIntent, EffectInterface
    {
        private const int PLAYER_SPEED = 10;

        [SerializeField] private Slider hpBar;
        [SerializeField] private Slider ressourcesBar;

        [SerializeField] public GameObject playerGameObject;
        [SerializeField] public Transform playerTransform;
        [SerializeField] private Rigidbody playerRigidbody;
        
        [SerializeField] public GameObject cameraPoint;
        [SerializeField] public Camera camera;
        [SerializeField] public GameObject cameraGameObject;
        [SerializeField] private GameObject originalCameraPosition;

        public int playerIndex;
        public bool canMove;

        private Vector3 positionPointed;

        private void Start()
        {
            Player player = new Player();

            entity = player;
            entity.entityPrefab = this;

            player.SetPlayerPrefab(this);
            player.InitPlayerStats(Classes.Ranger);
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
            if (!entity.underEffects.ContainsKey(TypeEffect.MadeADash))
            {
                Movement();
            }
    
            CameraRotation();
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

            if (!entity.doingSkill)
            {
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

            mousePosition = Input.mousePosition;
        }

        private void TryCastSpell(Spell spell)
        {
            if (spell.typeSpell == TypeSpell.Active ||
                spell.typeSpell == TypeSpell.Toggle ||
                spell.typeSpell == TypeSpell.ActiveWithPassive ||
                spell.typeSpell == TypeSpell.ToggleWithPassive
            )
            {
                if (spell.cost < entity.ressource1 && spell.canLaunch)
                {
                    Debug.Log("Lance le spell");
                    StartCoroutine(Cooldown(spell));
                    StartCoroutine(CastSpell(spell));
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

            yield return new WaitForSeconds(spell.castTime);

            foreach (SpellInstruction spellInstruction in spell.spellInstructions)
            {
                switch (spellInstruction.TypeSpellInstruction)
                {
                    case TypeSpellInstruction.InstantiateSomething:
                        ActiveSpellObject(spellInstruction);
                        
                        GameObject gameObjectSpell = ObjectPooler.SharedInstance.GetPooledObject(spellInstruction.idPoolObject);
                        gameObjectSpell.SetActive(true);
                        break;
                    case TypeSpellInstruction.SelfEffect:
                        entity.ApplyEffect(spellInstruction.effect);
                        break;
                    case TypeSpellInstruction.EffectOnTargetWhenDamageDeal:
                        StartCoroutine(AddDamageDealExtraEffect(spellInstruction.effect, spellInstruction.durationInstruction));
                        break;
                    case TypeSpellInstruction.SelfEffectOnDamageReceive:
                        StartCoroutine(AddDamageReceiveExtraEffect(spellInstruction.effect, spellInstruction.durationInstruction));
                        break;
                }
            }
        }

        private void ActiveSpellObject(SpellInstruction spellInstruction)
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
//                    projectileSpell.transform.LookAt(positionPointed);
                    projectileSpell.SetActive(true);

                    ProjectilesPrefab projectilesPrefab = projectileSpell.GetComponent<ProjectilesPrefab>();
                    projectilesPrefab.rigidbody.AddForce(transform.forward * 1000, ForceMode.Acceleration);
                    projectilesPrefab.origin = entity;
                    break;
            }
        }

        public void Movement()
        {
            Rigidbody rigidbody = playerRigidbody;

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

            var locVel = transform.InverseTransformDirection(rigidbody.velocity);
            locVel.x = horizontalMove * PLAYER_SPEED;
            locVel.z = verticalMove * PLAYER_SPEED;

            rigidbody.velocity = transform.TransformDirection(locVel);
        }

        private void CameraRotation()
        {
            int rotationSpeed = 5;
            
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            float vertical = Input.GetAxis("Mouse Y") * rotationSpeed;
            playerGameObject.transform.Rotate(0, horizontal, 0);
            
            cameraPoint.transform.Rotate(-vertical, 0, 0, Space.Self);
            virtualHand.transform.eulerAngles = camera.transform.eulerAngles;

            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            if (Physics.Raycast(ray, out hit, 1000, ~LayerMask.GetMask("Player", "Explosion")) && entity.weapons[0].type != TypeWeapon.Cac)
            {
                positionPointed = hit.point;
                virtualHand.transform.LookAt(positionPointed);
            }

            if (Physics.Raycast(playerTransform.position, (camera.transform.forward * -1), out hit, 6))
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