using System;
using System.Collections.Generic;
using UnityEngine;

namespace Menus
{
    public class LightMenuController : MonoBehaviour
    {
        [SerializeField]
        private List<Light> lights;
        [SerializeField]
        private float minIntensity;
        [SerializeField]
        private float maxIntensity;
        [SerializeField]
        private float speedLight;

        private float intensity = 2;
        private bool addOrSub = true;

        private void Start()
        {
            Debug.Log("StartLight");
            foreach (Light light in lights)
            {
                light.intensity = minIntensity;
            }
        }

        private void Update()
        {
            if (intensity > maxIntensity)
            {
                addOrSub = false;
            }
            if (intensity < minIntensity)
            {
                addOrSub = true;
            }
            float valueLight = addOrSub ? speedLight : -speedLight;
            intensity += valueLight;
            
            foreach (Light light in lights)
            {
                light.intensity = intensity;
            }
        }
    }
}