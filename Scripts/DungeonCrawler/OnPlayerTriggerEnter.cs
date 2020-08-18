using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class OnPlayerTriggerEnter : MonoBehaviour
{
    public UnityEvent uEvent;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            uEvent.Invoke();
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            uEvent.Invoke();
        }
    }
}
