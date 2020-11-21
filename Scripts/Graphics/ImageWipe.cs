using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageWipe : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 3f)]
    private float wipeSpeed = 1f;
    [SerializeField]
    [Range(0.1f, 3f)]
    private float wipeSpeedFast = 0.1f;

    private Image image;

    [Serializable]
    public enum WipeMode { Empty, WipingToEmpty, Filled, WipingToFilled }

    public WipeMode wipeMode = WipeMode.Empty;

    private float wipeProgress;
    
    public bool isWipingFast { get; private set; }

    public bool isDone() => wipeMode == WipeMode.Empty || wipeMode == WipeMode.Filled;

    private void Awake(){
        image = GetComponentInChildren<Image>();
    }

    private void Start(){
        if(wipeMode == WipeMode.Empty) {
            image.fillAmount = 0;
            ToggleWipe(false);
        }
        else if (wipeMode == WipeMode.Filled) {
            image.fillAmount = 1;
            ToggleWipe(true);
        }
    }

    // TODO rename this method. "toggle" shouldn't take a bool, and "wipe" is ambiguous.
    public void ToggleWipe(bool fillScreen){
        image.raycastTarget = true;
        isWipingFast = false;
        wipeMode = fillScreen ? WipeMode.WipingToFilled : WipeMode.WipingToEmpty;
    }

    public void ToggleWipeFast(bool fillScreen){
        image.raycastTarget = true;
        isWipingFast = true;
        wipeMode = fillScreen ? WipeMode.WipingToFilled : WipeMode.WipingToEmpty;
    }

    private void Update(){
        wipeProgress = image.fillAmount;
        switch (wipeMode)
        {
            case WipeMode.WipingToFilled:
                WipeToFilled(isWipingFast ? wipeSpeedFast : wipeSpeed);
                break;
            case WipeMode.WipingToEmpty:
                WipeToEmpty(isWipingFast ? wipeSpeedFast : wipeSpeed);
                break;
        }
    }

    private void WipeToFilled(float wipeSpeed)
    {
        wipeProgress += Time.deltaTime * (1f / wipeSpeed);
        image.fillAmount = wipeProgress;
        if (wipeProgress >= 1f)
        {
            wipeMode = WipeMode.Filled;
            image.raycastTarget = true;
        }
    }

    private void WipeToEmpty(float wipeSpeed)
    {
        wipeProgress -= Time.deltaTime * (1f / wipeSpeed);
        image.fillAmount = wipeProgress;
        if (wipeProgress <= 0)
        {
            wipeMode = WipeMode.Empty;
            image.raycastTarget = false;
        }
    }

    [ContextMenu("Block")]
    private void Block() { ToggleWipe(true); }
    [ContextMenu("Clear")]
    private void Clear() { ToggleWipe(false); }

}