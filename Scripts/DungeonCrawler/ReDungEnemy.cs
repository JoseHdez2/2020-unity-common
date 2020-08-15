using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReDungEnemy : MonoBehaviour
{
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<DungeonCrawlerPlayer>().Die();
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<DungeonCrawlerPlayer>().Die();
        }
    }
}
