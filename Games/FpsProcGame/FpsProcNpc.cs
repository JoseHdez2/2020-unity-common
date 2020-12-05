using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsProcNpc : MonoBehaviour
{
    public static List<string> unisexNames = new List<string>{"Addison", "Adrian", "Alex", "Arden", "Aubrey", "August", "Bailey"};
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = $"{unisexNames[3]} Smith";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
