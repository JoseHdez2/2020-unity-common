using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Destroy something when outside camera bounds. Note: OnBecameVisible requires a Renderer component.
/// </summary>
public class DestroyOnInvisible : MonoBehaviour {

    void OnBecameInvisible() {
        Destroy(this.gameObject);
    }
}
