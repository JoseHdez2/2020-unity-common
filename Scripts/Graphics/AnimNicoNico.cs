using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Get a 
public class AnimNicoNico : MonoBehaviour
{
    enum Direction { UpToDown, DownToUp, LeftToRight, RightToLeft };

    [Header("Spawn")]
    [Range(0, 10)]
    public float spawnPeriodRange;

    [Header("Prefab")]
    public GameObject prefab;
    [SerializeField] Direction prefabDirection;
    [Range(1,10)]
    [SerializeField] float prefabSpeedRange;

    [Header("Speed and offset")]
    public float offset = 0f;
    [Range(0, 10)]
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        float time = (offset + Time.time) * speed;
        // transform.rotation = Quaternion.Euler(new Vector3(0, 0, initialDegrees + Mathf.Sin(time) * rangeOfDegrees));
    }

   //  public Vector3 GetRandomPos() {

   //  }
}
