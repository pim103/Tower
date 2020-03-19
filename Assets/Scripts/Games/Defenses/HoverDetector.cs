using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Games.Defenses
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
        
        [SerializeField]
        private LayerMask gridMask;
        [SerializeField]
        private LayerMask cardMask;
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

        private bool aboveMap;
        
        private CardBehavior currentCardBehavior;
        
        private void Start()
        {
            //mouseMask = LayerMask.GetMask("Grid");
        }

        void Update()
        {
            ray = defenseCam.ScreenPointToRay(Input.mousePosition);
            aboveMap = false;
            if (Physics.Raycast(ray, out hit, 100f, gridMask))
            {
                aboveMap = true;
                currentTileController = hit.collider.gameObject.GetComponent<GridTileController>();
                //Debug.Log(hit.collider.name);
                if (objectInHand && objectInHand.layer == LayerMask.NameToLayer("CardInHand") && currentCardBehavior.cardType == 1)
                {
                    if (currentTileController.contentType == GridTileController.TypeData.Group)
                    {
                        currentTileController.ChangeColorToGreen();
                        canPutItHere = true;
                    }
                    else
                    {
                        currentTileController.ChangeColorToRed();
                        canPutItHere = false;
                    }
                }
                else if (currentTileController.contentType != GridTileController.TypeData.Empty || path.status != NavMeshPathStatus.PathComplete)
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
                    if (objectInHand.layer == LayerMask.NameToLayer("Wall") || objectInHand.layer == LayerMask.NameToLayer("Trap"))
                    {
                        objectInHand.transform.position = hit.collider.gameObject.transform.position + Vector3.down * 1.5f;
                    }
                    else if(objectInHand.layer == LayerMask.NameToLayer("CardInHand"))
                    {
                        objectInHand.transform.position = hit.collider.gameObject.transform.position + Vector3.down * 1.5f;
                        currentCardBehavior.ownMeshRenderer.enabled = false;
                        currentCardBehavior.groupParent.SetActive(true);
                    }
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
                            if (objectInHand.layer == LayerMask.NameToLayer("Wall") ||
                                (objectInHand.layer == LayerMask.NameToLayer("CardInHand") &&
                                 currentCardBehavior.cardType != 1) || objectInHand.layer == LayerMask.NameToLayer("Trap"))
                            {
                                Debug.Log("yes");
                                lastObjectPutInPlay = objectInHand;
                                lastTileWithContent = currentTileController;
                                currentTileController.content = objectInHand;
                                if (objectInHand.layer == LayerMask.NameToLayer("Wall"))
                                {
                                    currentTileController.contentType = GridTileController.TypeData.Wall;
                                }
                                else if (objectInHand.layer == LayerMask.NameToLayer("CardInHand"))
                                {
                                    currentTileController.contentType = GridTileController.TypeData.Group;
                                } 
                                else if (objectInHand.layer == LayerMask.NameToLayer("Trap"))
                                {
                                    currentTileController.contentType = GridTileController.TypeData.Trap;
                                }

                                objectInHand = null;
                                if (lastObjectPutInPlay.layer == LayerMask.NameToLayer("Wall"))
                                {
                                    defenseUiController.PutWallInHand();
                                }
                            }
                            else
                            {
                                lastObjectPutInPlay = objectInHand;
                                lastTileWithContent = currentTileController;
                                currentTileController.content.GetComponent<CardBehavior>().equipementsList.Add(objectInHand);
                                objectInHand.SetActive(false);
                                objectInHand = null;
                            }

                            /*else
                            {
                                lastObjectPutInPlay.layer = LayerMask.NameToLayer("Card");
                            }*/
                        }
                    }
                    else if (!canPutItHere && !objectInHand)
                    {
                        objectInHand = currentTileController.content;
                        lastObjectPutInPlay = null;
                        currentTileController.content = null;
                        currentTileController.contentType = GridTileController.TypeData.Empty;
                        if (objectInHand.layer == LayerMask.NameToLayer("CardInHand"))
                        {
                            currentCardBehavior = objectInHand.GetComponent<CardBehavior>();
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (!canPutItHere && currentTileController.contentType != GridTileController.TypeData.Empty && currentTileController.content.layer == LayerMask.NameToLayer("Wall") &&
                        !objectInHand)
                    {
                        currentTileController.content.SetActive(false);
                        currentTileController.content = null;
                        currentTileController.contentType = GridTileController.TypeData.Empty;
                        defenseUiController.currentWallNumber += 1;
                        defenseUiController.wallButtonText.text = "Mur x" + defenseUiController.currentWallNumber;
                    } else if (objectInHand)
                    {
                        if (objectInHand.layer == LayerMask.NameToLayer("Wall"))
                        {
                            objectInHand.SetActive(false);
                            objectInHand = null;
                            defenseUiController.currentWallNumber += 1;
                            defenseUiController.wallButtonText.text = "Mur x" + defenseUiController.currentWallNumber;
                        }
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
            
            if (Physics.Raycast(ray, out hit, 100f, cardMask))
            {
                if (!objectInHand && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    defenseUiController.PutCardInHand(hit.collider.gameObject);
                    currentCardBehavior = objectInHand.GetComponent<CardBehavior>();
                }
            }
            
            if(Input.GetKeyDown(KeyCode.Mouse1) && objectInHand && objectInHand.layer == LayerMask.NameToLayer("CardInHand"))
            {
                foreach (var equipementCard in currentCardBehavior.equipementsList)
                {
                    equipementCard.SetActive(true);
                    CardBehavior equipementCardBehavior = equipementCard.GetComponent<CardBehavior>();
                    equipementCardBehavior.groupParent.SetActive(false);
                    equipementCardBehavior.groupParent.transform.localPosition = Vector3.zero;
                    equipementCardBehavior.ownMeshRenderer.enabled = true;
                    equipementCardBehavior.transform.SetParent(equipementCardBehavior.container);
                    equipementCardBehavior.transform.localPosition = Vector3.zero;
                    equipementCardBehavior.gameObject.layer = LayerMask.NameToLayer("Card");
                }
                currentCardBehavior.groupParent.SetActive(false);
                currentCardBehavior.groupParent.transform.localPosition = Vector3.zero;
                currentCardBehavior.ownMeshRenderer.enabled = true;
                objectInHand.transform.SetParent(currentCardBehavior.container);
                objectInHand.transform.localPosition = Vector3.zero;
                objectInHand.layer = LayerMask.NameToLayer("Card");
                objectInHand = null;
            }
            
            if(objectInHand && objectInHand.layer == LayerMask.NameToLayer("CardInHand") && !aboveMap)
            {
                currentCardBehavior.groupParent.SetActive(false);
                currentCardBehavior.groupParent.transform.localPosition = Vector3.zero;
                var worldPos = Input.mousePosition;
                worldPos.z = 10.0f;
                worldPos = defenseCam.ScreenToWorldPoint(worldPos);
                objectInHand.transform.position = worldPos;
                currentCardBehavior.ownMeshRenderer.enabled = true;
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
                    lastTileWithContent.contentType = GridTileController.TypeData.Empty;
                    lastObjectPutInPlay = null;
                }
                
                WarningPanel.SetActive(true);
                WarningPanelText.text = "Chemin bloqué : Bougez le bloc ou cliquez droit pour annuler";
            }
        }
    }
}
