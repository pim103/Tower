using System;
using UnityEngine;
using UnityEngine.UIElements;

//Author : Attika

public class Popup : MonoBehaviour
{
    private static Popup _this;
    public Button CancelButton;

    private void Awake()
    {
        if(_this == null)
            _this = this;
        //CancelButton.clicked += Close();
    }

    public static void Open()
    {
        _this.gameObject.SetActive(true);
    }
    
    public static void Close()
    {
        _this.gameObject.SetActive(false);
    }
    
}
