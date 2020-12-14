using UnityEngine;
using System;

[System.Serializable]
public class VfxNinjaVision : MonoBehaviour {
    [SerializeField] LerpMovement obj;
    [SerializeField] GameObject stageLeft, stageCenter, stageRight;
    [SerializeField] ImageWipe ninjaVisionTop, ninjaVisionBottom;
    public bool leftToRight = true;

    // TODO implement "autoInitializeGameObjects" bool, which creates all of the stage and ninjaVision gameObjects on the fly.
    private void Start() {
        GetReady();
    }

    public void GetReady(){
        if(obj){
            obj.transform.position = leftToRight ? stageLeft.transform.position : stageRight.transform.position;
        }
    }

    public void Activate(){
        if(obj){
            obj.destinationPos = stageCenter.transform.position;
        }
        ninjaVisionTop.Toggle(true);
        ninjaVisionBottom.Toggle(true);
    }

    public void Deactivate(){
        if(obj){
            obj.destinationPos = leftToRight ? stageRight.transform.position : stageLeft.transform.position;
        }
        ninjaVisionTop.Toggle(false);
        ninjaVisionBottom.Toggle(false);
    }
    
}