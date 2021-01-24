using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Minijam72Switch : MonoBehaviour
{
    [SerializeField] Sprite spriteUnpressed, spritePressed;
    private SpriteRenderer spriteRenderer;
    public bool isPressed = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        spriteRenderer.sprite = spritePressed;
        isPressed = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        spriteRenderer.sprite = spriteUnpressed;
        isPressed = false;
    }
}
