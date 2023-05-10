using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public enum SubmarineWeapons
{
    explosive,
    retractable,
    specialAbility
}

public enum SpecialAbilities
{
    pufferFish,
    anglerFish,
    swordFish,
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
    private bool coolDownAttack = true;
    private float lastFire;
    private SubmarineWeapons currentWeapon;
    private int currentWeaponIndex;
    private int shootVector;

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

        currentWeapon = SubmarineWeapons.explosive;
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
            shootVector = -1;
        }
        if (context.action == shootRight)
        {
            shootVector = 1;
        }
    }

    private void OnShootEnd(InputAction.CallbackContext context)
    {
        shootVector = 0;
    }
    
    private void SwapWeapon(InputAction.CallbackContext context)
    {
        switch(currentWeapon)
        {
            case SubmarineWeapons.explosive:
                currentWeapon = SubmarineWeapons.retractable;
                currentWeaponIndex = 1;
                Debug.Log("Current Weapon: " + currentWeapon);
                break;
            case SubmarineWeapons.retractable:
                currentWeapon = SubmarineWeapons.explosive;
                currentWeaponIndex = 0;
                Debug.Log("Current Weapon: " + currentWeapon);
                break;
        }
    }

    void Update()
    {
        if (shootVector != 0)
            Attack();
    }

    private void Attack()
    {
        float shootHor = 1f;
        Transform bulletSpawnLocation = null;
        
        if (Time.time > lastFire + fireDelay)
        {

            GameObject bullet = shooters[currentWeaponIndex].shooterScriptableObject.BulletPrefab;
            bulletSpeed = shooters[currentWeaponIndex].shooterScriptableObject.bulletSpeed;

            switch (shootVector)
            {
                case -1:
                    
                    bulletSpeed = Mathf.Abs(bulletSpeed);
                    bulletSpawnLocation = bulletSpawnLeft;
                    break;

                case 1:

                    if (bulletSpeed >= 0)
                        bulletSpeed = -bulletSpeed;
                   
                    bulletSpawnLocation = bulletSpawnRight;
                    break;
            }

            bullet = Instantiate(bullet, bulletSpawnLocation.position, bulletSpawnLocation.rotation) as GameObject;
            bullet.GetComponent<BHProjectile>().isEnemyBullet = false;
            bullet.GetComponent<BHProjectile>().Shoot(shootHor, bulletSpeed, bulletSize, bulletDamage, bulletLife, cityCenter, currentWeapon);

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
