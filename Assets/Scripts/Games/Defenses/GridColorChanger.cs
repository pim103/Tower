using UnityEngine;

namespace Scripts.Games.Defenses
{
    public class GridColorChanger : MonoBehaviour
    {
        public void ChangeColorToGreen()
        {
            GetComponent<Renderer>().material.color = Color.green;
        }

        public void ChangeColorToCyan()
        {
            GetComponent<Renderer>().material.color = new Color(0,255,255,0);
        }
    }
}
