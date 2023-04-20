using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BHMove : MonoBehaviour
{
    public bool allowVertical;
    public bool allowHorizontal;

    [Header("Verical Movement")]
    public float verticalMoveSpeed;
    [Range(0.6f, .999f)]
    public float dragCoefficient = 0.95f;
    private float inputY;

    [Space(10)]
    [Header("Horizontal Movement")]
    public Transform center;
    public bool autoRotate;
    public bool applyDampen;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float horizontalMoveSpeed = 80.0f;
    private float inputX;
    private Vector3 axis = Vector3.up;
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
        if(allowVertical)
            PlayerHeight();


        if (allowHorizontal)
        {
            if (applyDampen)
                PlayerRotationWithDampen();
            else
                PlayerRotation();
        }
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

        //Debug.Log(String.Format("InputX: {0} InputY {1}", inputX, inputY));
    }


    private void PlayerRotation()
    {
        if (!autoRotate)
        {
            transform.RotateAround(center.position, axis, -inputX * horizontalMoveSpeed * Time.deltaTime);

        }
        else
        {
            transform.RotateAround(center.position, axis, horizontalMoveSpeed * Time.deltaTime);
        }

        desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);

        Debug.Log(String.Format("InputX: {0} InputY {1}", radiusSpeed, desiredPosition));
    }

    private void PlayerRotationWithDampen()
    {
        // Accelerate/decelerate towards target speed
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocity, smoothTime);

        // Rotate player around center
        transform.RotateAround(center.position, axis, -currentSpeed * Time.deltaTime);

        // Move player towards desired position
        desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * currentSpeed);
    }

    private void PlayerHeight()
    {
        // Apply movement force to the Rigidbody
        rigidBody.AddForce(new Vector2(0, inputY) * verticalMoveSpeed);

        // Reduce velocity gradually when no input is given
        if  (inputY == 0)
        {
            if (!applyDampen)
                rigidBody.velocity *= 0;
            else
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

