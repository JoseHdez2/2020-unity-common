using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool selected = false;
    Vector2 offMax;
    Vector2 offMin;
    RectTransform myRT;
    void Start()
    {
        myRT = transform as RectTransform;
        offMax = myRT.offsetMax;
        offMin = myRT.offsetMin;
    }

    public void OnSelect(BaseEventData data)
    {
        myRT.offsetMax = offMax * 1.1f;
        myRT.offsetMin = offMin * 1.1f;
        selected = true;
    }
    public void OnDeselect(BaseEventData data)
    {
        myRT.offsetMax = offMax;
        myRT.offsetMin = offMin;
        selected = false;
    }
}
