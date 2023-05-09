using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float wallRunSpeed;

    public Transform orientation;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    float inputX;
    float inputY;
    public bool wallRunning;

    public MovementState state;
    public enum MovementState
    {
        walking, 
        air,
        wallRunning
    }


    public FPPlayerActions moveActions;
    private InputAction movePlayer;
    private InputAction jump;
    Vector3 moveDirection;
    Rigidbody rb;

    public float fallMultiplier;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

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

        //Locks cursor to middle of screen and makes it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check
        //This check makes a line that is a little longer than half of the player's body.
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        CurrentInput();
        SpeedControl();
        StateHandler();

        //Handle Drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else { rb.drag = 0; }
        //else if(rb.velocity.y > 0) { rb.drag = 2; }
    }

    private void StateHandler()
    {
        //Walk State
        if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        //Wall Run State
        if (wallRunning)
        {
            state = MovementState.wallRunning;
            moveSpeed = wallRunSpeed;
        }

        else
        {
            state = MovementState.air;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        //This code adjusts the fall speed of the player's jump
        if ((rb.velocity.y < 0 || rb.velocity.y > 0) && !OnSlope())
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

    }

    private void CurrentInput()
    {
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;


        //if (inputX == 0 && inputY == 0 && grounded)
        //{
        //    rb.velocity = new Vector3(0, rb.velocity.y, 0);
        //}
        

    }

    private void MovePlayer()
    {
        //Calculate Movement Direction
        moveDirection = orientation.forward * inputY + orientation.right * inputX;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        if(!wallRunning) rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Limit velocity
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(readyToJump && grounded)
        {
            exitingSlope = true;
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

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            //Measures how steep the slope is
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            Debug.Log("On a slope angle: " + angle);

            //Returns true if the angle is smaller than the max angle and not zero
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }


}
