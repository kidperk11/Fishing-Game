using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BHPlayerController : MonoBehaviour
{
    public bool autoRotate;

    [Header("Verical Movement")]
    public float verticalMoveSpeed;
    [Range(0.6f, .999f)]
    public float dragCoefficient = 0.95f;
    private float inputY;

    [Space(10)]
    [Header("Horizontal Movement")]
    public Transform center;

    
    public float radiusSpeed = 0.5f;
    public float horizontalMoveSpeed = 80.0f;
    private float currentSpeed = 0.0f;
    private float targetSpeed = 0.0f;
    private float smoothTime = 0.2f;
    private float currentVelocity = 0.0f;
    private float inputX;
    private Vector3 desiredPosition;
    private float radius = 5f;
    public float m_Radius { set { radius = value; } }

    public float rotationDamping = 2f;
    public float movementDamping = 2;

    [Space(10)]
    [Header("Other")]
    public Transform topLimit;
    public Transform bottomLimit;
    public BHPlayerSpriteController spriteController;
    public BHShooterController playerShootingController;
    public BHGameManager gameManager;
    public Rigidbody rigidBody;
    //Angle on cylinder 
    public float angularPosition = -90;

    protected static BHPlayerController s_Instance;
    public static BHPlayerController instance { get { return s_Instance; } }
    //Player Inputs
    public BHPlayerActions moveActions;


    private InputAction shootRight;
    private InputAction shootLeft;

    private InputAction movePlayer;
    private InputAction weaponSwap;

    private float downHeightLimiter;
    private float upHeightLimiter;

    private float distanceFromTop;
    private float distanceFromBottom;

    private void Awake()
    {
        moveActions = new BHPlayerActions();
    }

    private void OnEnable()
    {
        shootRight = moveActions.Player.ShootRight;


        shootRight.performed += OnShootStart;
        shootRight.canceled += OnShootEnd;

        shootRight.Enable();

        movePlayer = moveActions.Player.Move;

        shootLeft = moveActions.Player.ShootLeft;
        weaponSwap = moveActions.Player.WeaponSwap;

        weaponSwap.performed += SwapWeapon;

        shootLeft.performed += OnShootStart;
        shootLeft.canceled += OnShootEnd;

        shootLeft.Enable();
        weaponSwap.Enable();
        movePlayer.Enable();

    }

    private void Start()
    {
        s_Instance = this;
        //rigidBody = GetComponent<Rigidbody>();
        transform.position = (transform.position - center.position).normalized * radius + center.position;

    }

    private void Update()
    {
        CurrentInput();

        // Update target speed based on input
        targetSpeed = inputX * horizontalMoveSpeed;


        distanceFromTop =
    (rigidBody.transform.position.y - topLimit.transform.position.y) / (topLimit.position.y - bottomLimit.position.y);

        distanceFromBottom =
            (rigidBody.transform.position.y - bottomLimit.transform.position.y) / (topLimit.position.y - bottomLimit.position.y);
    }

    private void FixedUpdate()
    {
        // Move the player
        PlayerHeight();
        PlayerRotate();
    }

    private void OnDisable()
    {
        shootLeft.Disable();
        shootLeft.performed -= OnShootStart;
        shootLeft.canceled -= OnShootEnd;

        shootRight.Disable();
        shootRight.performed -= OnShootStart;
        shootRight.canceled -= OnShootEnd;

        movePlayer.Disable();
    }

    private void AdjustPlayer()
    {
        transform.rotation = Quaternion.identity;
        // Convert angle to radians
        float radians = angularPosition * Mathf.Deg2Rad;

        // Calculate position on the circumference
        float x = radius * Mathf.Cos(radians);
        float y = 0f;
        float z = radius * Mathf.Sin(radians);

        // Set the object's position
        transform.position = new Vector3(x, y, z);
    }



    private void OnShootStart(InputAction.CallbackContext context)
    {
        if (context.action == shootLeft)
        {
            //Shoot on the left, -1
            playerShootingController.OnShootStart(context, -1);
        }
        if (context.action == shootRight)
        {
            //Shoot on the right, 1
            playerShootingController.OnShootStart(context, 1);
        }
    }

    private void OnShootEnd(InputAction.CallbackContext context)
    {
        playerShootingController.OnShootEnd(context);
    }

    private void SwapWeapon(InputAction.CallbackContext context)
    {
        playerShootingController.SwapWeapon();
        Debug.Log("Player Switched weapons");
    }

    private void CurrentInput()
    {
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;

        //Rounds up input values when moving diagonally 
        if (inputX != 0)
            inputX = Mathf.Sign(inputX);

        if (inputY != 0 && inputY > 0)
        {
            inputY = Mathf.Sign(inputY) * upHeightLimiter;
        }

        if(inputY != 0 && inputY < 0)
        {
            inputY = Mathf.Sign(inputY) * downHeightLimiter;
        }

        spriteController.FlipSprite(inputX);
    }

    private void PlayerRotate()
    {
        // Accelerate/decelerate towards target speed
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocity, smoothTime);

        if (autoRotate)
        {
            transform.RotateAround(center.position, Vector3.up, horizontalMoveSpeed * Time.deltaTime);
            RotateToNewRadius();
        }
        else
        {
            // Rotate player around center
            transform.RotateAround(center.position, Vector3.up, -currentSpeed * Time.deltaTime);
        }
    }

    internal void UpdateRadius(int newRadius)
    {
        radius = newRadius;
    }

    private void RotateToNewRadius()
    {
        // Move player towards desired position
        desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }

    private void PlayerHeight()
    {

        downHeightLimiter = 1f;
        upHeightLimiter = 1f;

        // Apply movement force to the Rigidbody
        rigidBody.AddForce(new Vector2(0, inputY) * verticalMoveSpeed);

        // Reduce velocity gradually when no input is given
        if  (inputY == 0)
        {
            rigidBody.velocity *= dragCoefficient;
        }


        if (distanceFromBottom < 0.1f)
        {
            rigidBody.AddForce(new Vector2(0, 1) * verticalMoveSpeed);
            downHeightLimiter = 0;
            return;
        }
        if (distanceFromBottom < 0.2f)
        {
            downHeightLimiter = .1f;
            return;
        }
        if (distanceFromBottom < 0.3f)
        {
            downHeightLimiter = .3f;
            return;
        }

        if (distanceFromTop > -0.2f)
        {
            rigidBody.AddForce(new Vector2(0, -1) * verticalMoveSpeed);
            upHeightLimiter = 0f;
            return;
        }
        if (distanceFromTop > -0.3f)
        {
            upHeightLimiter = 0.1f;
            return;
        }
        if (distanceFromTop > -0.4f)
        {
            upHeightLimiter = 0.3f;
            return;
        }
        
    }

}

