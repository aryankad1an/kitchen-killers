using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    // for camera movement: use Cinemachine(really good plugin)

    public static Player Instance { get; private set; } // auto code lmao(C# feature) {its like an instance variable with getter and setter}
    public event EventHandler <OnSelectedCounterChangedEventArgs>OnSelectedCounterChanged; // event handler in C#

    // this is a way of assigning arguments to an event handler
    public class OnSelectedCounterChangedEventArgs : EventArgs // note that it extends EventArgs class
    {
        public ClearCounter selectedCounter;
    }
    
    // its not reccomended to make everything public because,
    // not just your editor but the other classes can access your variable which can be unsafe
    // using serialise field, this will be available to the unity editor
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask layerMask;
    
    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;

        bool didHit = Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, layerMask); // with lastInteractDir, even if we stop moving it still works
        // the raycastHit is an output parameter that helps in getting the value of the struct of things that is hit
        // with layerMask it will only raycast towards the specified gameObject Layer, thus all the unnecessary obstactles(like invisible wall) will be ignored
        if (didHit)
        {
            // out means output parameters
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // has ClearCounter
                if (clearCounter != selectedCounter)
                {
                    selectedCounter = clearCounter;
                    SetSelectedCounter(selectedCounter);
                    
                }
                // interact button
                // a simple event listener and handler
                if (Input.GetKeyDown(KeyCode.E))
                {
                    selectedCounter.Interact();
                }
                
            }
            else
            {
                selectedCounter = null;
                SetSelectedCounter(selectedCounter);

            }
        }
        else
        {
            selectedCounter = null;
            SetSelectedCounter(selectedCounter);

        }
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

    private void SetSelectedCounter(ClearCounter selectedCounterPara)
    {
        // event invoked
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            // adding event parameters
            selectedCounter = selectedCounterPara
        });
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
        }
        // awake is called when a particular script instance is loaded
        Instance = this;
    }
}

