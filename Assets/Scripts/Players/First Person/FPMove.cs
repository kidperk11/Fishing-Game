using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;

    float inputX;
    float inputY;
    public FPPlayerActions moveActions;
    private InputAction movePlayer;

    Vector3 moveDirection;

    Rigidbody rb;

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
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void CurrentInput()
    {
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;
    }

    private void MovePlayer()
    {
        //Calculate Movement Direction
        moveDirection = orientation.forward * inputY + orientation.right * inputX;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);


    }


}
