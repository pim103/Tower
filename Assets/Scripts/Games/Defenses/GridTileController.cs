using UnityEngine;

namespace Games.Defenses
{
    public class GridTileController : MonoBehaviour
    {
        public enum TypeData
        {
            Empty,
            Trap,
            Group,
            Wall
        }

        public Vector2 coordinates;
        public GameObject content;
        public TypeData contentType = TypeData.Empty;
        public bool isTooCloseFromAMob;
        
        public void ChangeColorToGreen()
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        public void ChangeColorToRed()
        {
            GetComponent<Renderer>().material.color = Color.red;
        }

        public void ChangeColorToCyan()
        {
            GetComponent<Renderer>().material.color = new Color(0,255,255,100);
        }
    }
}
