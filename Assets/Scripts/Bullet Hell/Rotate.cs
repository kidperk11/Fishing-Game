using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate : MonoBehaviour
{
    public Transform center;
    public float horizontalMoveSpeed = 80.0f;
    private float inputX;
    private Vector3 desiredPosition;

    float currentSpeed = 0.0f;
    float targetSpeed = 0.0f;
    float smoothTime = 0.2f;
    float currentVelocity = 0.0f;

    //Player Inputs
    public BHPlayerActions moveActions;
    private InputAction movePlayer;

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
        transform.position = (transform.position - center.position).normalized + center.position;
    }

    private void OnDisable()
    {
        movePlayer.Disable();
    }

    private void Update()
    {
        // Get player input
        inputX = movePlayer.ReadValue<Vector2>().x;

        // Update target speed based on input
        targetSpeed = inputX * horizontalMoveSpeed;
    }

    private void FixedUpdate()
    {
        // Accelerate/decelerate towards target speed
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocity, smoothTime);

        // Rotate player around center
        transform.RotateAround(center.position, Vector3.up, -currentSpeed * Time.deltaTime);

        // Move player towards desired position
        desiredPosition = (transform.position - center.position).normalized + center.position;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * currentSpeed);

    }
}