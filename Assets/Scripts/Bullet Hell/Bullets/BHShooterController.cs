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

public struct ProjectileStats
{
    public float fireDelay;
    public float bulletSpeed;
    public float bulletSize;
    public float bulletDamage;
    public float bulletLife;
    public float verticalDirection;
    public SubmarineWeapons weaponType;
    public ProjectileType statsProjectileType;
 }

public class BHShooterController : MonoBehaviour
{
    [Tooltip("List of possible Shooters.")]
    [SerializeField]
    public List<BHShooter> shooters = new List<BHShooter>();

    public GameObject bulletPrefab;
    public Transform bulletSpawnLeft;
    public Transform bulletSpawnRight;
    public Transform cityCenter;

    [Space(10)]
    [Header("Projectile Type")]
    public ProjectileType projectileType;

    private int currentWeaponIndex;
    private int shootVector;
    private float lastFire;
    private float shootHor = 1f;
    private float offsetIncreaseAmount = 0f;
    private Vector3 spawnOffset = Vector3.zero;
    private SubmarineWeapons currentWeapon;
    private ProjectileStats projectileStats;


    private void OnEnable()
    {
        currentWeapon = SubmarineWeapons.explosive;
        currentWeaponIndex = 0;
    }

    private void Update()
    {
        if (shootVector != 0)
            Attack();
    }

    public void OnShootStart(Vector2 shootDirection)
    {
        if (shootDirection.x == -1)
        {
            shootVector = -1;
        }
        if (shootDirection.x == 1)
        {
            shootVector = 1;
        }
    }

    public void OnShootEnd(InputAction.CallbackContext context)
    {
        shootVector = 0;
    }
    
    public void SwapWeapon()
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
        projectileStats.weaponType = currentWeapon;
    }



    private void Attack()
    {
        Transform bulletSpawnLocation = null;
        
        if (Time.time > lastFire + projectileType.bulletFireDelay)
        {
            //Grab Projectile info from Serialized Object
            GameObject bullet = shooters[currentWeaponIndex].shooterScriptableObject.BulletPrefab;

            projectileStats.bulletSpeed = projectileType.bulletSpeed;
            projectileStats.bulletSize = shooters[currentWeaponIndex].shooterScriptableObject.bulletSize;
            projectileStats.bulletLife = projectileType.bulletLifetime;
            projectileStats.bulletDamage = projectileType.bulletDamage;
            projectileStats.statsProjectileType = projectileType;

            switch (shootVector)
            {
                case -1:

                    projectileStats.bulletSpeed = Mathf.Abs(projectileStats.bulletSpeed);
                    bulletSpawnLocation = bulletSpawnLeft;
                    break;

                case 1:

                    if (projectileStats.bulletSpeed >= 0)
                        projectileStats.bulletSpeed = -projectileStats.bulletSpeed;
                   
                    bulletSpawnLocation = bulletSpawnRight; 
                    break;
            }

            //When firing multiple bullets, offset them so they are in the center.
            CalculateOffset();

            //Does this need a comment?
            SpawnBullets(bullet, shootHor, bulletSpawnLocation);

        }
    }

    private void CalculateOffset()
    {
        spawnOffset = Vector3.zero;
        float yOffset = 0f;

        switch (projectileType.multiBulletAmount)
        {
            case 1:
                yOffset = 0f;
                break;
            case 2:
                yOffset = -.07f;
                break;
            case 3:
                yOffset = -.15f;
                break;
            case 4:
                yOffset = -.2f;
                break;
            case 5:
                yOffset = -.25f;
                break;
            default:
                break;
        }

        spawnOffset = new Vector3(0, yOffset, 0);
        offsetIncreaseAmount = .15f;
    }

    private void SpawnBullets(GameObject bullet, float shootHorizontal, Transform spawnLocation)
    {
        switch (projectileType.projectileEnumReference)
        {
            case ProjectileType.ProjectilesEnum.multiBullet:
                SpawnMultiBullet(bullet, shootHorizontal, spawnLocation);
                break;
            case ProjectileType.ProjectilesEnum.sprayBullet:
                SpawnSprayBullet(bullet, shootHorizontal, spawnLocation);
                break;
            case ProjectileType.ProjectilesEnum.crissCrossBullet:
                SpawnCrissCrossBullet(bullet, shootHorizontal, spawnLocation);
                break;
        }
    }

    private void SpawnMultiBullet(GameObject bullet, float shootHorizontal, Transform spawnLocation)
    {
        Vector3 bulletOffset = spawnOffset;

        for (int i = 0; i < projectileType.multiBulletAmount; i++)
        {
            bullet = Instantiate(bullet, spawnLocation.position + bulletOffset, spawnLocation.rotation) as GameObject;
            bullet.GetComponent<BHProjectile>().isEnemyBullet = false;
            bullet.GetComponent<BHProjectile>().Shoot(cityCenter, projectileStats);

            lastFire = Time.time;
            StartCoroutine(CoolDown());

            bulletOffset.y += offsetIncreaseAmount;
        }
    }

    private void SpawnSprayBullet(GameObject bullet, float shootHorizontal, Transform spawnLocation)
    {
        Vector3 bulletOffset = spawnOffset;

        projectileStats.verticalDirection = -1;

        for (int i = 0; i <= 2; i++)
        {
            bullet = Instantiate(bullet, spawnLocation.position + bulletOffset, spawnLocation.rotation) as GameObject;
            bullet.GetComponent<BHProjectile>().isEnemyBullet = false;
            bullet.GetComponent<BHProjectile>().Shoot(cityCenter, projectileStats);
            projectileStats.verticalDirection += 1;

            lastFire = Time.time;
            StartCoroutine(CoolDown());

            bulletOffset.y += offsetIncreaseAmount;
        }
    }

    private void SpawnCrissCrossBullet(GameObject bullet, float shootHorizontal, Transform spawnLocation)
    {
        Vector3 bulletOffset = new Vector3(0, 0, 0);

        projectileStats.verticalDirection = -1;

        for (int i = 0; i <= 2; i++)
        {
            bullet = Instantiate(bullet, spawnLocation.position + bulletOffset, spawnLocation.rotation) as GameObject;
            bullet.GetComponent<BHProjectile>().isEnemyBullet = false;
            bullet.GetComponent<BHProjectile>().Shoot(cityCenter, projectileStats);
            projectileStats.verticalDirection = 1;

            lastFire = Time.time;
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(projectileType.bulletFireDelay);
    }
}
