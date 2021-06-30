using UnityEngine;

namespace Games.Global
{
    public class MapStats : MonoBehaviour
    {
        public int mapsize;
        [SerializeField] public Transform startPos;
        [SerializeField] public GameObject endZone;
        [SerializeField] public GameObject endDoor;
    }
}
