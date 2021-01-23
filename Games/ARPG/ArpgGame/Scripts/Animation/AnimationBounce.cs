using UnityEngine;
using System.Collections;

public class AnimationBounce : MonoBehaviour
{
    public float yPosVariance = 1; // TODO rename
    public float period = 0.2f;

    private Vector2 initPos;
    void Awake()
    {
        initPos = gameObject.transform.position;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float yPos = initPos.y + yPosVariance * Mathf.Sin(Time.deltaTime);
        // Debug.Log(yPos);
        gameObject.transform.position = new Vector2(initPos.x, yPos);
        yield return new WaitForSeconds(period);
        StartCoroutine(Animate());
    }
}
