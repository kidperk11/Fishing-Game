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

    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float horizontalMoveSpeed = 80.0f;
    private float inputX;
    private Vector3 desiredPosition;
    float currentSpeed = 0.0f;
    float targetSpeed = 0.0f;
    float smoothTime = 0.2f;
    float currentVelocity = 0.0f;

    public float rotationDamping = 2f;
    public float movementDamping = 2;

    [Space(10)]
    [Header("Other")]
    public BHPlayerSpriteController spriteController;
    public BHShooterController playerShootingController;
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
        Vector2 shootDirection = new Vector2(0, 0);

        if (context.action == shootLeft)
        {
            shootDirection = new Vector2(-1, 0);
        }
        if (context.action == shootRight)
        {
            shootDirection = new Vector2(1, 0);
        }

        playerShootingController.OnShootStart(shootDirection);
    }

    private void OnShootEnd(InputAction.CallbackContext context)
    {
        playerShootingController.OnShootEnd(context);
    }

    private void SwapWeapon(InputAction.CallbackContext context)
    {
        playerShootingController.SwapWeapon();
    }

    private void CurrentInput()
    {
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;

        //Rounds up input values when moving diagonally 
        if (inputX != 0)
            inputX = Mathf.Sign(inputX);
        if (inputY != 0)
            inputY = Mathf.Sign(inputY);


        spriteController.FlipSprite(inputX);
    }

    private void PlayerRotate()
    {
        // Accelerate/decelerate towards target speed
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocity, smoothTime);

        if (autoRotate)
        {
            transform.RotateAround(center.position, Vector3.up, horizontalMoveSpeed * Time.deltaTime);
            NewRadius();
        }
        else
        {
            // Rotate player around center
            transform.RotateAround(center.position, Vector3.up, -currentSpeed * Time.deltaTime);
        }
    }

    private void NewRadius()
    {
        // Move player towards desired position
        desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }

    private void PlayerHeight()
    {
        // Apply movement force to the Rigidbody
        rigidBody.AddForce(new Vector2(0, inputY) * verticalMoveSpeed);

        // Reduce velocity gradually when no input is given
        if  (inputY == 0)
        {
            rigidBody.velocity *= dragCoefficient;
        }
    }
}

