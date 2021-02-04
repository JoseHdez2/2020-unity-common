using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damager2d : MonoBehaviour
{
    public ETeam team;
    public int damage = 1;
    public float pushSpeed = 1;
    public UnityEvent expireEvent; // null for non-bullets.

    protected Rigidbody2D rb2d;

    private void OnTriggerEnter2D(Collider2D collision) {
        EntityDamageable ed = collision.gameObject.GetComponent<EntityDamageable>();
        if (ed && ed.team != team){
            Vector2 pushVector = collision.gameObject.transform.position - transform.position;
            ed.Damage(new Damage(this.damage, pushVector.normalized, this.pushSpeed));
            if(expireEvent != null){
                expireEvent.Invoke();
            }
        }
    }

}