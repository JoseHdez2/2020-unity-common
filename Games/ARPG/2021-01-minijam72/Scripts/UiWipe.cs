

using System;
using UnityEngine;
using UnityEngine.UI;

public class UiWipe : MonoBehaviour, IToggleable
{
    [SerializeField]
    float wipeSpeed = 0.01f;
    private float signedWipeSpeed; // wipeSpeed but negative if we're decreasing.

    private Image image;

    private float wipeCur, wipeTarget;

    public bool IsDone() => wipeCur == wipeTarget;

    private void Awake(){
        image = GetComponentInChildren<Image>();
    }

    public void Toggle(bool fillScreen){
        // if(image != null){
        //     image.raycastTarget = true;
        // }
        WipeToN(fillScreen ? 1 : 0);
    }

    private void Update(){
        wipeCur = image.fillAmount;
        if(!IsDone()){
            if(Math.Abs(wipeTarget - wipeCur) < wipeSpeed){
                wipeCur = wipeTarget;
            } else {
                wipeCur += signedWipeSpeed;
            }
            image.fillAmount = wipeCur;
        }
    }

    public void WipeToN(float n){
        wipeTarget = n;
        signedWipeSpeed = (wipeTarget < wipeCur) ? -wipeSpeed : wipeSpeed;
    }

}