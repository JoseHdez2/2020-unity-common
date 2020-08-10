using UnityEngine;
using System.Collections;
using TMPro;

public class DamagePopup : Ephemeral
{
    public float speed = 1f;

    [Tooltip("Number of Units that the number will float above its parent at the start.")]
    public float heightOffset = 1f;

    private float timeLived = 0f;
    private TMP_Text text;
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        transform.position += new Vector3(0, heightOffset);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.up * speed;
    }

    public void SetPopupText(string str) { text.text = str; }

    public void SetPopupColor(Color color) { text.color = color; }

}
