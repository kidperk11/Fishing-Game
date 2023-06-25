using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProjectileType
{
    public string projectileName;

    public enum ProjectilesEnum
    {
        multiBullet,
        sprayBullet,
        helixBullet,
        remoteExplosive
    }

    public ProjectilesEnum projectileEnumReference;

    public int multiBulletAmount;
    public float bulletFireDelay;
    public float bulletLifetime;
    public float bulletDamage;
    public float bulletSpeed;

    // Helix bullet
    public float verticalSpeed; // Speed of vertical movement
    public float verticalRange; // Range of vertical movement

    // Spray Bullet
    public int sprayBulletDirection;

    // Remote Detonation
    public float autoDetonationTimer;
}
