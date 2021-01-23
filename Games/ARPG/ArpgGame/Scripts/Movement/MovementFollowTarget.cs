using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MovementFollowTarget: AbstractMovement
{
    public Transform target;
    public float minDistance = 0.1f;
    // public float t = 0.2f;
    public float speed = 1f;
    
    private Animator animator;
    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 pos = gameObject.transform.position;
        if (Vector2.Distance(pos, target.position) > minDistance)
        {
            float x = target.position.x - pos.x;
            float y = target.position.y - pos.y;
            SetMovement(EMovementType.FOLLOW_TARGET, new Vector2(x, y).normalized * speed);
            if(animator != null)
            {
                animator.SetFloat("xAxis", x);
                animator.SetFloat("yAxis", y);
            }
            // gameObject.transform.position = Vector2.Lerp(pos, target.position, t);
        }
    }
}

public enum EMovementType
{
    PLAYER,
    FOLLOW_TARGET,
    BULLET_PUSH // TODO will only be pushed by latest bullet
}

// FIXME maybe delete this?
public class AbstractMovement : MonoBehaviour {

    [Range(0, 1)]
    public float decelerationCoef = 0.1f;
    public float negligibleVelocityCutoff = 0.1f;
    private MyDictionary<EMovementType, Vector2> curMovements = new MyDictionary<EMovementType, Vector2>();

    private Rigidbody2D rb2d;
    public virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void RemoveMovement(EMovementType movementKey)
    {
        curMovements.Remove(movementKey);
    }

    public void SetMovement(EMovementType movementKey, Vector2 movementVelocity)
    {
        curMovements.AddItem(movementKey, movementVelocity);
    }

    public virtual void FixedUpdate()
    {
        foreach(EMovementType key in curMovements.Keys.ToList())
        {
            if (curMovements[key].magnitude <= negligibleVelocityCutoff)
            {
                RemoveMovement(key);
            } else {
                SetMovement(key, curMovements[key] * (1 - decelerationCoef));
            }
        }
        rb2d.velocity = curMovements.Values.Aggregate(Vector2.zero, (a, b) => (a + b) / 2);
    }
}