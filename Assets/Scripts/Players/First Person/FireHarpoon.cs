using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FireHarpoon : MonoBehaviour
{
    public FPPlayerActions playerActions;
    public Rigidbody playerRB;
    private InputAction fireHarpoon;
    public Camera fpsCam;
    public FPFireGun fireGun;
    

    //Crosshair
    public Image crosshair;
    public Color defaultCrosshairColor;
    public Color enemyCrosshairColor;
    public Color grapplePointCrosshairColor;


    //Harpoon
    private HarpoonController harpoonInstance;
    public GameObject harpoon;
    public Transform harpoonSpawnPoint;
    [SerializeField] private float harpoonRange;
    public bool readyToFire;


    [Header("Sound Effects")]
    public AudioSource fire;
    public AudioSource reel;
    public AudioSource clickIn;

    private void OnEnable()
    {
        fireHarpoon = playerActions.Player.Harpoon;
        fireHarpoon.Enable();

        fireHarpoon.performed += Fire;
    }

    private void OnDisable()
    {
        fireHarpoon.Disable();
    }

    private void Awake()
    {
        playerActions = new FPPlayerActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        readyToFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        CrosshairColor();   
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (readyToFire)
        {

            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(75); //This is just a point 75m from the player's LOS
            }

            Vector3 directionWithoutSpread = targetPoint - harpoonSpawnPoint.position;

            harpoonInstance = Instantiate(harpoon, harpoonSpawnPoint.position, Quaternion.identity).GetComponent<HarpoonController>();
            harpoonInstance.gameObject.transform.forward = directionWithoutSpread.normalized;
            harpoonInstance.rb.AddForce(directionWithoutSpread.normalized * harpoonInstance.harpoonSpeed, ForceMode.Impulse);
            harpoonInstance.harpoonGun = this;
            harpoonInstance.harpoonRange = this.harpoonRange;
            harpoonInstance.initialPlayerPosition = this.transform.position;
            readyToFire = false;
            Debug.Log("Harpoon has been fired");
        }
        else
        {
            Debug.Log("Cannot fire harpoon, value of readyToFire is: " + readyToFire);
        }
    }

    private void CrosshairColor()
    {
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Vector3.Distance(this.transform.position, hit.transform.position) <= harpoonRange)
            {
                //Add additional colors for additional tags here
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    crosshair.color = enemyCrosshairColor;
                }
                else if (hit.transform.gameObject.CompareTag("GrapplePoint"))
                {
                    crosshair.color = grapplePointCrosshairColor;
                }
                else { crosshair.color = defaultCrosshairColor; }
            }
            else { crosshair.color = defaultCrosshairColor; }
        }
    }

    public void ResetFire()
    {
        readyToFire = true;
    }

    public void SendBulletToGun(string bulletType)
    {
        fireGun.SetSpecialBullet(bulletType);
    }
}
