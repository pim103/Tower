using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagers : MonoBehaviour
{
    public static GameManagers instance;

    private void Awake()
    {
        if (instance != null)
        {
            //Destroy(gameObject);
            Destroy(GameManagers.instance.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}