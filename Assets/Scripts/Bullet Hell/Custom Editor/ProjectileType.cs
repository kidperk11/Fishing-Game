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
        crissCrossBullet
    }

    public ProjectilesEnum projectileEnumReference;

    public int multiBulletAmount;
    public float bulletFireDelay;
    public float bulletLifetime;
    public float bulletDamage;
    public float bulletSpeed;

    public int sprayBulletDirection;
}
