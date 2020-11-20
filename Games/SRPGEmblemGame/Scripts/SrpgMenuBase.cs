using UnityEngine;

public abstract class SrpgMenuBase : ButtonMenuBase {
    protected SrpgUnit selectedUnit;
    
    protected SrpgAudioSource audioSource;
    protected SrpgController srpgController;
    protected SrpgDatabase srpgEmblemDatabase;
    [SerializeField] protected ImageWipe buttonContainer;

    void Start()
    {
        audioSource = FindObjectOfType<SrpgAudioSource>();
        srpgController = FindObjectOfType<SrpgController>();
        srpgEmblemDatabase = FindObjectOfType<SrpgDatabase>();
        buttonContainer.ToggleWipe(false);
    }

    public void OnEnable(){
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
                audioSource.PlaySound(ESRPGSound.MenuCursor);
            }
            selectedButton = eventSystem.currentSelectedGameObject;
        }
        if(Input.GetButtonDown("Cancel")){
            HandleCancel();
        }
    }

    protected abstract void HandleCancel();

    public void Close(){
        buttonContainer.ToggleWipe(false);
        audioSource.PlaySound(ESRPGSound.Cancel);
        gameObject.SetActive(false);
        srpgController.UpdateTeamsHard();
    }    
}