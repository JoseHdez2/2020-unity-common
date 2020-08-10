using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using ExtensionMethods;

[RequireComponent(typeof(CircleCollider2D))]
public class ShooterAuto : Shooter
{
    [Tooltip("Will target any entities with this Tag.")]
    public string targetTag;

    private CircleCollider2D range;

    public float turnSpeed = 1;

    new public void Start()
    {
        base.Start();
        range = GetComponent<CircleCollider2D>();
    }

    protected override void Update2()
    {
        base.Update2();
        List<GameObject> targets = TargetsInRange();
        if(targets.Count > 0)
        {
            transform.TurnTowardsTarget(targets.First().transform, turnSpeed);
            if (CanShoot()){ Shoot(); }
        }
    }

    private List<GameObject> TargetsInRange() => FindObjectsOfType<EntityDamageable>()
                    .ToList() // TODO converting to list twice. also this method is ineff...
                        // maybe use OverlapCollider instead.
                    .Where(d => d.gameObject && d.gameObject.CompareTag(targetTag))
                    .Select(d => d.gameObject.GetComponent<Collider2D>())
                    .Where(c => c != null && c.IsTouching(range))
                    .Select(c => c.gameObject).ToList();
}
