using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // since animation is added to same game component as this script is added, we will use "Get Component"

    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(IS_WALKING, true);
    }
}