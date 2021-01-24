using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public class ArpgWeaponThrust : LerpMovement {
    [SerializeField] ArpgPlayerBehavior behavior;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip thrustSound;
    [SerializeField] Collider2D wpnColl;
    [SerializeField] List<KeyCode> keyCodes;
    public bool canAttack = true;

    public static Dictionary<EDirection, Vector3> dirsToVects =
        new Dictionary<EDirection, Vector3>() {
            {EDirection.UP, Vector3.up},
            {EDirection.DOWN, Vector3.down},
            {EDirection.LEFT, Vector3.left},
            {EDirection.RIGHT, Vector3.right},
        };

    private void Start() {
        wpnColl.enabled = false;
    }

    new void Update() {
        base.Update();
        if(canAttack && keyCodes.Any(kc => Input.GetKeyDown(kc))){
            StartCoroutine(CrAttack());
        }
    }

    public IEnumerator CrAttack(){
        canAttack = false;
        behavior.canMove = false;
        wpnColl.enabled = true;
        audioSource.Play(thrustSound);
        destinationPos = behavior.transform.position + dirsToVects[behavior.GetFacingDirection()];
        yield return new WaitUntil(() => IsDone());
        wpnColl.enabled = false;
        destinationPos = behavior.transform.position;
        yield return new WaitUntil(() => IsDone());
        behavior.canMove = true;
        canAttack = true;
    }
}
