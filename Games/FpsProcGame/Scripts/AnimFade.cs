using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimKeyFrame {
    public float cgAlpha; // Canvas Group alpha.
}

public class AnimFade : MonoBehaviour, IToggleable {
    AnimKeyFrame kfPopIn = new AnimKeyFrame();
    AnimKeyFrame kfPopOut = new AnimKeyFrame();
    AnimKeyFrame kfCur = new AnimKeyFrame();
    AnimKeyFrame kfTarget = new AnimKeyFrame();
    private bool done = false;
    public float animationSpeed = 1f;
    // Start is called before the first frame update
    [SerializeField] CanvasGroup canvasGroup;

    void Awake(){
        if(canvasGroup){
            kfPopOut.cgAlpha = 0f;
            kfPopIn.cgAlpha = 1f;
            kfCur.cgAlpha = canvasGroup.alpha;
        } else {
            Debug.LogError("No CanvasGroup found!");
        }
    }

    void Update(){
        if(canvasGroup){
            if(!IsCgAlphaAnimDone()){
                kfCur.cgAlpha = Mathf.Lerp(kfCur.cgAlpha, kfTarget.cgAlpha, animationSpeed * Time.deltaTime);
                canvasGroup.alpha = kfCur.cgAlpha;
                if(IsCgAlphaAnimDone()){
                    canvasGroup.alpha = kfTarget.cgAlpha;
                }
            }
        }
    }

    public void Toggle(bool show){
        kfTarget = show ? kfPopIn : kfPopOut;
        done = false;
    }

    private bool IsCgAlphaAnimDone(){
        if(done) return true;
        done = Math.Abs(kfCur.cgAlpha - kfTarget.cgAlpha) < 0.05;
        return done;
    }

    public bool IsDone() => IsCgAlphaAnimDone();
}
