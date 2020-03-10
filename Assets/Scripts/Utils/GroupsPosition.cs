using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Utils
{
    public static class GroupsPosition
    {
        public static List<Vector3> position = new List<Vector3>();

        public static void InitPosition()
        {
            position.Add(new Vector3(0, 0, 0));
            position.Add(new Vector3(2, 0, 0));
            position.Add(new Vector3(2, 0, 2));
            position.Add(new Vector3(0, 0, 2));
            position.Add(new Vector3(-2, 0, 2));
            position.Add(new Vector3(-2, 0, 0));
            position.Add(new Vector3(-2, 0, -2));
            position.Add(new Vector3(0, 0, -2));
            position.Add(new Vector3(2, 0, -2));
        }
    }
}
