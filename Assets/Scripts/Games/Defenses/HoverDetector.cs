using System;
using DeckBuilding;
using Games.Global.Weapons;
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
        public DefenseUIController defenseUiController;
        
        public GameObject objectInHand;
        public GameObject lastObjectPutInPlay;
        public GridTileController lastTileWithContent;
        public GridTileController tileHoldingKeyGroup;
        public bool currentlyBlocked;
        
        [SerializeField]
        private LayerMask gridMask;
        [SerializeField]
        private LayerMask cardMask;
        public GameObject oldHover;

        public bool canPutItHere;
        private GridTileController currentTileController;
        
        public GameObject startPos;
        
        public GameObject dest;

        private NavMeshPath pathToEnd;
        private NavMeshPath pathToKey;

        [SerializeField] 
        private GameObject WarningPanel;
        [SerializeField] 
        private Text WarningPanelText;

        private bool aboveMap;
        
        private CardBehaviorInGame currentCardBehaviorInGame;

        private bool puttingWalls;
        public int maxResource;
        public int currentResource;
        private void Start()
        {
            //mouseMask = LayerMask.GetMask("Grid");
        }

        void Update()
        {
            ray = defenseCam.ScreenPointToRay(Input.mousePosition);
            aboveMap = false;
            if (Physics.Raycast(ray, out hit, 500f, gridMask))
            {
                aboveMap = true;
                currentTileController = hit.collider.gameObject.GetComponent<GridTileController>();
                //Debug.Log(objectInHand);
                //Debug.Log(currentCardBehavior.cardType);
                if (objectInHand && (objectInHand.layer == LayerMask.NameToLayer("CardInHand") && currentCardBehaviorInGame.cardType == 1 || objectInHand.layer == LayerMask.NameToLayer("Key")))
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
                else if (currentTileController.contentType != GridTileController.TypeData.Empty || 
                         pathToEnd.status != NavMeshPathStatus.PathComplete || 
                         currentTileController.isTooCloseFromAMob || 
                         (objectInHand && objectInHand.layer == LayerMask.NameToLayer("CardInHand") && currentCardBehaviorInGame.cardType == 0 && !currentCardBehaviorInGame.groupRangeBehavior.CheckContentEmpty()))
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
                        objectInHand.transform.position = hit.collider.gameObject.transform.position + Vector3.down * 2.25f;
                    }
                    else if(objectInHand.layer == LayerMask.NameToLayer("CardInHand"))
                    {
                        objectInHand.transform.position = hit.collider.gameObject.transform.position + Vector3.down * 1.5f;
                        currentCardBehaviorInGame.ownMeshRenderer.enabled = false;
                        if (currentCardBehaviorInGame.cardType == 0)
                        {
                            currentCardBehaviorInGame.rangeMeshRenderer.enabled = true;
                        }
                        currentCardBehaviorInGame.groupParent.SetActive(true);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse0) || puttingWalls)
                {
                    if (canPutItHere && objectInHand)
                    {
                        currentTileController = hit.collider.gameObject.GetComponent<GridTileController>();
                        pathToEnd = new NavMeshPath(); 
                        NavMesh.CalculatePath(startPos.transform.position,dest.transform.position,NavMesh.AllAreas,pathToEnd);
                        if (defenseUiController.keyAlreadyPut)
                        {
                            pathToKey = new NavMeshPath();
                            NavMesh.CalculatePath(startPos.transform.position, defenseUiController.keyObject.transform.position,NavMesh.AllAreas, pathToKey);
                        }
                        if (pathToEnd.status == NavMeshPathStatus.PathComplete && (pathToKey.status == NavMeshPathStatus.PathComplete || !defenseUiController.keyAlreadyPut))
                        {
                            currentlyBlocked = false;
                            if (objectInHand.layer == LayerMask.NameToLayer("Wall") ||
                                (objectInHand.layer == LayerMask.NameToLayer("CardInHand") && currentCardBehaviorInGame.cardType != 1 /*&& currentCardBehaviorInGame.group.cost <= currentResource*/) 
                                || objectInHand.layer == LayerMask.NameToLayer("Trap") && currentResource >= 1)
                            {
                                lastObjectPutInPlay = objectInHand;
                                lastTileWithContent = currentTileController;
                                currentTileController.content = objectInHand;
                                if (objectInHand.layer == LayerMask.NameToLayer("Wall"))
                                {
                                    currentTileController.contentType = GridTileController.TypeData.Wall;
                                    puttingWalls = true;
                                }
                                else if (objectInHand.layer == LayerMask.NameToLayer("CardInHand"))
                                {
                                    currentTileController.contentType = GridTileController.TypeData.Group;
                                    currentCardBehaviorInGame.groupRangeBehavior.SetAllTilesTo(true);
                                    //currentResource -= currentCardBehaviorInGame.group.cost;
                                    //currentCardBehavior.groupRange.SetActive(true);
                                } 
                                else if (objectInHand.layer == LayerMask.NameToLayer("Trap"))
                                {
                                    currentTileController.contentType = GridTileController.TypeData.Trap;
                                    //currentResource--;
                                }

                                objectInHand = null;
                                if (lastObjectPutInPlay.layer == LayerMask.NameToLayer("Wall"))
                                {
                                    defenseUiController.PutWallInHand();
                                }

                                defenseUiController.currentResourceText.text = currentResource.ToString();
                            }
                            else if(objectInHand.layer == LayerMask.NameToLayer("CardInHand") && currentCardBehaviorInGame.cardType == 1 /*&& currentCardBehaviorInGame.equipement.cost <= currentResource*/)
                            {
                                lastObjectPutInPlay = objectInHand;
                                lastTileWithContent = currentTileController;
                                CardBehaviorInGame contentCardBehaviorInGame =
                                    currentTileController.content.GetComponent<CardBehaviorInGame>();
                                switch (currentCardBehaviorInGame.equipement.type)
                                {
                                    case TypeWeapon.Distance:
                                        if (contentCardBehaviorInGame.rangedWeaponSlot)
                                        {
                                            defenseUiController.PutCardBackToHand(contentCardBehaviorInGame
                                                .rangedWeaponSlot);
                                        }

                                        contentCardBehaviorInGame.rangedWeaponSlot = objectInHand;
                                        break;
                                    case TypeWeapon.Cac:
                                        if (contentCardBehaviorInGame.meleeWeaponSlot)
                                        {
                                            defenseUiController.PutCardBackToHand(contentCardBehaviorInGame
                                                .meleeWeaponSlot);
                                        }

                                        contentCardBehaviorInGame.meleeWeaponSlot = objectInHand;
                                        break;
                                }

                                objectInHand.SetActive(false);
                                objectInHand = null;
                            }
                            else if(objectInHand.layer == LayerMask.NameToLayer("Key"))
                            {
                                CardBehaviorInGame contentCardBehaviorInGame =
                                    currentTileController.content.GetComponent<CardBehaviorInGame>();
                                tileHoldingKeyGroup = currentTileController;
                                contentCardBehaviorInGame.keySlot = objectInHand;
                                defenseUiController.keyAlreadyPut = true;
                                objectInHand.transform.position = hit.collider.gameObject.transform.position + Vector3.down * 2.25f;
                                //objectInHand.SetActive(false);
                                objectInHand = null;
                            }

                            /*else
                            {
                                lastObjectPutInPlay.layer = LayerMask.NameToLayer("Card");
                            }*/
                        }
                    }
                    else if (!canPutItHere && !objectInHand && currentTileController.contentType != GridTileController.TypeData.Empty)
                    {
                        objectInHand = currentTileController.content;
                        lastObjectPutInPlay = null;
                        currentTileController.content = null;
                        currentTileController.contentType = GridTileController.TypeData.Empty;
                        if (objectInHand.layer == LayerMask.NameToLayer("CardInHand"))
                        {
                            currentCardBehaviorInGame = objectInHand.GetComponent<CardBehaviorInGame>();
                            currentCardBehaviorInGame.groupRangeBehavior.SetAllTilesTo(false); 
                            //currentCardBehavior.groupRange.SetActive(false);
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
                    } /*else if (objectInHand)
                    {
                        if (objectInHand.layer == LayerMask.NameToLayer("Wall"))
                        {
                            objectInHand.SetActive(false);
                            objectInHand = null;
                            defenseUiController.currentWallNumber += 1;
                            defenseUiController.wallButtonText.text = "Mur x" + defenseUiController.currentWallNumber;
                        } else if (objectInHand.layer == LayerMask.NameToLayer("Trap"))
                        {
                            objectInHand.SetActive(false);
                            objectInHand = null;
                        }
                    }*/
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
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    CardBehaviorInGame hitCardBehavior = hit.transform.GetComponent<CardBehaviorInGame>();
                    if (hitCardBehavior.cardType == 0 && hitCardBehavior.group.cost <= currentResource ||
                        hitCardBehavior.cardType == 1 && hitCardBehavior.equipement.cost <= currentResource)
                    {
                        if (objectInHand)
                        {
                            if (objectInHand.layer == LayerMask.NameToLayer("CardInHand"))
                            {
                                defenseUiController.PutCardBackToHand(objectInHand);
                            }
                            else if (objectInHand.layer == LayerMask.NameToLayer("Wall"))
                            {
                                objectInHand.SetActive(false);
                                objectInHand = null;
                                defenseUiController.currentWallNumber += 1;
                                defenseUiController.wallButtonText.text =
                                    "Mur x" + defenseUiController.currentWallNumber;
                            }
                            else if (objectInHand.layer == LayerMask.NameToLayer("Trap"))
                            {
                                objectInHand.SetActive(false);
                                objectInHand = null;
                            }
                        }

                        defenseUiController.PutCardInHand(hit.collider.gameObject);
                        currentCardBehaviorInGame = objectInHand.GetComponent<CardBehaviorInGame>();
                        if (currentCardBehaviorInGame.cardType == 0)
                        {
                            currentResource -= currentCardBehaviorInGame.group.cost;
                        }
                        else
                        {
                            currentResource -= currentCardBehaviorInGame.equipement.cost;
                        }
                        defenseUiController.currentResourceText.text = currentResource.ToString();
                    }
                }
            }
            
            if(Input.GetKeyDown(KeyCode.Mouse1) && objectInHand){

                if (objectInHand.layer == LayerMask.NameToLayer("CardInHand"))
                {
                    defenseUiController.PutCardBackToHand(currentCardBehaviorInGame.meleeWeaponSlot);
                    defenseUiController.PutCardBackToHand(currentCardBehaviorInGame.rangedWeaponSlot);

                    currentCardBehaviorInGame.meleeWeaponSlot = null;
                    currentCardBehaviorInGame.rangedWeaponSlot = null;
                    currentCardBehaviorInGame.keySlot = null;
                    defenseUiController.PutKeyBackToSlot();

                    currentCardBehaviorInGame.groupParent.SetActive(false);
                    currentCardBehaviorInGame.groupParent.transform.localPosition = Vector3.zero;
                    currentCardBehaviorInGame.ownMeshRenderer.enabled = true;
                    if (currentCardBehaviorInGame.cardType == 0)
                    {
                        currentCardBehaviorInGame.rangeMeshRenderer.enabled = false;
                    }

                    objectInHand.transform.SetParent(currentCardBehaviorInGame.ownCardContainer);
                    objectInHand.transform.localPosition = Vector3.zero;
                    objectInHand.layer = LayerMask.NameToLayer("Card");
                    objectInHand = null;
                    if (currentCardBehaviorInGame.cardType == 0)
                    {
                        currentResource += currentCardBehaviorInGame.group.cost;
                    }
                    else
                    {
                        currentResource += currentCardBehaviorInGame.equipement.cost;
                    }
                    defenseUiController.currentResourceText.text = currentResource.ToString();
                } else if (objectInHand.layer == LayerMask.NameToLayer("Wall"))
                {
                    objectInHand.SetActive(false);
                    objectInHand = null;
                    defenseUiController.currentWallNumber += 1;
                    defenseUiController.wallButtonText.text = "Mur x" + defenseUiController.currentWallNumber;
                } else if (objectInHand.layer == LayerMask.NameToLayer("Trap"))
                {
                    objectInHand.SetActive(false);
                    objectInHand = null;
                    currentResource += 1;
                    defenseUiController.currentResourceText.text = currentResource.ToString();
                }
                else if(objectInHand.layer == LayerMask.NameToLayer("Key"))
                {
                    objectInHand = null;
                    defenseUiController.PutKeyBackToSlot();
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f && objectInHand && objectInHand.layer == LayerMask.NameToLayer("Trap"))
            {
                objectInHand.GetComponent<TrapBehavior>().rotation += 1; 
                objectInHand.transform.Rotate(Vector3.up,90);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && objectInHand && objectInHand.layer == LayerMask.NameToLayer("Trap"))
            {
                objectInHand.GetComponent<TrapBehavior>().rotation -= 1; 
                objectInHand.transform.Rotate(Vector3.up,-90);
            }

            if (objectInHand && objectInHand.layer == LayerMask.NameToLayer("CardInHand") && !aboveMap)
            {
                currentCardBehaviorInGame.groupParent.SetActive(false);
                currentCardBehaviorInGame.groupParent.transform.localPosition = Vector3.zero;
                var worldPos = Input.mousePosition;
                worldPos.z = 10.0f;
                worldPos = defenseCam.ScreenToWorldPoint(worldPos);
                objectInHand.transform.position = worldPos;
                currentCardBehaviorInGame.ownMeshRenderer.enabled = true;
                if (currentCardBehaviorInGame.cardType == 0)
                {
                    currentCardBehaviorInGame.rangeMeshRenderer.enabled = false;
                }
            }

            if (objectInHand && objectInHand.layer == LayerMask.NameToLayer("Key"))
            {
                var worldPos = Input.mousePosition;
                worldPos.z = 10.0f;
                worldPos = defenseCam.ScreenToWorldPoint(worldPos);
                objectInHand.transform.position = worldPos;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && puttingWalls)
            {
                puttingWalls = false;
            }
        }
        
        private void LateUpdate()
        {
            pathToEnd = new NavMeshPath(); 
            NavMesh.CalculatePath(startPos.transform.position,dest.transform.position,NavMesh.AllAreas,pathToEnd);
            pathToKey = new NavMeshPath();
            if (defenseUiController.keyAlreadyPut)
            {
                NavMesh.CalculatePath(startPos.transform.position, defenseUiController.keyObject.transform.position,NavMesh.AllAreas, pathToKey);
                Debug.Log(pathToKey.status);
            }
            if (pathToEnd.status != NavMeshPathStatus.PathComplete || pathToKey.status != NavMeshPathStatus.PathComplete && defenseUiController.keyAlreadyPut)
            {
                currentlyBlocked = true;
                if (!objectInHand)
                {
                    objectInHand = lastObjectPutInPlay;
                    if (lastTileWithContent)
                    {
                        lastTileWithContent.content = null;
                        lastTileWithContent.contentType = GridTileController.TypeData.Empty;
                    }
                    lastObjectPutInPlay = null;
                }

                if (lastTileWithContent)
                {
                    WarningPanel.SetActive(true);
                    WarningPanelText.text = "Chemin bloqué : Bougez le bloc ou cliquez droit pour annuler";
                }
            }
        }
    }
}
