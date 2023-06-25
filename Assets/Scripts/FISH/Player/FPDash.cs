using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPDash : MonoBehaviour
{
    [Header("Dashing")]
    public float maxDashTime;
    public float dashTimer;
    public bool readyToDash;

    [Header("External References")]
    public Transform orientation;
    public FPMove moveScript;
    public Rigidbody rb;


    [Header("Input")]
    public float inputX;
    public float inputY;
    private Vector3 dashDirection;
    public FPPlayerActions moveActions;
    private InputAction movePlayer;
    private InputAction dash;


    private void OnEnable()
    {
        movePlayer = moveActions.Player.Move;
        movePlayer.Enable();
        dash = moveActions.Player.Dash;
        dash.Enable();
        dash.performed += StartDash;
    }

    private void OnDisable()
    {
        movePlayer.Disable();
        dash.Disable();
    }

    private void Awake()
    {
        moveActions = new FPPlayerActions();
    }

    private void Start()
    {
        readyToDash = true;
    }

    private void Update()
    {
        GetInputs();
        if(moveScript.grounded && moveScript.state != FPMove.MovementState.dashing)
        {
            ResetDash();
        }
    }

    private void FixedUpdate()
    {
        if (moveScript.dashing)
        {
            DashMovement();
        }
    }

    private void GetInputs()
    {
        //Get inputs
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;
    }

    private void StartDash(InputAction.CallbackContext context)
    {
        if (readyToDash)
        {
            rb.useGravity = false;
            //Calculate Movement Direction
            dashDirection = orientation.forward * inputY + orientation.right * inputX;

            if (dashDirection == Vector3.zero)
            {
                dashDirection = orientation.forward * 1;
            }

            moveScript.dashing = true;
            readyToDash = false;
        }
    }

    private void DashMovement()
    {
        dashTimer += Time.deltaTime;
        if(dashTimer >= maxDashTime)
        {
            StopDash();
        }
        else
        {
            rb.AddForce(dashDirection.normalized * moveScript.dashSpeed * 10f, ForceMode.Force);
        }
    }

    private void StopDash()
    {
        moveScript.dashing = false;
        dashTimer = 0;
        rb.useGravity = false;

    }

    public void ResetDash()
    {
        readyToDash = true;
    }
}
