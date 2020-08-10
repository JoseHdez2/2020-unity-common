using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class TurnTowardsTarget2D : MonoBehaviour
{
    public float turnSpeed = 5;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.TurnTowardsTarget(target, turnSpeed);
    }
}
