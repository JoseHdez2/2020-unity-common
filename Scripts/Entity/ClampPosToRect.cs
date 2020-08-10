using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampPosToRect : Pausable{
    public BoxCollider2D rect;

    protected override void Update2() {
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, rect.bounds.min.x, rect.bounds.max.x),
            Mathf.Clamp(transform.position.y, rect.bounds.min.y, rect.bounds.max.y)
        );
    }

    protected override void FixedUpdate2() { return; }

    protected override void OnPause(bool isPaused) { return; }
}
