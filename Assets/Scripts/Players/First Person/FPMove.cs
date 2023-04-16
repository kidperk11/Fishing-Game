using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    float inputX;
    float inputY;
    public FPPlayerActions moveActions;
    private InputAction movePlayer;
    private InputAction jump;

    Vector3 moveDirection;

    Rigidbody rb;

    private void OnEnable()
    {
        movePlayer = moveActions.Player.Move;
        movePlayer.Enable();
        jump = moveActions.Player.Jump;
        jump.Enable();

        jump.performed += Jump;
    }

    private void OnDisable()
    {
        movePlayer.Disable();
        jump.Disable();
    }

    private void Awake()
    {
        moveActions = new FPPlayerActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check
        //This check makes a line that is a little longer than half of the player's body.
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        CurrentInput();
        SpeedControl();

        //Handle Drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else { rb.drag = 0; }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void CurrentInput()
    {
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;

    }

    private void MovePlayer()
    {
        //Calculate Movement Direction
        moveDirection = orientation.forward * inputY + orientation.right * inputX;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limit velocity
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(readyToJump && grounded)
        {
            Debug.Log("Jump has been triggered");
            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("Grounded Status: " + grounded);
            Debug.Log("ReadyToJump Status: " + readyToJump);
        }
    }

    public void ResetJump()
    {
        readyToJump = true;
    }
}
