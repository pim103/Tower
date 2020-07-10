using System;
using UnityEngine;

namespace Utils
{
public class MaterialExposer : MonoBehaviour
{
    [SerializeField] private Material invisibleMaterial;
    [SerializeField] private Material burningMaterial;
    [SerializeField] private Material electricityMaterial;
    [SerializeField] private Material freezingMaterial;
    [SerializeField] private Material thornMaterial;
    [SerializeField] private Material poisonMaterial;
    [SerializeField] private Material defaultMaterial;

    private void Start()
    {
        StaticMaterials.invisibleMaterial = invisibleMaterial;
        StaticMaterials.defaultMaterial = defaultMaterial;
        StaticMaterials.burningMaterial = burningMaterial;
        StaticMaterials.electricityMaterial = electricityMaterial;
        StaticMaterials.freezingMaterial = freezingMaterial;
        StaticMaterials.poisonMaterial = poisonMaterial;
        StaticMaterials.thornMaterial = thornMaterial;
    }
}

public static class StaticMaterials
{
    public static Material invisibleMaterial;
    public static Material defaultMaterial;
    public static Material burningMaterial;
    public static Material electricityMaterial;
    public static Material freezingMaterial;
    public static Material thornMaterial;
    public static Material poisonMaterial;
}
}
