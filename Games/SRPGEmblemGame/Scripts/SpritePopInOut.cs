using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// A sprite that pops in when enabled, and pops out when destroyed/disabled.
[RequireComponent(typeof(SpriteRenderer))]
public class SpritePopInOut : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    private Vector3 popInSize;
    private float popInAlpha;
    private Quaternion popInRotation;

    public Vector3 popOutSize = Vector3.zero;
    public float popOutAlpha = 0f;
    public Vector3 popOutRotationConfig;
    private Quaternion popOutRotation;

    private Vector3 targetSize;
    private float targetAlpha;
    private Quaternion targetRotation;

    public float animationSpeed = 1f;
    // Start is called before the first frame update
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        popInAlpha = spriteRenderer.color.a;
        popInSize = spriteRenderer.size;
        popInRotation = this.transform.rotation;
        popOutRotation = Quaternion.Euler(popOutRotationConfig);
        transform.rotation = popOutRotation;
        if(spriteRenderer.drawMode != SpriteDrawMode.Sliced){
            Debug.LogWarning("SpriteRenderer not set to Sliced! Setting automatically.");
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        }
    }

    // Update is called once per frame
    void Update() {
        if(!IsSizeAnimationFinished()){
            spriteRenderer.size = Vector3.Lerp(spriteRenderer.size, targetSize, animationSpeed * Time.deltaTime);
            if(IsSizeAnimationFinished()){
                spriteRenderer.size = targetSize;
            }
        }
        if(!IsAlphaAnimationFinished()){
            float alpha = Mathf.Lerp(spriteRenderer.color.a, targetAlpha, animationSpeed * Time.deltaTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            if(IsAlphaAnimationFinished()){
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);
            }
        }
        if(!IsRotationAnimationFinished()){
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, animationSpeed * Time.deltaTime);
            if(IsRotationAnimationFinished()){
                transform.rotation = targetRotation;
            }
        }
    }

    private void OnEnable() {
        spriteRenderer.size = popOutSize;
        StartCoroutine(CrPopIn());
    }


    public void SelfDisable() {
        StartCoroutine(CrPopOut(destroy: false));
    }

    public void SelfDestroy() {
        StartCoroutine(CrPopOut(destroy: true));
    }

    private IEnumerator CrPopIn(){
        targetSize = popInSize;
        targetAlpha = popInAlpha;
        targetRotation = popInRotation;
        yield return new WaitUntil(() => IsAnimationFinished());
        GetComponent<Collider2D>().enabled = true;
        yield return null;
    }

    private IEnumerator CrPopOut(bool destroy){
        Collider2D coll = GetComponent<Collider2D>();
        if(coll != null){
            coll.enabled = false;
        }
        targetSize = popOutSize;
        targetAlpha = popOutAlpha;
        targetRotation = popOutRotation;
        yield return new WaitUntil(() => IsAnimationFinished());
        if(destroy){
            Destroy(this.gameObject);
        } else {
            this.gameObject.SetActive(false);
        }
    }

    private bool IsSizeAnimationFinished(){
        return Math.Abs(spriteRenderer.size.x - targetSize.x) < 0.05;
    }

    private bool IsAlphaAnimationFinished(){
        return Math.Abs(spriteRenderer.color.a - targetAlpha) < 0.05;
    }

    private bool IsRotationAnimationFinished(){
        // return true;
        return Quaternion.Angle(transform.rotation, targetRotation) < 0.5;
    }

    private bool IsAnimationFinished(){
        return IsSizeAnimationFinished() && IsAlphaAnimationFinished() && IsRotationAnimationFinished();
    }
}