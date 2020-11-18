using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is the ResourceInfo script, it contains functionality that is specific to the resource
/// </summary>
public class ResourceInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Set the resource
    [SerializeField] private Resource resource;
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
        RectTransform container = (RectTransform)transform;
        UIManager.MyInstance.ShowTooltip(resource, transform.position, container);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}