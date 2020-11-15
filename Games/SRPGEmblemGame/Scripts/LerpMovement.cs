using UnityEngine;

public class LerpMovement : MonoBehaviour {
    
    public Vector3? destinationPos;

    public float speed = 1f;

    public void Update() {
        if(destinationPos.HasValue){
            transform.position = Vector3.Lerp(transform.position, destinationPos.Value, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, destinationPos.Value) < 0.05){
                transform.position = destinationPos.Value;
                destinationPos = null;
            }
        }
    }
}