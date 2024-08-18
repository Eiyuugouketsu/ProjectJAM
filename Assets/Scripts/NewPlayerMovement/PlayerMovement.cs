using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float dragMovement;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpCooldown;
    [SerializeField]
    private float airMultiplier;
    private bool canJump;

    [Header("Keybinds")]
    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField]
    private float playerHeight;
    [SerializeField]
    private LayerMask ground;


    [SerializeField]
    private Transform orientation;
    private bool isGrounded;
    
    private Vector2 movementInputs;
    private Vector3 moveDirection;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canJump = true;
    }
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, ground); 
        
        CheckingInputs();
        SpeedControl();

        if(isGrounded)
            rb.drag = dragMovement;
        else
            rb.drag = 0;
    }
    private void FixedUpdate(){
        MovePlayer();
    }
    private void CheckingInputs(){
        movementInputs.x = Input.GetAxisRaw("Horizontal");
        movementInputs.y = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && canJump && isGrounded){
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void MovePlayer(){
        moveDirection = orientation.forward * movementInputs.y + orientation.right * movementInputs.x;

        float airMulti = isGrounded ? 1 : airMultiplier;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMulti, ForceMode.Force);
        if(rb.velocity.y < 0 && ! isGrounded)
            rb.AddForce(Vector3.down * airMultiplier * 3);
    }
    private void SpeedControl(){
        Vector3 flatSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatSpeed.magnitude > moveSpeed){
            Vector3 limitedSpeed = flatSpeed.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedSpeed.x, rb.velocity.y, limitedSpeed.z);
        }
    }
    private void Jump(){
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump(){
        canJump = true;
    }
}
