using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteKeyframe {
    public Vector3 scale;
    public float alpha;
    public Quaternion rotation;
}

// A sprite that pops in when enabled, and pops out when destroyed/disabled.
[RequireComponent(typeof(SpriteRenderer))]
public class SpritePopInOut : MonoBehaviour {
    [Tooltip("Optional collider to disable when animating and popping out.")]
    [SerializeField] Collider2D coll;
    private SpriteRenderer spriteRenderer;

    private Vector3 popInSize;
    private float popInAlpha;
    private Quaternion popInRotation;

    public Vector3 popOutSize = new Vector3(0.01f, 0.01f, 0.01f); // TODO setting this to zero results in NaN assignment.
    public float popOutAlpha = 0f;
    public Vector3 popOutRotationConfig;
    private Quaternion popOutRotation;

    private Vector3 targetSize;
    private float targetAlpha;
    private Quaternion targetRotation;
    SpriteKeyframe target, popIn, popOut = new SpriteKeyframe(){scale=new Vector3(0.01f, 0.01f, 0.01f), alpha=0f}; // TODO use this.

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

    private void OnEnable() { // TODO change to Start.
        if(spriteRenderer){  
            spriteRenderer.size = popOutSize;
            PopIn();
        }
    }

    public void SelfEnable() {
        this.gameObject.SetActive(true);
        PopIn();
    }

    // Update is called once per frame
    void Update() {
        UpdateAnimation();
    }

    private void UpdateAnimation() {
        if (spriteRenderer == null) {
            return;
        }
        if (!IsSizeAnimationFinished()) {
            Vector3 scale = Vector3.Lerp(spriteRenderer.size, targetSize, animationSpeed * Time.deltaTime);
            spriteRenderer.size = new Vector3(Norm(scale.x), Norm(scale.y), Norm(scale.z)); // TODO maybe overkill
            if (IsSizeAnimationFinished()) {
                spriteRenderer.size = targetSize;
            }
        }
        if (!IsAlphaAnimationFinished()) {
            float alpha = Mathf.Lerp(spriteRenderer.color.a, targetAlpha, animationSpeed * Time.deltaTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            if (IsAlphaAnimationFinished()) {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);
            }
        }
        if (!IsRotationAnimationFinished()) {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, animationSpeed * Time.deltaTime);
            if (IsRotationAnimationFinished()) {
                transform.rotation = targetRotation;
            }
        }
    }

    private float Norm(float f){
        return float.IsNaN(f) ? 0f : f;
    }

    public void PopIn(){
        StartCoroutine(CrPopIn());
    }

    public void SelfHide() {
        StartCoroutine(CrPopOut(PopOutMode.Disable));
    }

    public void SelfDisable() {
        StartCoroutine(CrPopOut(PopOutMode.Disable));
    }

    public void SelfDestroy() {
        StartCoroutine(CrPopOut(PopOutMode.Destroy));
    }

    private IEnumerator CrPopIn(){
        targetSize = popInSize;
        targetAlpha = popInAlpha;
        targetRotation = popInRotation;
        yield return new WaitUntil(() => IsAnimationFinished());
        if(coll != null){
            coll.enabled = true;
        }
        yield return null;
    }

    private enum PopOutMode { Hide, Disable, Destroy };

    private IEnumerator CrPopOut(PopOutMode mode){
        if(coll != null){
            coll.enabled = false;
        }
        targetSize = popOutSize;
        targetAlpha = popOutAlpha;
        targetRotation = popOutRotation;
        yield return new WaitUntil(() => IsAnimationFinished());
        switch(mode){
            case PopOutMode.Hide: break;
            case PopOutMode.Disable:
                this.gameObject.SetActive(false); break;
            case PopOutMode.Destroy:
                Destroy(this.gameObject); break;
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

    public bool IsAnimationFinished(){
        if(spriteRenderer == null){
            return false;
        }
        return IsSizeAnimationFinished() && IsAlphaAnimationFinished() && IsRotationAnimationFinished();
    }
}