using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PlayerDamageable : EntityDamageable
{
    [Tooltip("Seconds after hit where player is invulnerable.")]
    public float iFramePeriod = 2f;
    public TMP_Text healthText;

    private Color colorInv = new Color(1, 1, 1, 0.5f);
    private bool invulnerable = false;
    public override void Start()
    {
        base.Start();
        UpdateHealthText();
    }

    public override void Damage(Damage damage)
    {
        if (!invulnerable)
        {
            base.Damage(damage);
            StartCoroutine(SetInvulnerable());
        }
        UpdateHealthText();
    }

    private IEnumerator SetInvulnerable()
    {
        invulnerable = true;

        AddEffect(EDamageableEffect.INVINCIBLE, colorInv);
        yield return new WaitForSeconds(iFramePeriod);
        RemoveEffect(EDamageableEffect.INVINCIBLE);
        
        invulnerable = false;
    }
    

    public override void Heal(int healQty)
    {
        base.Heal(healQty);
        UpdateHealthText();
    }

    public override void Die()
    {
        // base.Die(); DO NOT call base Die method, we don't want to destroy the player.

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void UpdateHealthText() {
        if(healthText){
            healthText.text = $"Health: {health}";
        }
    }
}
