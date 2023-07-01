using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public enum SubmarineWeaponType
{
    explosive,
    retractable,
    specialAbility
}



public struct ProjectileStats
{
    public float fireDelay;
    public float bulletSpeed;
    public float bulletSize;
    public float bulletDamage;
    public float bulletLife;
    public float verticalDirection;
    public SubmarineWeaponType weaponType;
    public BHTorpedoStats statsProjectileType;
 }

public class BHShooterController : BHNewShooterController
{
    private float shootHor = 1f;

    private SubmarineWeaponType currentWeapon;

    // Remote Explosive
    public bool singleProjectileActive;
    public BHProjectile activeRemoteExplosive;

    private void OnEnable()
    {
        currentWeapon = SubmarineWeaponType.explosive;
        currentWeaponIndex = 0;
    }

    public void OnShootStart(InputAction.CallbackContext context, int newShootVector)
    {
        shootVector = newShootVector;
    }

    public void OnShootEnd(InputAction.CallbackContext context)
    {
        shootVector = 0;
    }

    public void SwapWeapon()
    {
        switch (currentWeapon)
        {
            case SubmarineWeaponType.explosive:
                currentWeapon = SubmarineWeaponType.retractable;
                currentWeaponIndex = 1;
                Debug.Log("Current Weapon: " + currentWeapon);
                break;
            case SubmarineWeaponType.retractable:
                currentWeapon = SubmarineWeaponType.explosive;
                currentWeaponIndex = 0;
                Debug.Log("Current Weapon: " + currentWeapon);
                break;
        }
        projectileStats.weaponType = currentWeapon;
    }

    private void Update()
    {
        if(shootVector != 0)
        {
            GameObject bullet = shooters[currentWeaponIndex].shooterScriptableObject.BulletPrefab;
            GetStats();


            switch (projectileType.projectileEnumReference)
            {
                case BHTorpedoType.multiBullet:
                    if(Time.time > lastFire + projectileType.bulletFireDelay)
                        SpawnMultiBullet(bullet, shootHor, bulletSpawnLocation);
                    break;


                case BHTorpedoType.sprayBullet:
                    if (Time.time > lastFire + projectileType.bulletFireDelay)  
                        SpawnSprayBullet(bullet, shootHor, bulletSpawnLocation);
                    break;


                case BHTorpedoType.helixBullet:
                    if (Time.time > lastFire + projectileType.bulletFireDelay)
                        SpawnHelixBullet(bullet, bulletSpawnLocation);   
                    break;


                case BHTorpedoType.remoteExplosive:
                    if (Time.time > lastFire + projectileType.remoteExplodeFireDelay)
                    {
                        if (!singleProjectileActive)
                        {
                            SpawnRemoteExplodeBullet(bullet, bulletSpawnLocation);
                            break;

                        }
                        else if (singleProjectileActive && activeRemoteExplosive != null)
                        {
                            activeRemoteExplosive.ManualRemoteExplode(this);
                        }
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Fired only by player. Press fire once to fire bullet, which explodes after set time for small damage.
    /// or press fire again to manually explode for more damage
    /// </summary>
    /// <param name="bullet"></param>
    /// <param name="spawnLocation"></param>
    private void SpawnRemoteExplodeBullet(GameObject bullet, Transform spawnLocation)
    {
        Vector3 bulletOffset = spawnOffset;

        bullet = Instantiate(bullet, spawnLocation.position + bulletOffset, spawnLocation.rotation) as GameObject;
        activeRemoteExplosive = bullet.GetComponent<BHProjectile>();
        bullet.GetComponent<BHProjectile>().isEnemyBullet = false;
        bullet.GetComponent<BHProjectile>().ShootRemoteExplosive(cityCenter, projectileStats, this);

        lastFire = Time.time;
    }
}
