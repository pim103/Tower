using Scripts.Games;
using System.Collections;
using System.Collections.Generic;
using Games;
using UnityEngine;

namespace Scripts.Games.Players
{
    public class PlayerMovementMaster : MonoBehaviour
    {
        private const int PLAYER_SPEED = 10;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        public void ApplyMovement()
        {
            for (int i = 0; i < objectsInScene.playerExposer.Length; i++)
            {
                Movement(i);
            }
        }

        public void Movement(int playerIndex)
        {
            Rigidbody rigidbody = objectsInScene.playerExposer[playerIndex].playerRigidbody;
            PlayerIntent playerIntent = objectsInScene.playerExposer[playerIndex].playerMovement;

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

            Camera playerCamera = objectsInScene.playerExposer[playerIndex].playerCamera.GetComponent<Camera>();
            RaycastHit hit;
            Ray cameraRay = playerCamera.ScreenPointToRay(playerIntent.mousePosition);

            if (Physics.Raycast(cameraRay, out hit))
            {
                Vector3 point = hit.point;
                point.y = 0;
                Transform playerTransform = objectsInScene.playerExposer[playerIndex].playerTransform;
                playerTransform.LookAt(point);
                playerTransform.localEulerAngles = Vector3.up * playerTransform.localEulerAngles.y;
            }
        }
    }
}
