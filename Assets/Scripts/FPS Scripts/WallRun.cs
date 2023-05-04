using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallRun : MonoBehaviour
{
    [Header("WallRunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public float inputX;
    public float inputY;
    public FPPlayerActions moveActions;
    private InputAction movePlayer;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform orientation;
    public FPMove moveScript;
    private Rigidbody rb;

    private void OnEnable()
    {
        movePlayer = moveActions.Player.Move;
        movePlayer.Enable();
    }

    private void OnDisable()
    {
        movePlayer.Disable();
    }

    private void Awake()
    {
        moveActions = new FPPlayerActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveScript = GetComponent<FPMove>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (moveScript.wallRunning)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, out rightWallHit, minJumpHeight, whatIsWall);
    }

    private void StateMachine()
    {
        //Get inputs
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;

        //Wallrunning State
        if((wallLeft || wallRight) && inputY > 0 && AboveGround())
        {
            //Start Wallrun
            if (!moveScript.wallRunning)
            {
                StartWallRun();
            }
        }

        //Neutral State
        else
        {
            if (moveScript.wallRunning)
            {
                StopWallRun();
            }
        }
    }

    private void StartWallRun()
    {
        moveScript.wallRunning = true;
    }

    private void StopWallRun()
    {
        moveScript.wallRunning = false;
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Vector3.Cross takes the up and right direction to find the forward direction.
        //"?" can be used as an operator, and it says "if the first value is null, use this other value"
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //This is a check to make sure you maintain direction;
        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        //Push towards curved walls
        if (!(wallLeft && inputX > 0) && !(wallRight && inputX < 0))
        {
            Debug.Log("Wall Left: " + wallLeft);
            Debug.Log("Wall Right: " + wallRight);
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }
            

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
    }
}
