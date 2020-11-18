using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This is the Resource script, it contains functionality that is specific to the resource
/// </summary>
[CreateAssetMenu(menuName = "Resources/Resource")]
public class Resource : ScriptableObject
{
    [Space]
    [Header("Information on the resource")]
    [SerializeField] string id;
    public string ID
    {
        get
        {
            return id;
        }
    }
    public string ResourceName;
    public Sprite Icon;
    [Range(1, 999)]
    public int MaximumStacks = 1;

    [Space]
    [Header("Description of the resource")]
    [SerializeField] string ResourceDescription;

    #if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }
    #endif

    /// <summary>
    /// Get the description of the resource
    /// </summary>
    public string GetDescription()
    {
        return ResourceDescription;
    }
}