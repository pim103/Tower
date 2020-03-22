using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStats : MonoBehaviour
{
    [SerializeField]
    public int mapHeight;
    [SerializeField]
    public int mapWidth;
    [SerializeField] 
    public GameObject cameraPosition;
    [SerializeField] 
    public GameObject spawnPosition;
    [SerializeField] 
    public int wallNumber;
    [SerializeField] 
    public int wallType;
    [SerializeField] 
    public GameObject endCube;
    [SerializeField] 
    public GameObject startPos;
}
