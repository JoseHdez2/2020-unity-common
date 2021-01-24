using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minijam72Healer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EntityDamageable ed = collision.gameObject.GetComponent<EntityDamageable>();
        if (ed && ed.team == ETeam.PLAYER){
            ed.Heal(ed.maxHealth);
        }
    }
}
