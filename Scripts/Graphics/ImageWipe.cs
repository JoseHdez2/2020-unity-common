using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageWipe : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 3f)]
    private float wipeSpeed = 1f;

    private Image image;

    [Serializable]
    public enum WipeMode { Empty, WipingToEmpty, Filled, WipingToFilled }

    public WipeMode wipeMode = WipeMode.Empty;

    private float wipeProgress;

    public bool isDone { get; private set; }

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        if(wipeMode == WipeMode.Empty) { image.fillAmount = 0; }
        else if (wipeMode == WipeMode.Filled) { image.fillAmount = 1; }
    }

    public void ToggleWipe(bool fillScreen)
    {
        isDone = false;
        wipeMode = fillScreen ? WipeMode.WipingToFilled : WipeMode.WipingToEmpty;
    }

    private void Update()
    {
        wipeProgress = image.fillAmount;
        switch (wipeMode)
        {
            case WipeMode.WipingToFilled:
                WipeToFilled();
                break;
            case WipeMode.WipingToEmpty:
                WipeToEmpty();
                break;
        }
    }

    private void WipeToFilled()
    {
        wipeProgress += Time.deltaTime * (1f / wipeSpeed);
        image.fillAmount = wipeProgress;
        if (wipeProgress >= 1f)
        {
            isDone = true;
            wipeMode = WipeMode.Filled;
        }
    }

    private void WipeToEmpty()
    {
        wipeProgress -= Time.deltaTime * (1f / wipeSpeed);
        image.fillAmount = wipeProgress;
        if (wipeProgress <= 0)
        {
            isDone = true;
            wipeMode = WipeMode.Empty;
        }
    }

    [ContextMenu("Block")]
    private void Block() { ToggleWipe(true); }
    [ContextMenu("Clear")]
    private void Clear() { ToggleWipe(false); }

}