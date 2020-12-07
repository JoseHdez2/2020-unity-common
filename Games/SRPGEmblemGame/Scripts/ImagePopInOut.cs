// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using UnityEngine.UI;

// // An image that pops in when enabled, and pops out when destroyed/disabled.
// public class ImagePopInOut : MonoBehaviour {
//     private RectTransform rectTransform;
//     private Image image;

//     private Vector3 popInSize;
//     private float popInAlpha;
//     private Quaternion popInRotation;

//     public Vector3 popOutSize = new Vector3(0.01f, 0.01f, 0.01f); // TODO setting this to zero results in NaN assignment.
//     public float popOutAlpha = 0f;
//     public Vector3 popOutRotationConfig;
//     private Quaternion popOutRotation;

//     private Vector3 targetSize;
//     private float targetAlpha;
//     private Quaternion targetRotation;

//     public float animationSpeed = 1f;
//     // Start is called before the first frame update
//     void Awake() {
//         popInAlpha = image.color.a;
//         popInSize = rectTransform.localScale;
//         popInRotation = this.transform.rotation;
//         popOutRotation = Quaternion.Euler(popOutRotationConfig);
//         transform.rotation = popOutRotation;
//     }

//     private void OnEnable() { // TODO change to Start.
//         if(rectTransform){  
//             rectTransform.sizeDelta = popOutSize;
//             PopIn();
//         }
//     }

//     public void SelfEnable() {
//         this.gameObject.SetActive(true);
//         PopIn();
//     }

//     // Update is called once per frame
//     void Update() {
//         if(!IsSizeAnimationFinished()){
//             Vector3 scale = Vector3.Lerp(rectTransform.sizeDelta, targetSize, animationSpeed * Time.deltaTime);
//             spriteRenderer.size = new Vector3(Norm(scale.x), Norm(scale.y), Norm(scale.z)); // TODO maybe overkill
//             if(IsSizeAnimationFinished()){
//                 spriteRenderer.size = targetSize;
//             }
//         }
//         if(!IsAlphaAnimationFinished()){
//             float alpha = Mathf.Lerp(spriteRenderer.color.a, targetAlpha, animationSpeed * Time.deltaTime);
//             spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
//             if(IsAlphaAnimationFinished()){
//                 spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);
//             }
//         }
//         if(!IsRotationAnimationFinished()){
//             transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, animationSpeed * Time.deltaTime);
//             if(IsRotationAnimationFinished()){
//                 transform.rotation = targetRotation;
//             }
//         }
//     }

//     private float Norm(float f){
//         return float.IsNaN(f) ? 0f : f;
//     }

//     public void PopIn(){
//         StartCoroutine(CrPopIn());
//     }

//     public void SelfHide() {
//         StartCoroutine(CrPopOut(PopOutMode.Disable));
//     }

//     public void SelfDisable() {
//         StartCoroutine(CrPopOut(PopOutMode.Disable));
//     }

//     public void SelfDestroy() {
//         StartCoroutine(CrPopOut(PopOutMode.Destroy));
//     }

//     private IEnumerator CrPopIn(){
//         targetSize = popInSize;
//         targetAlpha = popInAlpha;
//         targetRotation = popInRotation;
//         yield return new WaitUntil(() => IsAnimationFinished());
//         GetComponent<Collider2D>().enabled = true;
//         yield return null;
//     }

//     private enum PopOutMode { Hide, Disable, Destroy };

//     private IEnumerator CrPopOut(PopOutMode mode){
//         Collider2D coll = GetComponent<Collider2D>();
//         if(coll != null){
//             coll.enabled = false;
//         }
//         targetSize = popOutSize;
//         targetAlpha = popOutAlpha;
//         targetRotation = popOutRotation;
//         yield return new WaitUntil(() => IsAnimationFinished());
//         switch(mode){
//             case PopOutMode.Hide: break;
//             case PopOutMode.Disable:
//                 this.gameObject.SetActive(false); break;
//             case PopOutMode.Destroy:
//                 Destroy(this.gameObject); break;
//         }
//     }

//     private bool IsSizeAnimationFinished(){
//         return Math.Abs(spriteRenderer.size.x - targetSize.x) < 0.05;
//     }

//     private bool IsAlphaAnimationFinished(){
//         return Math.Abs(spriteRenderer.color.a - targetAlpha) < 0.05;
//     }

//     private bool IsRotationAnimationFinished(){
//         // return true;
//         return Quaternion.Angle(transform.rotation, targetRotation) < 0.5;
//     }

//     public bool IsAnimationFinished(){
//         return IsSizeAnimationFinished() && IsAlphaAnimationFinished() && IsRotationAnimationFinished();
//     }
// }