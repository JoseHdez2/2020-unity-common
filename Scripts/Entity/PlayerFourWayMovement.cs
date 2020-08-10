using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFourWayMovement : Pausable
{
    public float speed;

    protected override void Update2() {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * Time.deltaTime;
    }

    protected override void FixedUpdate2() { return; }

    protected override void OnPause(bool isPaused) { return; }
}
