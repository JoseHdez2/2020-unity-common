using System.Collections;
using UnityEngine;

public abstract class SrpgMenuBase : ButtonMenuBase {
    protected SrpgUnit selectedUnit;
    
    protected SrpgAudioSource audioSource;
    protected SrpgController srpgController;
    protected SrpgDatabase srpgEmblemDatabase;

    new void Awake()
    {
        base.Awake();
        audioSource = FindObjectOfType<SrpgAudioSource>();
        srpgController = FindObjectOfType<SrpgController>(includeInactive: true);
        srpgEmblemDatabase = FindObjectOfType<SrpgDatabase>();
    }

    public new void OnEnable(){
        base.OnEnable();

        selectedButton = null;
    }

    // Must reset to null each time the menu becomes inactive. Only used for making a sound when the selected button changes.
    private GameObject selectedButton;

    // Update is called once per frame
    void Update()
    {         
        if (eventSystem.currentSelectedGameObject != selectedButton) {
            if(selectedButton != null){ // Avoid playing when the first button becomes selected automatically. but play thereafter.
                audioSource.PlaySound(ESrpgSound.MenuCursor);
            }
            selectedButton = eventSystem.currentSelectedGameObject;
            Debug.Log($"Selected: {selectedButton}");
        }
        if(Input.GetButtonDown("Cancel")){
            HandleCancel();
        }
    }

    protected abstract void HandleCancel();

    public override void PreClose() {
        audioSource.PlaySound(ESrpgSound.Cancel);
    }

    public override void PostClose() {
        srpgController.UpdateTeamsAndCheckForTurnChange();
    }
}