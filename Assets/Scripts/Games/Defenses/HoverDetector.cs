using Games.Defenses;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
        public GameObject lastObjectPutInPlay;
        public GridTileController lastTileWithContent;
        
        private LayerMask mouseMask;
        public GameObject oldHover;

        public bool canPutItHere;
        private GridTileController currentTileController;

        [SerializeField] 
        private GameObject startPos;
        [SerializeField] 
        private GameObject dest;

        private NavMeshPath path;

        [SerializeField] 
        private GameObject WarningPanel;
        [SerializeField] 
        private Text WarningPanelText;
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
                //Debug.Log(hit.collider.name);
                
                    if (currentTileController.content || path.status != NavMeshPathStatus.PathComplete)
                    {
                        currentTileController.ChangeColorToRed();
                        canPutItHere = false;
                    }
                    else
                    {
                        currentTileController.ChangeColorToGreen();
                        WarningPanel.SetActive(false);
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
                        currentTileController = hit.collider.gameObject.GetComponent<GridTileController>();
                        path = new NavMeshPath(); 
                        NavMesh.CalculatePath(startPos.transform.position,dest.transform.position,NavMesh.AllAreas,path);
                        if (path.status == NavMeshPathStatus.PathComplete)
                        {
                            lastObjectPutInPlay = objectInHand;
                            lastTileWithContent = currentTileController;
                            currentTileController.content = objectInHand;
                            objectInHand = null;
                            defenseUiController.PutObjectInHand();
                        }
                    }
                    else if (!canPutItHere && !objectInHand)
                    {
                        objectInHand = currentTileController.content;
                        lastObjectPutInPlay = null;
                        currentTileController.content = null;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (!canPutItHere && currentTileController.content && currentTileController.content.layer == LayerMask.NameToLayer("Wall") &&
                        !objectInHand)
                    {
                        currentTileController.content.SetActive(false);
                        currentTileController.content = null;
                        defenseUiController.currentWallNumber += 1;
                        defenseUiController.wallButtonText.text = "Mur x" + defenseUiController.currentWallNumber;
                    } else if (objectInHand)
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
        
        private void LateUpdate()
        {
            path = new NavMeshPath(); 
            NavMesh.CalculatePath(startPos.transform.position,dest.transform.position,NavMesh.AllAreas,path);
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                if (!objectInHand)
                {
                    objectInHand = lastObjectPutInPlay;
                    lastTileWithContent.content = null;
                    lastObjectPutInPlay = null;
                }
                
                WarningPanel.SetActive(true);
                WarningPanelText.text = "Chemin bloqué : Bougez le bloc ou cliquez droit pour annuler";
            }
        }
    }
}
