using UnityEngine;

namespace Scripts.Games.Defenses
{
    public class HoverDetector : MonoBehaviour
    {
        Ray ray;
        RaycastHit hit;

        [SerializeField]
        private Camera defenseCam;

        [SerializeField] 
        private GameObject card;
    
        private bool hasSelectedCard;
        private LayerMask mouseMask;
        public GameObject oldHover;
        private void Start()
        {
            //hasSelectedCard = false;
            mouseMask = LayerMask.GetMask("Grid");
        }

        void Update()
        {
            ray = defenseCam.ScreenPointToRay(Input.mousePosition);
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                Debug.Log("yes");
                if (Physics.Raycast(ray, out hit, 100f,mouseMask))
                {
                    Debug.Log("hit");
                    Debug.Log(hit.collider.name);
                    if (hit.collider.gameObject != oldHover)
                    {
                        hit.collider.gameObject.GetComponent<GridColorChanger>().ChangeColorToGreen();
                        if (oldHover)
                        {
                            oldHover.GetComponent<GridColorChanger>().ChangeColorToCyan();
                        }
                        oldHover = hit.collider.gameObject;
                    }
                }
                else
                {
                    if (oldHover)
                    {
                        oldHover.GetComponent<GridColorChanger>().ChangeColorToCyan();
                        oldHover = null;
                    }
                }
            }

            /*if (hasSelectedCard)
        {
            //keep track of the mouse position
            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z); 
            card.transform.position = defenseCam.ScreenToWorldPoint(Input.mousePosition)+new Vector3(0f,0f,10f);
        }*/
        }
        /*void OnMouseOver()
    {
        Debug.Log("On End");
    }

    void OnMouseExit()
    {
        Debug.Log("Exit End");
    }*/
    }
}
