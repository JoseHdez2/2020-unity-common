using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO require component Collider?
public class CreateInRandomPositions : MonoBehaviour
{
    public GameObject prefab;
    public int quantity;
    
    void Start()
    {
        Collider coll = GetComponent<Collider>();
        for(int i = 0; i < quantity; i++) {
            Instantiate(prefab, coll.GetRandomPoint(), Quaternion.identity, transform);
        }
    }
}
