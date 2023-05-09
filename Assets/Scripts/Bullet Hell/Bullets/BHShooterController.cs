using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public enum SubmarineWeapons
{
    torpedo,
    captureNet,

}

public class BHShooterController : MonoBehaviour
{
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
    private InputAction weaponSwap;
    private bool isShootLeft;
    private bool isShootRight;
    private bool coolDownAttack = true;
    private float lastFire;
    private SubmarineWeapons currentWeapon;
    private int currentWeaponIndex;

    private void Awake()
    {
        moveActions = new BHPlayerActions();
    }

    private void OnEnable()
    {
        shootLeft = moveActions.Player.ShootLeft;
        shootRight = moveActions.Player.ShootRight;
        weaponSwap = moveActions.Player.WeaponSwap;

        weaponSwap.performed += SwapWeapon;
        shootLeft.performed += OnShootStart;
        shootLeft.canceled += OnShootEnd;

        shootRight.performed += OnShootStart;
        shootRight.canceled += OnShootEnd;

        shootLeft.Enable();
        shootRight.Enable();
        weaponSwap.Enable();

        currentWeapon = SubmarineWeapons.torpedo;
        currentWeaponIndex = 0;
    }

    private void OnDisable()
    {
        shootLeft.Disable();
        shootLeft.performed -= OnShootStart;
        shootLeft.canceled -= OnShootEnd;

        shootRight.Disable();
        shootRight.performed -= OnShootStart;
        shootRight.canceled -= OnShootEnd;

        weaponSwap.Disable();
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
    
    private void SwapWeapon(InputAction.CallbackContext context)
    {
        if(currentWeapon == SubmarineWeapons.torpedo)
        {
            currentWeapon = SubmarineWeapons.captureNet;
            currentWeaponIndex = 1;
            Debug.Log("Current Weapon: " + currentWeapon);
            return;
        }

        if(currentWeapon == SubmarineWeapons.captureNet)
        {
            currentWeapon = SubmarineWeapons.torpedo;
            currentWeaponIndex = 0;
            Debug.Log("Current Weapon: " + currentWeapon);
            return;
        }

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
                Debug.Log("Current Weapon: " + currentWeapon + " with index of " + currentWeaponIndex);
                GameObject bullet = shooters[currentWeaponIndex].shooterScriptableObject.BulletPrefab;
                bullet = Instantiate(bullet, bulletSpawnLeft.position, bulletSpawnLeft.transform.rotation) as GameObject;
                bullet.GetComponent<BHProjectile>().isEnemyBullet = false;
                bullet.GetComponent<BHProjectile>().Shoot(shootHor, bulletSpeed, bulletSize, bulletDamage, bulletLife, cityCenter);
            }
            if (isShootRight)
            {
                GameObject bullet = shooters[currentWeaponIndex].shooterScriptableObject.BulletPrefab;
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
