using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHanging : MonoBehaviour
{
    [Range(0,359)]
    public float initialDegrees = 0;
    [Range(0,360)]
    public float rangeOfDegrees = 90;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, initialDegrees + Mathf.Sin(Time.time) * rangeOfDegrees));
    }
}
