using UnityEngine;
using System.Collections;
using System;

public class PlayerShooter : AbstractPlayerShooter
{
    public int maxAmmo;
    public int ammo;

    public PlayerShooterBullet bulletPrefab;
    public AudioClip soundShoot;

    // Use this for initialization
    void Start()
    {
        ammo = maxAmmo;
    }

    public override bool HasBulletsLeft()
    {
        return ammo > 0;
    }

    public override void SpendBullet()
    {
        ammo = --ammo;
    }

    public override PlayerShooterBullet GetBulletPrefab()
    {
        return bulletPrefab;
    }

    public override AudioClip GetShotSound()
    {
        return soundShoot;
    }
}

public abstract class AbstractPlayerShooter : MonoBehaviour
{
    public Transform firePoint;
    public bool fourWayShooting = true;

    [Tooltip("Don't spend any ammo.")]
    public bool infiniteAmmo = false;

    // TODO soundNoBulletsLeft
    private AudioSource sound;
    private ArpgPlayerBehavior player;
    protected virtual void Awake()
    {
        sound = GetComponent<AudioSource>();
        player = GetComponent<ArpgPlayerBehavior>();
    }

    public abstract bool HasBulletsLeft();

    public abstract void SpendBullet();

    public abstract PlayerShooterBullet GetBulletPrefab();

    public abstract AudioClip GetShotSound();

    // Update is called once per frame
    void Update()
    {
        // TODO move firePoint
        if (Input.GetButtonDown("Fire1") 
            && (infiniteAmmo || HasBulletsLeft())){
            Shoot();
        }
    }

    void Shoot()
    {
        if (fourWayShooting)
        {
            SetDirection(player.GetFacingDirection());
        }
        sound.clip = GetShotSound(); // TODO this may override other Player sounds.
        sound.Play();
        PlayerShooterBullet bullet = Instantiate(GetBulletPrefab(), firePoint.position, firePoint.rotation);
        SpendBullet();
    }

    public void SetDirection(EDirection dir)
    {
        switch (dir)
        {
            case EDirection.RIGHT: firePoint.rotation = Quaternion.Euler(0, 0, 0); break;
            case EDirection.UP: firePoint.rotation = Quaternion.Euler(0, 0, 90); break;
            case EDirection.LEFT: firePoint.rotation = Quaternion.Euler(0, 0, 180); break;
            case EDirection.DOWN: firePoint.rotation = Quaternion.Euler(0, 0, 270); break;
        }
    }

}

