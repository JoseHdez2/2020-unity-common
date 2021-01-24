using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirection
{
    NONE, UP, DOWN, LEFT, RIGHT
}


public class ArpgPlayerBehavior : AbstractMovement
{
    public float runSpeed = 10f;
    public bool canMove = true;
    private bool isMoving;
    private EDirection dirFacing; // does not affect movement, only shooting and sprite anim.

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void FixedUpdate() {
        base.FixedUpdate();
        float x = Input.GetAxis("Horizontal") * runSpeed;
        float y = Input.GetAxis("Vertical") * runSpeed;

        isMoving = (x != 0 || y != 0);
        if (canMove && isMoving) {
            SetMovement(EMovementType.PLAYER, new Vector2(x, y));
            dirFacing = calculateFacingDirection(x, y);
        } else {
            RemoveMovement(EMovementType.PLAYER);
        }

        if(animator){
            animator.SetFloat("xAxis", x);
            animator.SetFloat("yAxis", y);
            animator.SetBool("notMoving", !isMoving);
        }
    }

    public EDirection calculateFacingDirection(float x, float y){
        if (Mathf.Abs(x) > Mathf.Abs(y)) {
            return x > 0 ? EDirection.RIGHT : EDirection.LEFT;
        } else {
            return y > 0 ? EDirection.UP : EDirection.DOWN;
        }
    }
    
    public bool IsMoving() => isMoving;

    public EDirection GetFacingDirection() => dirFacing;
}
