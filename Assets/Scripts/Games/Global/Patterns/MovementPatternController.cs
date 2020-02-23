using System.Collections;
using UnityEngine;

namespace Games.Global.Patterns
{
    public class MovementPatternController : MonoBehaviour
    {
        private IEnumerator TriggerMovement(Pattern[] pattern, GameObject objectToMove)
        {
            // TODO need to interpret more than 1 action at the same time for diagonal or other
            int i = 0;
            while (i < pattern.Length)
            {
                Pattern movement = pattern[i];
                float movementDuration = movement.movementDuration;
                Vector3 rot;

                while (movementDuration > 0)
                {
                    switch (movement.inst)
                    {
                        case PatternInstructions.ROTATE_UP:
                            objectToMove.transform.Rotate(-((movement.value * movement.cadence) / movement.movementDuration), 0, 0, Space.Self);
                            break;
                        case PatternInstructions.ROTATE_DOWN:
                            objectToMove.transform.Rotate((movement.value * movement.cadence) / movement.movementDuration, 0, 0, Space.Self);
                            break;
                        case PatternInstructions.ROTATE_LEFT:
                            objectToMove.transform.Rotate(0, 0, -((movement.value * movement.cadence) / movement.movementDuration), Space.Self);
                            break;
                        case PatternInstructions.ROTATE_RIGHT:
                            objectToMove.transform.Rotate(0, 0, (movement.value * movement.cadence) / movement.movementDuration, Space.Self);
                            break;
                        case PatternInstructions.FRONT:
                            objectToMove.transform.Translate(0, 0, (movement.value * movement.cadence) / movement.movementDuration, Space.Self);
                            break;
                        case PatternInstructions.BACK:
                            objectToMove.transform.Translate(0, 0, -((movement.value * movement.cadence) / movement.movementDuration), Space.Self);
                            break;
                        case PatternInstructions.LEFT:
                            objectToMove.transform.Translate(-((movement.value * movement.cadence) / movement.movementDuration), 0, 0, Space.Self);
                            break;
                        case PatternInstructions.RIGHT:
                            objectToMove.transform.Translate((movement.value * movement.cadence) / movement.movementDuration, 0, 0, Space.Self);
                            break;
                        case PatternInstructions.WAIT:
                            break;
                        default:
                            Debug.Log("WUT ?");
                            break;
                    }

                    yield return new WaitForSeconds(movement.cadence);
                    movementDuration -= movement.cadence;
                }

                i++;
            }
        }

        public void PlayMovement(Pattern[] pattern, GameObject objectToMove)
        {
            StartCoroutine(TriggerMovement(pattern, objectToMove));
        }
    }
}