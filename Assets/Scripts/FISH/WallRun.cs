using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;
using MoreMountains.Feel;
using MoreMountains.Tools;

public class WallRun : MonoBehaviour
{
    [Header("WallRunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public float inputX;
    public float inputY;
    public FPPlayerActions moveActions;
    private InputAction movePlayer;
    private InputAction wallJump;
    public bool buttonHeld;

    [Header("Exiting")]
    [SerializeField] private int maxWallRuns;
    [SerializeField] private int wallRunCounter;
    private bool exitingWall;
    public float exitWallTime;
    public float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;


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
    public FPCam cam;
    public MMF_Player wallRunSoundStart;
    public MMF_Player wallRunSoundEnd;

    [Header("Timers")]
    [SerializeField] private float soundTimer;
    [SerializeField] private float soundInterval;

    [Header("Sound Effects")]
    public AudioSource jumpSound;

    private void OnEnable()
    {
        movePlayer = moveActions.Player.Move;
        movePlayer.Enable();
        wallJump = moveActions.Player.Jump;
        wallJump.Enable();
        wallJump.started += JumpHeld;
        wallJump.canceled += WallJump;
    }

    private void OnDisable()
    {
        movePlayer.Disable();
        wallJump.Disable();
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
        if (moveScript.grounded)
        {
            wallRunCounter = 0;
        }
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
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        //Get inputs
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;

        //Wallrunning State
        if((wallLeft || wallRight) && buttonHeld && AboveGround() && !exitingWall && wallRunCounter < maxWallRuns)
        {
            if(wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
                if(wallRunTimer <= 0 && moveScript.wallRunning)
                {
                    exitingWall = true;
                    exitWallTimer = exitWallTime;
                }
            }
            //Start Wallrun
            if (!moveScript.wallRunning)
            {
                StartWallRun();
            }
        }

        //Exit State
        else if (exitingWall)
        {
            if (moveScript.wallRunning)
            {
                StopWallRun();
            }

            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            if(exitWallTimer <= 0)
            {
                exitingWall = false;
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
        wallRunTimer = maxWallRunTime;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        

        //camera effects
        //cam.DoFOV(70);
        if (wallLeft) cam.DoWallTilt("right");
        if (wallRight) cam.DoWallTilt("left");
        wallRunSoundStart.PlayFeedbacks();
    }

    private void StopWallRun()
    {
        wallRunCounter++;
        wallRunSoundEnd.PlayFeedbacks();
        moveScript.wallRunning = false;

        //cam.DoFOV(60f);
        cam.DoWallTilt("reset");

    }

    private void WallRunningMovement()
    {
        //soundTimer += Time.deltaTime;
        //if(soundTimer >= soundInterval)
        //{
        //    soundTimer = 0;
        //    wallRunSoundStart.PlayFeedbacks();
        //}
        rb.useGravity = useGravity;
        

        //Vector3.Cross takes the up and right direction to find the forward direction.
        //"?" can be used as an operator, and it says "if the first value is null, use this other value"
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //This is a check to make sure you maintain direction;
        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        //Pushes towards curved walls and allows you to look away from flat walls a bit
        if (!(wallLeft && inputX > 0) && !(wallRight && inputX < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        if (useGravity)
        {
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }
            
        //Normal forward force for the wall run.
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
    }

    private void WallJump(InputAction.CallbackContext context)
    {
        buttonHeld = false;
        if (moveScript.wallRunning)
        {
            jumpSound.Play();
            exitingWall = true;
            exitWallTimer = exitWallTime;
            Debug.Log("WallJump has been started");
            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

            Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
            Debug.Log(forceToApply);

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(forceToApply, ForceMode.Impulse);
        }
        
    }

    private void JumpHeld(InputAction.CallbackContext context)
    {
        buttonHeld = true;
        Debug.Log("Button Held Value: " + buttonHeld);
    }
}
