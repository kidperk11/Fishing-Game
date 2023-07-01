[System.Serializable]
public struct BHTorpedoStats
{
    public string projectileName;

    public BHTorpedoType projectileEnumReference;

    // Universal
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
    public float remoteExplodeFireDelay;
}
