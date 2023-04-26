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
    [Header("Sprites")]
    public SpriteRenderer submarine;
    public SpriteRenderer rotor;


    protected static BHPlayerController s_Instance;
    public static BHPlayerController instance { get { return s_Instance; } }
    //Player Inputs
    public BHPlayerActions moveActions;
    private InputAction movePlayer;
    public Rigidbody rigidBody;

    private void Awake()
    {
        moveActions = new BHPlayerActions();
    }

    private void OnEnable()
    {
        movePlayer = moveActions.Player.Move;
        movePlayer.Enable();
    }

    private void Start()
    {
        s_Instance = this;
        //rigidBody = GetComponent<Rigidbody>();
        transform.position = (transform.position - center.position).normalized * radius + center.position;
    }

    private void OnDisable()
    {
        movePlayer.Disable();
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

    private void CurrentInput()
    {
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;

        //Rounds up input values when moving diagonally 
        if (inputX != 0)
            inputX = Mathf.Sign(inputX);
        if (inputY != 0)
            inputY = Mathf.Sign(inputY);
    }

    private void PlayerRotate()
    {
        // Accelerate/decelerate towards target speed
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocity, smoothTime);

        if (autoRotate)
        {
            transform.RotateAround(center.position, Vector3.up, horizontalMoveSpeed * Time.deltaTime);
        }
        else
        {
            // Rotate player around center
            transform.RotateAround(center.position, Vector3.up, -currentSpeed * Time.deltaTime);
        }

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

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (inputX > 0)
        {
            submarine.flipX = true;
        }
        else if (inputX < 0)
        {
            submarine.flipX = false;
        }
    }
}

