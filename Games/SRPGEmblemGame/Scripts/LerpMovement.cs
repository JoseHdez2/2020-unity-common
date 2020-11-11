using UnityEngine;

public class LerpMovement : MonoBehaviour {
    
    public Vector3? destinationPos;

    [Range(0,1)]
    public float speed = 0.05f;

    protected void Update() {
        if(destinationPos.HasValue){
            transform.position = Vector3.Lerp(transform.position, destinationPos.Value, speed);
            if(Vector3.Distance(transform.position, destinationPos.Value) < 0.05){
                transform.position = destinationPos.Value;
                destinationPos = null;
            }
        }
    }
}