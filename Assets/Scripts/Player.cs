using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    // for camera movement: use Cinemachine(really good plugin)
    
    // its not reccomended to make everything public because,
    // not just your editor but the other classes can access your variable which can be unsafe
    // using serialise field, this will be available to the unity editor
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // adding collision detection(make sure to add box collider{with appropriate size} to the collision participants)
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        //                                  lower point,                     upper point,                            radius of capsule,direction of cast,action distance                     
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up*playerHeight, playerRadius, moveDir, moveDistance); // returns a boolean(hits or not)
        // raycast is something like a laser and it tells if it hits something or not
        // capsuleCase is raycast but fires a capsule size areal laser

        
        isWalking = ( moveDir != new Vector3(0, 0, 0) ); // self explanatory

        if (canMove)
        {
            transform.position += moveDir * moveDistance; // transform refers to wherever the script is attached
            // this time thingy will keep this uniform in different FPS 

        }
        else
        {
            // cannot move towards moveDir
            /*
             THIS CODE WILL HELP IN ACHIEVING DIAGONAL MOVEMENT
             
             imagine ur facing a wall(up direction) and ur trying to move in up and right direction, in this situation we will make player slowly move in right direction instead of completely disabling him from moving
             */
            
            // Try X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            bool canMoveX = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up*playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMoveX)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Try Z Movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                bool canMoveZ = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up*playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMoveZ)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // cannot move in any direction(somehow)
                    moveDir = Vector3.zero;
                }
            }

            transform.position += moveDir * moveDistance;
        }

        // to add rotation with movement
        // transform.forward = moveDir; // sets the forward vector to move direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime*rotateSpeed);  // this is to smooth-en the rotation
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
