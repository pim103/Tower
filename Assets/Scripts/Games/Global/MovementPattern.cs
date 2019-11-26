﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace Scripts.Games.Global
{
    public class MovementPattern : MonoBehaviour
    {
        private IEnumerator TriggerMovement(Pattern[] pattern, GameObject objectToMove)
        {
            int i = 0;
            while (i < pattern.Length)
            {
                Pattern movement = pattern[i];
                float movementDuration = movement.movementDuration;

                while (movementDuration > 0)
                {
                    Debug.Log(movementDuration);
                    switch (movement.inst)
                    {
                        case PatternInstructions.ROTATE_UP:
                            objectToMove.transform.Rotate( -((movement.value * movement.cadence) / movement.movementDuration), 0, 0);
                            break;
                        case PatternInstructions.ROTATE_DOWN:
                            objectToMove.transform.Rotate((movement.value * movement.cadence) / movement.movementDuration, 0, 0);
                            break;
                        case PatternInstructions.ROTATE_LEFT:
                            break;
                        case PatternInstructions.ROTATE_RIGHT:
                            break;
                        case PatternInstructions.FRONT:
                            break;
                        case PatternInstructions.BACK:
                            break;
                        case PatternInstructions.LEFT:
                            break;
                        case PatternInstructions.RIGHT:
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
            Debug.Log("Ok");
            StartCoroutine(TriggerMovement(pattern, objectToMove));
        }
    }
}