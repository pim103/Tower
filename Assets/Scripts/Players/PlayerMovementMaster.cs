using Scripts.Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Players
{
    public class PlayerMovementMaster : MonoBehaviour
    {
        private const int PLAYER_SPEED = 10;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        public void ApplyMovement()
        {
            for (int i = 0; i < objectsInScene.playersMovement.Length; i++)
            {
                Movement(i);
            }
        }

        public void Movement(int playerIndex)
        {
            Rigidbody rigidbody = objectsInScene.playersRigidbody[playerIndex];
            PlayerIntent playerIntent = objectsInScene.playersMovement[playerIndex];

            if(playerIndex == 1)
            {
                Debug.Log("" + playerIntent.wantToGoBack + playerIntent.wantToGoForward);
            }

            int horizontalMove = 0;
            int verticalMove = 0;

            if(playerIntent.wantToGoForward)
            {
                verticalMove += 1;
            }
            else if(playerIntent.wantToGoBack)
            {
                verticalMove -= 1;
            }

            if(playerIntent.wantToGoLeft)
            {
                horizontalMove -= 1;
            }
            else if(playerIntent.wantToGoRight)
            {
                horizontalMove += 1;
            }

            Vector3 movement = rigidbody.velocity;
            movement.x = horizontalMove * PLAYER_SPEED;
            movement.z = verticalMove * PLAYER_SPEED;

            rigidbody.velocity = movement;
        }
    }
}
