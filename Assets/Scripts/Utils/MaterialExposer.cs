using System;
using UnityEngine;

namespace Utils
{
    public class MaterialExposer : MonoBehaviour
    {
        [SerializeField] private Material invisibleMaterial;
        [SerializeField] private Material defaultMaterial;

        private void Start()
        {
            StaticMaterials.invisibleMaterial = invisibleMaterial;
            StaticMaterials.defaultMaterial = defaultMaterial;
        }
    }

    public static class StaticMaterials
    {
        public static Material invisibleMaterial;
        public static Material defaultMaterial;
    }
}
