using UnityEngine;
using System.Collections;

public class PlayerShooterBullet : EntityDamager
{
    public float bulletSpeed = 20f;
    public int ttl = 2;

    private void Start()
    {
        rb2d.velocity = transform.right * bulletSpeed;
        Destroy(gameObject, ttl);
    }
}
