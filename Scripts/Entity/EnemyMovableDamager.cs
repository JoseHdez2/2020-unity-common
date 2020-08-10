using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EntityDamageable))]
[RequireComponent(typeof(EntityDamager))]
[RequireComponent(typeof(MovableRigidbody2D))]
public class EnemyMovableDamager : Pausable
{
    private EntityDamageable damageable;
    private EntityDamager damager;
    private MovableRigidbody2D movableRb;

    public void Start()
    {
        damageable = GetComponent<EntityDamageable>();
        damager = GetComponent<EntityDamager>();
        movableRb = GetComponent<MovableRigidbody2D>();
    }

    protected override void FixedUpdate2(){ return; }

    protected override void OnPause(bool isPaused){ return; }

    protected override void Update2(){ return; }
}
