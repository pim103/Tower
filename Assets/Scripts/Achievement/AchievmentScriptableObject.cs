using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the AchievmentScriptableObject script, it contains functionality
/// that is specific to the AchievmentScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "Achievments/Achievment")]
public class AchievmentScriptableObject : ScriptableObject {
  public string Category;
  public string Title;
  public string Description;
  public int Points;
  public int SpriteIndex;
  public int Progress;
  // public string[] Dependencies;
  public AchievmentScriptableObject[] AchievmentDependencies;
}