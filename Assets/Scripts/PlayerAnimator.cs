using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // since animation is added to same game component as this script is added, we will use "Get Component"

    private Animator animator;
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player player; // getting a reference from Player Script lol(need to drag dawg)
    
    // awake is when script instance is being loaded..
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}