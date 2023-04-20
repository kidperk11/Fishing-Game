using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BHMove : MonoBehaviour
{


    [Header("Verical Movement")]
    float inputY;
    public float verticalMoveSpeed;
    [Range(0.6f, .999f)]
    public float dragCoefficient = 0.95f;



    [Header("Horizontal Movement")]
    float inputX;
    public float horizontalMoveSpeed;
    public Transform center;
    public Vector3 axis = Vector3.up;
    public Vector3 desiredPosition;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;


    [Header("Sprites")]
    public GameObject submarineSprite;



    public BHPlayerActions moveActions;
    private InputAction movePlayer;
    Rigidbody rigidBody;

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
        rigidBody = GetComponent<Rigidbody>();
        transform.position = (transform.position - center.position).normalized * radius + center.position;
    }

    private void OnDisable()
    {
        movePlayer.Disable();
    }

    private void Update()
    {
        CurrentInput();
    }

    private void FixedUpdate()
    {   
        // Move the player
        PlayerHeight();
        PlayerRotation();
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

        Debug.Log(String.Format("InputX: {0} InputY {1}", inputX, inputY));
    }


    private void PlayerRotation()
    {
        //center.transform.Rotate(new Vector3(0, (-inputX * horizontalMoveSpeed), 0));
        transform.RotateAround(center.position, axis, -inputX * rotationSpeed * Time.deltaTime);
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
            rigidBody.velocity *= dragCoefficient; // 0.9f is the drag coefficient
        }

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        // Change sprite depending on movement direction
        Vector3 scaleHolder = submarineSprite.transform.localScale;

        if (inputX > 0)
        {
            submarineSprite.transform.localScale = (new Vector3(-0.77f, scaleHolder.y, scaleHolder.z));
        }
        else if (inputX < 0)
        {
            submarineSprite.transform.localScale = (new Vector3(0.77f, scaleHolder.y, scaleHolder.z));
        }
    }
}

