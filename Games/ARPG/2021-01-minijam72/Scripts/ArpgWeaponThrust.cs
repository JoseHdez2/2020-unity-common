using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArpgWeaponThrust : VfxLerpInOut
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine(CrAttack());
        }
    }

    public IEnumerator CrAttack(){
        Toggle(true);
        yield return new WaitUntil(IsDone);
        Toggle(false);
    }
}
