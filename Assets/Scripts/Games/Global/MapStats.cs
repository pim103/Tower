using UnityEngine;

namespace Games.Global
{
    public class MapStats : MonoBehaviour
    {
        [SerializeField] public int mapSize;
        [SerializeField] public int floors;
        [SerializeField] public Transform startPos;
        [SerializeField] public GameObject endZone;
        [SerializeField] public GameObject endDoor;
        [SerializeField] public GameObject endFx;
        [SerializeField] public GameObject roof;
    }
}
