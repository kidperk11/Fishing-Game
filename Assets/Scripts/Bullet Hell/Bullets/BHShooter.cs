using System;
using UnityEngine;


/// <summary>
///     Represents a single rudder. If rudder has a floating object component it will also be used for steering and not be
///     visual-only.
/// </summary>
[Serializable]
public class BHShooter
{
    [SerializeField] public BHShooterScriptableObject shooterScriptableObject;

    public string name = "Main Bullet";

    public void PrepareBullet(GameObject bullet, float playerX, float playerZ, int bulletNumber)
    {
        float shootHor = 0;
        float shootVert = 0;
        float bulletSpeed = shooterScriptableObject.bulletSpeed;
        float bulletSize = shooterScriptableObject.bulletSize;
        float bulletDamage = shooterScriptableObject.bulletDamage;

        if (playerX < -.71 && playerZ < .71)
            shootHor = -1;
        if (playerX > .71 && playerZ < .71)
            shootHor = 1;
        if (playerX > -.71 && playerZ > .71)
            shootVert = 1;
        if (playerX < .71 && playerZ < -.71)
            shootVert = -1;

        if (shooterScriptableObject.lockOn)
        {
            Shoot(bullet, playerX, playerZ, bulletSpeed, bulletSize, bulletDamage);
        }

        else if (shooterScriptableObject.multiDirection)
        {
            switch (bulletNumber)
            {
                case (0):
                    Shoot(bullet, 0, 1, bulletSpeed, bulletSize, bulletDamage);
                    break;
                case (1):
                    Shoot(bullet, 1, 0, bulletSpeed, bulletSize, bulletDamage);
                    break;
                case (2):
                    Shoot(bullet, 0, -1, bulletSpeed, bulletSize, bulletDamage);
                    break;
                case (3):
                    Shoot(bullet, -1, 0, bulletSpeed, bulletSize, bulletDamage);
                    break;
            }
        }
        else
        {
            Shoot(bullet, shootHor, shootVert, bulletSpeed, bulletSize, bulletDamage);
        }
    }

    private void Shoot(GameObject bullet, float xDir, float zDir, float bulletSpeed, float bulletSize, float bulletDamage)
    {

        //bullet.GetComponent<Fireball>().Shoot(xDir, zDir, bulletSpeed, bulletSize, bulletDamage);
    }
}
