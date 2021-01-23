using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEphemeral : MonoBehaviour
{
    [Tooltip("Time To Live")]
    public int ttl;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, ttl);
    }
}
