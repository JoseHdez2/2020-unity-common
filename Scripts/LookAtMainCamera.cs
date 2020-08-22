using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{
    private Camera mainCam;
    
    void Start()
    {
        // TODO ensure 'MainCamera' tag
        mainCam = FindObjectOfType<Camera>();
    }
    
    void Update()
    {
        transform.LookAt(mainCam.transform);
    }
}
