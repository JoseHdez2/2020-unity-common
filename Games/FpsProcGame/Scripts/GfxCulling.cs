using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GfxCulling : MonoBehaviour
{
    [Tooltip("Minimum distance at which culling will take effect.")]
    public float minDistance = 10;
    public float refreshRate = 0.5f;
    public float enrollRate = 5f;
    private Camera mainCam;
    private RepeatingTimer cullTimer, enrollTimer;
    Renderer[] renderers;
    // Start is called before the first frame update
    void Awake()
    {
        cullTimer = new RepeatingTimer(refreshRate);
        mainCam = FindObjectsOfType<Camera>(includeInactive: true).Where(o => o.CompareTag("MainCamera")).First();
        
        enrollTimer = new RepeatingTimer(refreshRate);
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cullTimer.UpdateAndCheck(Time.deltaTime)){
            foreach(Renderer r in renderers){
                r.enabled = r.IsVisibleFrom(mainCam) || Vector3.Distance(transform.position, mainCam.transform.position) < minDistance;
            }
        }
        if(enrollTimer.UpdateAndCheck(Time.deltaTime)){
            renderers = GetComponentsInChildren<Renderer>();
        }
    }
}
