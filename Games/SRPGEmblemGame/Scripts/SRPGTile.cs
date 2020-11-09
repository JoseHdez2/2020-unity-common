using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRPGTile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Vector3 animationSizeDestination = Vector3.one;
    [Range(0,1)]
    public float animationSpeed = 0.1f;
    // Start is called before the first frame update
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if(spriteRenderer.size.x != animationSizeDestination.x){
            spriteRenderer.size = Vector3.Lerp(spriteRenderer.size, animationSizeDestination, animationSpeed);
        }
    }

    private void OnEnable() {
        spriteRenderer.size = Vector3.zero;
    }

    public void SelfDestroy() {
        Debug.Log("SelfDestroy");
        Destroy(this.gameObject, 1);
        animationSizeDestination = Vector3.zero;
    }
}
