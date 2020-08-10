using UnityEngine;
using System.Collections;

public enum ETeam
{
    PLAYER,
    ENEMY
}

public enum EDamagerType
{
    MONSTER, // default
    BULLET_WEAK,
    BULLET_PENETRATING // dont die after hitting one enemy
}

public class Damage
{
    public int damage;
    // TODO color;
    public Vector2 pushVector;
    public float pushSpeed;

    public Damage(int damage, Vector2 pushVector, float pushSpeed){
        this.damage = damage;
        this.pushVector = pushVector;
        this.pushSpeed = pushSpeed;
    }
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))] // TODO this wouldnt work with just Collider2D I think...
public class EntityDamager : Expirable
{
    public ETeam team;
    public EDamagerType damagerType;
    public int damage = 1;
    public float speed = 1;

    protected Rigidbody2D rb2d;
    public virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (IsBullet()) {
            rb2d.velocity = transform.right * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EntityDamageable ed = collision.gameObject.GetComponent<EntityDamageable>();
        if (ed && ed.team != team)
        {
            Vector2 pushVector = rb2d.velocity.normalized;
            ed.Damage(new Damage(this.damage, pushVector, this.speed));
            if(IsWeakBullet()){
                Expire();
            }
        }
    }

    public bool IsWeakBullet() => (damagerType == EDamagerType.BULLET_WEAK);
    public bool IsBullet() => (IsWeakBullet() || damagerType == EDamagerType.BULLET_PENETRATING);

}
