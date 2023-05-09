using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shooter", menuName = "ScriptableObject/BHEnemyShooter")]
public class BHShooterScriptableObject : ScriptableObject
{
    /// <summary>
    ///     Name of the Bullet. Can be any string.
    /// </summary>
    public string shooterName = "Shooter";

    [Header("Bullet Stats")]
    public GameObject _bulletPrefab;
    public float bulletSpeed;
    public float bulletSize;
    public float fireDelay;
    public float bulletDamage;

    [Header("Bullet Amount")]
    public bool burst;
    [Range(1, 20)]
    public int minBurst;
    [Range(1, 20)]
    public int maxBurst;

    [Header("Multi-Direction")]
    public bool multiDirection;
    [Range(1, 20)]
    public int minDirections;
    [Range(1, 20)]
    public int maxDirections;


    [Header("Bullet Targeting")]
    public bool lockOn;
    public bool homing;
    [Range(0, 1)]
    public float homingStrength;
    public bool shootFour;


    public GameObject BulletPrefab
    {
        get { return _bulletPrefab; }
    }

    private BHEnemyController _sc;
}