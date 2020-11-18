using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This is the ResourceSlot script, it contains functionality that is specific to the resource slot on the UI
/// </summary>
public class ResourceSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image image;
    [SerializeField] protected Text amountText;

    public event Action<ResourceSlot> OnPointerEnterEvent;
    public event Action<ResourceSlot> OnPointerExitEvent;
    public event Action<ResourceSlot> OnRightClickEvent;

    protected bool IsPointerOver;

    protected Color normalColor = Color.white;
    protected Color disabledColor = new Color(1, 1, 1, 0);

    protected Resource _resource;
    public Resource Resource
    {
        get
        {
            return _resource;
        }
        set
        {
            _resource = value;

            if (_resource == null && Amount != 0)
            {
                Amount = 0;
            }

            if (_resource == null)
            {
                image.color = disabledColor;
            }
            else
            {
                image.sprite = _resource.Icon;
                image.color = normalColor;
            }

            if (IsPointerOver)
            {
                OnPointerExit(null);
                OnPointerEnter(null);
            }
        }
    }

    private int _amount;
    public int Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;

            if (_amount < 0)
            {
                _amount = 0;
            }

            if (_amount == 0 && Resource != null)
            {
                Resource = null;
            }

            if (amountText != null)
            {
                amountText.enabled = _resource != null && _amount > 1;
                if (amountText.enabled)
                {
                    amountText.text = _amount.ToString();
                }
            }
        }
    }

    public virtual bool CanAddStack(Resource resource, int amount = 1)
    {
        return Resource != null && Resource.ID == resource.ID;
    }

    public virtual bool CanReceiveItem(Resource resource)
    {
        return false;
    }

    protected virtual void OnValidate()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (amountText == null)
        {
            amountText = GetComponentInChildren<Text>();
        }

        Resource = _resource;
        Amount = _amount;
    }

    protected virtual void OnDisable()
    {
        if (IsPointerOver)
        {
            OnPointerExit(null);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (OnRightClickEvent != null)
            {
                OnRightClickEvent(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsPointerOver = true;

        if (OnPointerEnterEvent != null)
        {
            OnPointerEnterEvent(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsPointerOver = false;

        if (OnPointerExitEvent != null)
        {
            OnPointerExitEvent(this);
        }
    }
}