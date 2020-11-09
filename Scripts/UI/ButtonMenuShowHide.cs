using UnityEngine;

public class ButtonMenuShowHide : ButtonMenuBase {
    
    [SerializeField] private ImageWipe menuBg;
    [SerializeField] private CanvasGroup menuCanvasGroup;

    protected void Start() {
        // base.Start();
        menuBg.ToggleWipe(false);
        menuCanvasGroup.blocksRaycasts = false;
        menuCanvasGroup.interactable = false;
    }

    public void Show(){
        menuBg.ToggleWipe(true);
        menuCanvasGroup.blocksRaycasts = true;
        menuCanvasGroup.interactable = true;
        gameObject.SetActive(true);
        ResetCursor();
    }

    public void Hide(){
        menuBg.ToggleWipe(false);
        menuCanvasGroup.blocksRaycasts = false;
        menuCanvasGroup.interactable = false;
        gameObject.SetActive(false);
    }

    public void OnEnable(){
        base.OnEnable();

        selectedButton = null;
    }

    // Override to play a sound when selection changes.
    protected virtual void OnChangeSelection(){}


    // Must reset to null each time the menu becomes inactive. Only used for making a sound when the selected button changes.
    private GameObject selectedButton;

    // Update is called once per frame
    void Update()
    {
        // base.Update();

        if (eventSystem.currentSelectedGameObject != selectedButton) {
            if(selectedButton != null){ // Avoid playing when the first button becomes selected automatically. but play thereafter.
                OnChangeSelection();
            }
            selectedButton = eventSystem.currentSelectedGameObject;
        }
    }

}