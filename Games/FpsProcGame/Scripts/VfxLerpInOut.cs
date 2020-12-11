using UnityEngine;
using System;

[System.Serializable]
public class VfxLerpInOut : MonoBehaviour {
    [SerializeField] LerpMovement obj;
    [SerializeField] GameObject stageIn, stageOut;

    // TODO implement "autoInitializeGameObjects" bool, which creates all of the stage and ninjaVision gameObjects on the fly.

    private void Awake(){
        stageIn = new GameObject("stageIn");
        stageIn.transform.position = transform.position;
        stageIn.transform.parent = transform.parent;
    }

    private void Start() {
        GetReady();
    }

    public void GetReady(){
        if(obj){
            obj.transform.position = stageOut.transform.position;
        }
    }

    public void Toggle(bool enable){
        if(obj){
            obj.destinationPos = enable ? stageIn.transform.position : stageOut.transform.position;
        }
    }
    
}