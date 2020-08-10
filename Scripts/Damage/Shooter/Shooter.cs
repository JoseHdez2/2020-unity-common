using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System;

[RequireComponent(typeof(AudioSourceShooter))]
public abstract class Shooter : Pausable
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public bool debugFirePoint;

    public float intervalOfAttack = 2f;
    private float timeToNextAttack;

    private AudioSourceShooter audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSourceShooter>();
        timeToNextAttack = intervalOfAttack;
    }

    protected void Shoot(){
        if (CanShoot())
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            audioSource.PlaySound(EShooterSound.SHOOT);
            timeToNextAttack = intervalOfAttack;
        } else {
            audioSource.PlaySound(EShooterSound.CANT_SHOOT);
        }
    }

    public bool CanShoot() => (timeToNextAttack <= 0);

    protected override void Update2() {
        if (timeToNextAttack > 0) { timeToNextAttack -= Time.deltaTime; }
        if (debugFirePoint) DebugDraw.DrawOrientation(firePoint, Color.red);
    }

    protected override void FixedUpdate2() { return; }

    protected override void OnPause(bool isPaused) { return; }
}
