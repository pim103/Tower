using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoneOre", menuName = "Items/StoneOre", order = 3)]
public class StoneOre : Item
{

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>A hand full of stone ore</color>");
    }

}
