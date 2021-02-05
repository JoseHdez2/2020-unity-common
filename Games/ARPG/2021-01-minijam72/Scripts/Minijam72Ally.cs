using UnityEngine;

public class Minijam72Ally : MonoBehaviour {
    public float followPlayerSpeed = 5f, followEnemySpeed = 10f;
    private MovementFollowTarget movementFollowTarget;
    private void Start() {
        movementFollowTarget = GetComponent<MovementFollowTarget>();
    }

    public void AttackTarget(Collider2D coll){
        movementFollowTarget.speed = followEnemySpeed;
        movementFollowTarget.target = coll.gameObject.transform;
    }

}