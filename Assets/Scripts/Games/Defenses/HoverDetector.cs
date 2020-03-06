using Games.Defenses;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Games.Defenses
{
    public class HoverDetector : MonoBehaviour
    {
        Ray ray;
        RaycastHit hit;
        RaycastHit hitObj;

        [SerializeField] 
        private Camera defenseCam;

        [SerializeField] 
        private DefenseUIController defenseUiController;
        
        public GameObject objectInHand;

        private LayerMask mouseMask;
        public GameObject oldHover;

        public bool canPutItHere;
        private GridTileController currentTileController;

        [SerializeField] 
        private GameObject startPos;
        [SerializeField] 
        private GameObject dest;

        private NavMeshPath path;
        private void Start()
        {
            mouseMask = LayerMask.GetMask("Grid");
        }

        void Update()
        {
            ray = defenseCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f, mouseMask))
            {
                currentTileController = hit.collider.gameObject.GetComponent<GridTileController>();
                path = new NavMeshPath(); 
                NavMesh.CalculatePath(startPos.transform.position,dest.transform.position,NavMesh.AllAreas,path);
                //Debug.Log(hit.collider.name);
                
                    if (currentTileController.content || path.status != NavMeshPathStatus.PathComplete)
                    {
                        currentTileController.ChangeColorToRed();
                        canPutItHere = false;
                    }
                    else
                    {
                        currentTileController.ChangeColorToGreen();
                        canPutItHere = true;
                    }

                    if (oldHover && oldHover!=hit.collider.gameObject)
                    {
                        oldHover.GetComponent<GridTileController>().ChangeColorToCyan();
                    }

                    oldHover = hit.collider.gameObject;
                

                if (objectInHand)
                {
                    objectInHand.transform.position = hit.collider.gameObject.transform.position + Vector3.down * 1.5f;
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (canPutItHere && objectInHand)
                    {
                        currentTileController.content = objectInHand;
                        objectInHand = null;
                        defenseUiController.PutObjectInHand();
                    }
                    else if (!canPutItHere && !objectInHand)
                    {
                        objectInHand = currentTileController.content;
                        currentTileController.content = null;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (!canPutItHere && currentTileController.content.layer == LayerMask.NameToLayer("Wall") &&
                        !objectInHand)
                    {
                        currentTileController.content.SetActive(false);
                        currentTileController.content = null;
                        defenseUiController.currentWallNumber += 1;
                        defenseUiController.wallButtonText.text = "Mur x" + defenseUiController.currentWallNumber;
                    }
                    if (objectInHand)
                    {
                        objectInHand.SetActive(false);
                        objectInHand = null;
                        defenseUiController.currentWallNumber += 1;
                        defenseUiController.wallButtonText.text = "Mur x" + defenseUiController.currentWallNumber;
                    }
                }
            }
            else
            {
                if (oldHover)
                {
                    oldHover.GetComponent<GridTileController>().ChangeColorToCyan();
                    oldHover = null;
                }
            }
        }
    }
}
