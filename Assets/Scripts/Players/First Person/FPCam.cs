using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;


public class FPCam : MonoBehaviour
{
    [Header("Transform and Camera References")]
    public CinemachineVirtualCamera fpsCam;
    public Image innerCrosshair;
    public Transform orientation;
    public Transform camHolder;

    [Header("Inputs and Input Handling")]
    public float mouseSenseX;
    public float mouseSenseY;
    public Vector2 controllerSenseScale;
    public FPPlayerActions cameraControls;
    public PlayerInput playerInput;
    float xRotation;
    float yRotation;
    private InputAction moveCam;

    [Header("Animation")]
    public Animator anim;

    private void OnEnable()
    {
        moveCam = cameraControls.Player.Look;
        moveCam.Enable();
    }

    private void OnDisable()
    {
        moveCam.Disable();
    }

    private void Awake()
    {
        cameraControls = new FPPlayerActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Locks cursor to middle of screen and makes it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX;
        float inputY;

        //This code will scale the controller's sensitivity to the mouse to make
        //it feel like the same speed as the mouse.
        if (playerInput.currentControlScheme == "Gamepad")
        {
            //Get a reference to the current camera input
            inputX = moveCam.ReadValue<Vector2>().x * mouseSenseX * controllerSenseScale.x;
            inputY = moveCam.ReadValue<Vector2>().y * mouseSenseY * controllerSenseScale.y;
        }
        else
        {
            //Get a reference to the current camera input
            inputX = moveCam.ReadValue<Vector2>().x * Time.deltaTime * mouseSenseX;
            inputY = moveCam.ReadValue<Vector2>().y * Time.deltaTime * mouseSenseY;
        }
        

        yRotation += inputX;

        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if(playerInput.currentControlScheme == "Gamepad")
        {
            ////Rotate camera
            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            ////Rotate player
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

            //NOTE: Add code for auto aim on controller
        }
        else
        {
            //Rotate camera
            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            //Rotate player
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        } 
    }

    //NOTE: This code is unused for now, but if FOV needs to be adjusted with a tween,
    //we can try re-implementing it.

    //public void DoFOV(float endValue)
    //{
        
    //    Debug.Log("Field of view adjustment to: " + endValue);
    //}

    public void DoWallTilt(string tiltDirection)
    {
        if (tiltDirection == "left")
        {
            anim.SetTrigger("wallTiltLeft");
        }
        if (tiltDirection == "right")
        {
            anim.SetTrigger("wallTiltRight");
        }
        if (tiltDirection == "reset")
        {
            anim.SetTrigger("resetTilt");
        }
    }

    public void DoNormalLandTilt()
    {
        anim.SetTrigger("normalLand");
        Debug.Log("Trigger normalLand has been set");
    }


}
