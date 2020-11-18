using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A sprite that pops in when enabled, and pops out when destroyed.
[RequireComponent(typeof(SpriteRenderer))]
public class SpritePopInOut : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    public Vector3 animationSizeDestination = Vector3.one;

    public float animationSpeed = 1f;
    // Start is called before the first frame update
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer.drawMode != SpriteDrawMode.Sliced){
            Debug.LogWarning("SpriteRenderer not set to Sliced! Setting automatically.");
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        }
    }

    // Update is called once per frame
    void Update() {
        if(spriteRenderer.size.x != animationSizeDestination.x){
            spriteRenderer.size = Vector3.Lerp(spriteRenderer.size, animationSizeDestination, animationSpeed * Time.deltaTime);
        }
    }

    private void OnEnable() {
        spriteRenderer.size = Vector3.zero;
    }

    public void SelfDestroy() {
        Collider2D coll = GetComponent<Collider2D>();
        if(coll != null){
            coll.enabled = false;
        }
        Destroy(this.gameObject, 1);
        animationSizeDestination = Vector3.zero;
    }
}