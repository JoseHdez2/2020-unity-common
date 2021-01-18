using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIfAppPlatform : MonoBehaviour
{
    public List<RuntimePlatform> include;
    // Start is called before the first frame update
    void Start() {
        if(!include.Contains(Application.platform)){
            Destroy(this.gameObject);
        }
        Debug.Log(Application.platform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
