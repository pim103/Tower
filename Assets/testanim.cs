using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testanim : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("doingShortSwordAttack");
        }
    }
}
