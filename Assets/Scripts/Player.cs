using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // its not reccomended to make everything public because,
    // not just your editor but the other classes can access your variable which can be unsafe
    // using serialise field, this will be available to the unity editor
    [SerializeField] private float moveSpeed = 7f;
    private void Update()
    {
        
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        } 
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * Time.deltaTime * moveSpeed; // transform refers to wherever the script is attached
        // this time thingy will keep this uniform in different FPS 

        // to add rotation with movement
        // transform.forward = moveDir; // sets the forward vector to move direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime*rotateSpeed);  // this is to smooth-en the rotation
    }
}
