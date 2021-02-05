using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;


public enum EDamageableEffect {
    BLINKING,
    INVINCIBLE
}

[RequireComponent(typeof(BoxCollider2D))] // TODO this wouldnt work with just Collider2D I think...
public class EntityDamageable : SpritePopInOut
{
    [Header("EntityDamageable")]
    public AudioClip soundDamage;
    public AudioClip soundHeal, soundDie;
    [SerializeField] DamagePopup pfDamagePopup;
    [SerializeField] GameObject spawnOnDeath;
    public ETeam team; // TODO move up into children.
    public int health, maxHealth;

    public Color colorDamage = Color.red, colorHeal = Color.green, colorExpandHealth = Color.yellow;

    [Header("EntityDamageable Sprites")]
    protected Sprite idleSprite;
    public Sprite hitSprite, deathSprite;

    private Dictionary<EDamageableEffect, Color> currentEffects = new Dictionary<EDamageableEffect, Color>();

    private SpriteRenderer spriteRenderer;
    private ObjectShake objectShake;
    // private AbstractMovement movement;
    private Color colorReal;
    [SerializeField] private AudioSource audioSource;
    public void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        objectShake = GetComponentInChildren<ObjectShake>();
        if(spriteRenderer){
            idleSprite = spriteRenderer.sprite;
            colorReal = spriteRenderer.color;
        }
        if(objectShake == null){
            Debug.LogWarning("No ObjectShake component found!");
        }
        // movement = GetComponent<AbstractMovement>();
        if(audioSource == null){
            audioSource = GetComponentInChildren<AudioSource>();
        }
        if(audioSource == null){
            audioSource = FindObjectOfType<AudioSource>();
        }
    }

    public virtual void Start()
    {
        health = maxHealth;
    }

    public virtual void Heal(int healQty)
    {
        health += healQty;
        if (health > maxHealth) { health = maxHealth; }
        if (soundHeal) { audioSource.clip = soundHeal; audioSource.Play(); }
        if (pfDamagePopup){ CreatePopup(healQty, colorHeal); }
        StartCoroutine(Blink(colorHeal));
    }

    private void CreatePopup(int qty, Color color)
    {
        DamagePopup dp = Instantiate(pfDamagePopup, transform.position, Quaternion.identity);
        dp.transform.SetParent(transform.parent);
        dp.SetPopupText(qty.ToString());
        dp.SetPopupColor(color);
    }

    internal void ExpandMaxHealth(int qty)
    {
        maxHealth += qty;
        if (soundHeal) { audioSource.clip = soundHeal; audioSource.Play(); }
        if (pfDamagePopup) { CreatePopup(qty, colorExpandHealth); }
        StartCoroutine(Blink(colorExpandHealth));
    }

    public virtual void Damage(Damage damage)
    {
        health -= damage.amount;
        StartCoroutine(CrDamage());
        if (pfDamagePopup) { CreatePopup(damage.amount, colorDamage); }
        if (health <= 0) { 
            Die(); 
        } else {
            Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            rb2d.velocity += damage.pushVector * damage.pushSpeed;
            // if (movement != null) { movement.SetMovement(EMovementType.BULLET_PUSH, damage.pushVector * damage.pushSpeed); } // TODO reduce pushSpeed with eg. armor
            if (soundDamage) { audioSource.clip = soundDamage; audioSource.Play(); }
            StartCoroutine(Blink(colorDamage));
            if(objectShake){
                objectShake.Shake();
            }
            StartCoroutine(CrDamage());
        }
    }

    protected IEnumerator CrDamage(){
        if(spriteRenderer && deathSprite){
            spriteRenderer.sprite = hitSprite;
        }
        yield return new WaitForSeconds(1f);
        if(spriteRenderer && idleSprite){
            spriteRenderer.sprite = idleSprite;
        }
    }

    public virtual void Die(){
        if (soundDie != null) { audioSource.clip = soundDie; audioSource.Play(); }
        if (spawnOnDeath) { Instantiate(spawnOnDeath, gameObject.transform.position, gameObject.transform.rotation); }
        GetComponents<Collider2D>().ToList().ForEach(coll => Destroy(coll));
        StartCoroutine(CrDie());
    }

    private IEnumerator CrDie(){
        if(deathSprite){
            spriteRenderer.sprite = deathSprite;
        }
        while(spriteRenderer.color.a > 0){
            float alpha = spriteRenderer.color.a - 0.1f;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return new WaitForSeconds(0.1f);
        }
        GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(sr => sr.enabled = false);
        GetComponentsInChildren<Collider2D>().ToList().ForEach(sr => sr.enabled = false);
        if(spawnOnDeath){

        }
        Destroy(this.gameObject, 2);
    }

    IEnumerator Blink(Color blinkColor, float blinkSeconds = 0.2f)
    {
        AddEffect(EDamageableEffect.BLINKING, blinkColor);
        yield return new WaitForSeconds(blinkSeconds);
        RemoveEffect(EDamageableEffect.BLINKING);
    }

    protected void RemoveEffect(EDamageableEffect effectKey)
    {
        currentEffects.Remove(effectKey);
        UpdateEffects();
    }

    protected void AddEffect(EDamageableEffect effectKey, Color effectColor)
    {
        currentEffects.Add(effectKey, effectColor);
        UpdateEffects();
    }

    private void UpdateEffects()
    {
        spriteRenderer.color = currentEffects.Values.Aggregate(colorReal, (a, b) => (a + b) / 2);
    }
}

