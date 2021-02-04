using UnityEngine;

public class Minijam72Ally : MonoBehaviour {
    public float followPlayerSpeed = 5f, followEnemySpeed = 10f;
    private MovementFollowTarget movementFollowTarget;
    private void Start() {
        movementFollowTarget = GetComponent<MovementFollowTarget>();
    }

    public void AttackTarget(Collider2D collision){
        Debug.Log(collision.gameObject.name);
        movementFollowTarget.speed = 10f;
        movementFollowTarget.target = collision.gameObject.transform;
    }

}