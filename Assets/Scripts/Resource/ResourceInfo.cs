using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Set the resource
    [SerializeField]
    private Resource resource;
    public Resource Resource
    {
        get
        {
            return resource;
        }
        set
        {
            resource = value;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.MyInstance.ShowTooltip(resource, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}