using UnityEngine;

namespace Scripts.Games.Defenses
{
    public class GridTileController : MonoBehaviour
    {
        public GameObject content;
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
            GetComponent<Renderer>().material.color = new Color(0,255,255,0);
        }
    }
}
