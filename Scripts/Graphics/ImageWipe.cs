using System;
using UnityEngine;
using UnityEngine.UI;

public interface IToggleable {
    void Toggle(bool enable);
    bool IsDone();
}

public class ImageWipe : MonoBehaviour, IToggleable
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

    public bool IsDone() => wipeMode == WipeMode.Empty || wipeMode == WipeMode.Filled;

    private void Awake(){
        image = GetComponentInChildren<Image>();
    }

    private void Start(){
        if(wipeMode == WipeMode.Empty) {
            image.fillAmount = 0;
            Toggle(false);
        }
        else if (wipeMode == WipeMode.Filled) {
            image.fillAmount = 1;
            Toggle(true);
        }
    }

    public void Toggle(bool fillScreen){
        if(image != null){
            image.raycastTarget = true;
        }
        isWipingFast = false;
        wipeMode = fillScreen ? WipeMode.WipingToFilled : WipeMode.WipingToEmpty;
    }

    public void ToggleFast(bool fillScreen){
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
    private void Block() { Toggle(true); }
    [ContextMenu("Clear")]
    private void Clear() { Toggle(false); }

}