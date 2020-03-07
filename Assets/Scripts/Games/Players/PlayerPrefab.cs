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

        public Player player;

        [SerializeField] private PlayerExposer playerExposer;
        [SerializeField] private MovementPatternController movementPatternController;
        [SerializeField] private Slider hpBar;

        public int playerIndex;
        public bool canMove;

        private void Start()
        {
            player = new Player();
            player.SetPlayerExposer(playerExposer);
            player.InitPlayerStats(Classes.Warrior);
            player.effectInterface = this;

            wantToGoBack = false;
            wantToGoForward = false;
            wantToGoLeft = false;
            wantToGoRight = false;
        }

        private void Update()
        {
            GetIntentPlayer();

            float diff = (float) player.hp / (float) player.initialHp;
            hpBar.value = diff;
        }

        private void FixedUpdate()
        {
            Movement();
        }

        public void GetIntentPlayer()
        {
            if (player.underEffects.ContainsKey(TypeEffect.Stun) || player.underEffects.ContainsKey(TypeEffect.Sleep))
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
        
        public void AddItemInHand(Weapon weapon)
        {
            GameObject weaponGameObject = Instantiate(weapon.model, playerExposer.playerHand.transform);
            WeaponPrefab weaponPrefab = weaponGameObject.GetComponent<WeaponPrefab>();
            weapon.weaponPrefab = weaponPrefab;
            weaponPrefab.SetWielder(player);
            weaponPrefab.SetWeapon(weapon);
        }

        public void PlayBasicAttack(WeaponPrefab weaponPrefab)
        {
            weaponPrefab.BasicAttack(movementPatternController, playerExposer.playerHand);
        }

        public void StartCoroutineEffect(Effect effect)
        {
            StartCoroutine(PlayEffectOnTime(effect));
        }

        public IEnumerator PlayEffectOnTime(Effect effect)
        {
            player.underEffects.Add(effect.typeEffect, effect);

            Effect effectInList = player.underEffects[effect.typeEffect];
            while (effectInList.durationInSeconds > 0)
            {
                yield return new WaitForSeconds(0.5f);

                player.TriggerEffect(effectInList);

                effectInList = player.underEffects[effect.typeEffect];
                effectInList.durationInSeconds -= 0.5f;
                player.underEffects[effect.typeEffect] = effectInList;
            }

            player.underEffects.Remove(effect.typeEffect);
        }
    }
}