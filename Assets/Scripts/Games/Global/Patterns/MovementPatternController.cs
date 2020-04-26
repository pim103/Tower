//using System.Collections;
//using Games.Global.Weapons;
//using UnityEngine;
//
//namespace Games.Global.Patterns
//{
//    public class MovementPatternController : MonoBehaviour
//    {
//        private IEnumerator TriggerMovement(Weapon weapon, float attSpeed, GameObject objectToMove, BoxCollider bc)
//        {
//           //pattern[]//pattern = weapon.pattern;
//            // TODO need to interpret more than 1 action at the same time for diagonal or other
//            int i = 0;
//            while (i <//pattern.Length)
//            {
//               //pattern movement =//pattern[i];
//                float initialMovement = movement.movementDuration / (attSpeed ///pattern.Length);
//                float movementDuration = initialMovement;
//
//                while (movementDuration > 0)
//                {
//                    switch (movement.inst)
//                    {
//                        //patternInstructions.ROTATE_UP:
//                            objectToMove.transform.Rotate(-((movement.value * movement.cadence) / initialMovement), 0, 0, Space.Self);
//                            break;
//                        //patternInstructions.ROTATE_DOWN:
//                            objectToMove.transform.Rotate((movement.value * movement.cadence) / initialMovement, 0, 0, Space.Self);
//                            break;
//                        //patternInstructions.ROTATE_LEFT:
//                            objectToMove.transform.Rotate(0, 0, -((movement.value * movement.cadence) / initialMovement), Space.Self);
//                            break;
//                        //patternInstructions.ROTATE_RIGHT:
//                            objectToMove.transform.Rotate(0, 0, (movement.value * movement.cadence) / initialMovement, Space.Self);
//                            break;
//                        //patternInstructions.FRONT:
//                            objectToMove.transform.Translate(0, 0, (movement.value * movement.cadence) / initialMovement, Space.Self);
//                            break;
//                        //patternInstructions.BACK:
//                            objectToMove.transform.Translate(0, 0, -((movement.value * movement.cadence) / initialMovement), Space.Self);
//                            break;
//                        //patternInstructions.LEFT:
//                            objectToMove.transform.Translate(-((movement.value * movement.cadence) / initialMovement), 0, 0, Space.Self);
//                            break;
//                        //patternInstructions.RIGHT:
//                            objectToMove.transform.Translate((movement.value * movement.cadence) / initialMovement, 0, 0, Space.Self);
//                            break;
//                        //patternInstructions.WAIT:
//                            break;
//                        default:
//                            Debug.Log("WUT ?");
//                            break;
//                    }
//
//                    yield return new WaitForSeconds(movement.cadence);
//                    movementDuration -= movement.cadence;
//                }
//
//                i++;
//            }
//            
//            // At the end of movement, desactive boxCollider of weapon
//            bc.enabled = false;
//            weapon.oneHitDamageUp = 0;
//        }
//
//        public void PlayMovement(Weapon weapon, float attSpeed, GameObject objectToMove, BoxCollider bc)
//        {
//            StartCoroutine(TriggerMovement(weapon, attSpeed, objectToMove, bc));
//        }
//    }
//}