using UnityEngine;
using System.Collections;

// Naive implementation that doesn't take collisions into account.
public class Movable : Pausable
{
    [Tooltip("Minimum distance to target before the movement is considered finished.")]
    public float minDistance = 0f;
    [Tooltip("Transform that this entity will always move towards, if far away from it.")]
    private Transform movementTarget;
    [Tooltip("In units per seconds.")]
    public float speed = 1;

    public bool debugTargetPos;

    public Transform MovementTarget { get => movementTarget; set => movementTarget = value; }

    public bool HasArrived(){
        if(MovementTarget == null){ return true; }
        return Vector3.Distance(transform.position, MovementTarget.position) < minDistance;
    }

    protected virtual void MoveToPos(Vector3 newPos){
        transform.position = newPos;
    }

    protected override void OnPause(bool isPaused){
        return;
    }

    protected override void Update2() { }

    protected override void FixedUpdate2()
    {
        if (debugTargetPos){
            DebugDraw.DrawPos(MovementTarget.position, Color.red);
        }
        if (HasArrived()) return;
        if (transform.position != MovementTarget.position){
            MoveToPos(CalculateNewPosition());
        }
    }

    private Vector3 CalculateNewPosition(){
        float distance = speed * Time.deltaTime;
        Vector3 newPosDir = (MovementTarget.position - transform.position).normalized;
        Vector3 newPos = transform.position + newPosDir * distance;
        return newPos;
    }
}
