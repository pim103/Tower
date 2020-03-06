using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlanningTest : MonoBehaviour
{
    [SerializeField] private GameObject dest;

    private NavMeshPath path;

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            path = new NavMeshPath();
            Debug.Log(NavMesh.CalculatePath(transform.position,dest.transform.position,NavMesh.AllAreas,path));
            Debug.Log(path.status);
            //agent.SetPath(path);
        }*/
    }
}
