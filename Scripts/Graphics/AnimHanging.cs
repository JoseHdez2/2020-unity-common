using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHanging : MonoBehaviour
{
    [Header("Speed and offset")]
    public float offset = 0f;
    [Range(0, 10)]
    public float speed = 1f;
   
    [Range(0,360)]
    public float rangeOfDegrees = 90;

    private float initialDegrees;

    // Start is called before the first frame update
    void Start()
    {
        initialDegrees = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        float time = (offset + Time.time) * speed;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, initialDegrees + Mathf.Sin(time) * rangeOfDegrees));
    }
}
