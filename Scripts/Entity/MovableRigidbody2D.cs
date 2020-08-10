using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableRigidbody2D : Movable
{
    private Rigidbody2D rb2d;
    
    protected void Start(){
        rb2d = GetComponent<Rigidbody2D>();
    }

    override protected void MoveToPos(Vector3 newPos){
        rb2d.MovePosition(newPos);
    }
}
