using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class TestPopup_TO_DO_DELETE : MonoBehaviour
{
    public TextMeshProUGUI helloWorld;

    public void AddListenerToPopup(Popup popup)
    {
        popup.AddListenerToCloseButton(HelloWorld);
    }

    public void HelloWorld()
    {
        helloWorld.gameObject.SetActive(true);
        StartCoroutine(WaitAndEraseHello());
    }

    private IEnumerator WaitAndEraseHello()
    {
        yield return new WaitForSeconds(2.0f);
        helloWorld.gameObject.SetActive(false);
    }
}
