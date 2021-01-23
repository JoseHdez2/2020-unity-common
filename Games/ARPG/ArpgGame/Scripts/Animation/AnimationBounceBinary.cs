using UnityEngine;
using System.Collections;

public class AnimationBounceBinary : MonoBehaviour
{
    public float yPosVariance = 1; // TODO rename
    public float period = 0.2f;
    private bool up = false; // TODO rename

    private Vector2 initPos;
    void Awake()
    {
        initPos = gameObject.transform.position;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        up = !up;
        float yPos = up? initPos.y + yPosVariance : initPos.y;
        Debug.Log(yPos);
        gameObject.transform.position = new Vector2(initPos.x, yPos);
        yield return new WaitForSeconds(period);
        StartCoroutine(Animate());
    }
}
