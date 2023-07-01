using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To be used by both players and enemies. Currently, BHShooterController will be renamed to
// BHPlayerShooterController. But renaming causes Errors.
public class BHNewShooterController : MonoBehaviour
{

    [Tooltip("List of possible Shooters.")]
    [SerializeField]
    internal List<BHShooter> shooters = new List<BHShooter>();
    public Transform bulletSpawnLeft;
    public Transform bulletSpawnRight;
    public Transform cityCenter;

    [Space(10)]
    [Header("Projectile Type")]
    public BHTorpedoStats projectileType;

    internal int shootVector;
    internal int currentWeaponIndex;
    internal float offsetIncreaseAmount = 0f;
    internal float lastFire;
    internal Vector3 spawnOffset = Vector3.zero;
    internal ProjectileStats projectileStats;
    internal Transform bulletSpawnLocation = null;


    internal void GetStats()
    {
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
    }

    private void CalculateOffset()
    {
        spawnOffset = Vector3.zero;
        float yOffset = 0f;

        //TODO: Calculate offset based on size of projectile
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

    internal void SpawnSprayBullet(GameObject bullet, float shootHorizontal, Transform spawnLocation)
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

            bulletOffset.y += offsetIncreaseAmount;
        }
    }

    internal void SpawnMultiBullet(GameObject bullet, float shootHorizontal, Transform spawnLocation)
    {
        Vector3 bulletOffset = spawnOffset;

        for (int i = 0; i < projectileType.multiBulletAmount; i++)
        {
            bullet = Instantiate(bullet, spawnLocation.position + bulletOffset, spawnLocation.rotation) as GameObject;
            bullet.GetComponent<BHProjectile>().isEnemyBullet = false;
            bullet.GetComponent<BHProjectile>().Shoot(cityCenter, projectileStats);

            lastFire = Time.time;

            bulletOffset.y += offsetIncreaseAmount;
        }
    }

    internal void SpawnHelixBullet(GameObject bullet, Transform spawnLocation)
    {
        projectileStats.verticalDirection = -1;

        for (int i = 0; i <= 2; i++)
        {
            bullet = Instantiate(bullet, spawnLocation.position, spawnLocation.rotation) as GameObject;
            bullet.GetComponent<BHProjectile>().isEnemyBullet = false;
            bullet.GetComponent<BHProjectile>().ShootHelix(cityCenter, projectileStats, projectileType.verticalSpeed, projectileType.verticalRange);
            projectileStats.verticalDirection = 1;

            lastFire = Time.time;
        }
    }
}
