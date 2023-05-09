using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class BHShooterController : MonoBehaviour
{
    [Tooltip("List of possible Shooters.")]
    [SerializeField]
    public BHShooter shooter;

    [Tooltip("List of possible Shooters.")]
    [SerializeField]
    public List<BHShooter> shooters = new List<BHShooter>();


    public GameObject bulletPrefab;
    public float fireDelay;
    public float bulletSpeed;
    public float bulletSize;
    public float bulletDamage;
    public float bulletLife;

    public Transform bulletSpawnLeft;
    public Transform bulletSpawnRight;
    public Transform cityCenter;

    //Player Inputs
    public BHPlayerActions moveActions;
    private InputAction shootLeft;
    private InputAction shootRight;
    private bool isShootLeft;
    private bool isShootRight;
    private bool coolDownAttack = true;
    private float lastFire;

    private void Awake()
    {
        moveActions = new BHPlayerActions();
    }

    private void OnEnable()
    {
        shootLeft = moveActions.Player.ShootLeft;
        shootRight = moveActions.Player.ShootRight;

        shootLeft.performed += OnShootStart;
        shootLeft.canceled += OnShootEnd;

        shootRight.performed += OnShootStart;
        shootRight.canceled += OnShootEnd;

        shootLeft.Enable();
        shootRight.Enable();
    }

    private void OnDisable()
    {
        shootLeft.Disable();
        shootLeft.performed -= OnShootStart;
        shootLeft.canceled -= OnShootEnd;

        shootRight.Disable();
        shootRight.performed -= OnShootStart;
        shootRight.canceled -= OnShootEnd;
    }

    private void OnShootStart(InputAction.CallbackContext context)
    {
        if (context.action == shootLeft)
        {
            isShootLeft = true;
        }
        if (context.action == shootRight)
        {
            isShootRight = true;
        }
    }

    private void OnShootEnd(InputAction.CallbackContext context)
    {
        isShootLeft = false;
        isShootRight = false;
    }

    void Update()
    {
        if(isShootLeft || isShootRight)
            Attack();

    }

    private void Attack()
    {
        float shootHor = 1f;

        if (Time.time > lastFire + fireDelay)
        {
            if(isShootLeft)
            {
                Debug.Log("Fire Left");
                GameObject bullet = shooters[0].shooterScriptableObject.BulletPrefab;
                bullet = Instantiate(bullet, bulletSpawnLeft.position, bulletSpawnLeft.transform.rotation) as GameObject;
                bullet.GetComponent<BHProjectile>().isEnemyBullet = false;
                bullet.GetComponent<BHProjectile>().Shoot(shootHor, bulletSpeed, bulletSize, bulletDamage, bulletLife, cityCenter);
            }
            if (isShootRight)
            {
                Debug.Log("Fire Right");
                GameObject bullet = shooters[0].shooterScriptableObject.BulletPrefab;
                bullet = Instantiate(bullet, bulletSpawnRight.position, bulletSpawnRight.transform.rotation) as GameObject;
                bullet.GetComponent<BHProjectile>().isEnemyBullet = false;
                bullet.GetComponent<BHProjectile>().Shoot(shootHor, -bulletSpeed, bulletSize, bulletDamage, bulletLife, cityCenter);
            }

            lastFire = Time.time;
            StartCoroutine(CoolDown());
        }
    }


    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(fireDelay);
        coolDownAttack = false;
    }
}
