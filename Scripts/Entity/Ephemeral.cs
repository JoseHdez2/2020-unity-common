using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Make Pausable.
public class Ephemeral : MonoBehaviour
{
    [Tooltip("Time To Live")]
    public int ttl;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, ttl);
    }
}
