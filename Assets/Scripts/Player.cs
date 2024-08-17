using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] float movementSpeed;
    [SerializeField] float cameraYabsClamp;
    [SerializeField] float mouseSensetivity;
    float yRotation = 0;
    Vector3 movementDirection;
    const float gravity = 0.1f;
    float dynamicGravity;
    bool isGround;

    private void Update() 
    {
        movementDirection = new Vector3(Input.GetAxis("Horizontal"),-dynamicGravity,Input.GetAxis("Vertical"));
        // float xAxis = Input.GetAxis("Mouse X")*mouseSensetivity;
        // float yAxis = Input.GetAxis("Mouse Y")*mouseSensetivity;
        // yRotation -= yAxis;
        // yRotation = Mathf.Clamp(yRotation,-cameraYabsClamp,cameraYabsClamp);
        // Camera.main.transform.localEulerAngles = Vector3.right*cameraYabsClamp;
        // Debug.Log(yAxis);
    }

    private void FixedUpdate() 
    {   
        if(characterController.isGrounded)
        {
            dynamicGravity = 0;
        }
        else
        {
            dynamicGravity += gravity;
        }
        characterController.Move(movementDirection * movementSpeed * Time.deltaTime);
    }
}
