using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Global
{
    /*
     * Enum listing instructions :
     * Wait with time in sec
     * Action with angle
     * 
     */
    public enum PatternInstructions
    {
        ROTATE_UP,
        ROTATE_DOWN,
        ROTATE_LEFT,
        ROTATE_RIGHT,
        FRONT,
        BACK,
        LEFT,
        RIGHT,
        WAIT
    }

    public class Pattern : MonoBehaviour
    {
        public PatternInstructions inst;
        public float value;
        public float movementDuration;
        public float cadence;

        public Pattern(PatternInstructions inst, float value, float movementDuration = 1f, float cadence = 0.1f)
        {
            this.inst = inst;
            this.value = value;
            this.movementDuration = movementDuration;
            this.cadence = cadence;
        }
    }
}