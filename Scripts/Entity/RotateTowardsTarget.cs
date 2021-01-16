using UnityEngine;

// To use this script, attach it to the GameObject that you would like to rotate towards another game object.
// After attaching it, go to the inspector and drag the GameObject you would like to rotate towards into the target field.
// Move the target around in the scene view to see the GameObject continuously rotate towards it.
public class RotateTowardsTarget : MonoBehaviour
{
    [Tooltip("Tag to find target transform, if none is provided.")]
    public string targetTag;
    // The target marker.
    public Transform target;
    public bool lockRotX, lockRotY, lockRotZ;

    private void Start() {
        FindTargetIfMissing();
    }

    void FindTargetIfMissing() {
        if (target == null && !string.IsNullOrEmpty(targetTag)) {
            GameObject obj = GameObject.FindGameObjectWithTag(targetTag);
            if (obj != null) {
                target = obj.transform;
            }
        }
    }

    // Angular speed in radians per sec.
    public float turnSpeed = 1.0f;

    void Update() {
        FindTargetIfMissing();
        RotTowardsTarget();
    }

    void RotTowardsTarget() {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = turnSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        Vector3 newDirectionWithLocks = new Vector3(
            lockRotX ? transform.forward.x : newDirection.x,
            lockRotX ? transform.forward.y : newDirection.y,
            lockRotX ? transform.forward.z : newDirection.z
        );

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirectionWithLocks);
    }
}