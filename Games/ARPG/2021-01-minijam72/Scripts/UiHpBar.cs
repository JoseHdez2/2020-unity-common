using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHpBar : MonoBehaviour
{
    public EntityDamageable damageable;
    public UiWipe wipeBar, wipeSecondary;
    public float secondaryLagSecs = 1f;

    private void Update() {
        UpdateHealth(damageable.health); // FIXME bad performance.
    }

    public void UpdateHealth(int health){
        StartCoroutine(CrUpdateHealth(health));
    }

    public IEnumerator CrUpdateHealth(int health){
        float n = (float)damageable.health / (float)damageable.maxHealth;
        Debug.Log(n);
        wipeBar.WipeToN(n);
        if(wipeSecondary){
            yield return new WaitForSeconds(secondaryLagSecs);
            wipeSecondary.WipeToN(n);
        }
    }

}
