using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SRPGUnitMenu : ButtonMenuBase
{
    [SerializeField] private ImageWipe buttonContainer;
    [SerializeField] private Button fightButton;

    private SRPGAudioSource audioSource;
    private SRPGFieldCursor fieldCursor;

    private SRPGUnit unit;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = FindObjectOfType<SRPGAudioSource>();
        fieldCursor = FindObjectOfType<SRPGFieldCursor>();
        buttonContainer.ToggleWipe(false);
    }

    public void OnEnable(){
        base.OnEnable();
        if(unit){
            if(!unit.InAttackRange()){
                fightButton.gameObject.SetActive(false);
            }
        }
        selectedButton = null;
    }

    public void Open(){
        buttonContainer.ToggleWipe(true);
        fieldCursor.gameObject.SetActive(false);
        gameObject.SetActive(true);
        audioSource.PlaySound(ESRPGSound.SelectUnit);
        ResetCursor();
    }

    public void Close(){
        buttonContainer.ToggleWipe(false);
        fieldCursor.gameObject.SetActive(true);
        audioSource.PlaySound(ESRPGSound.Cancel);
        gameObject.SetActive(false);
    }    

    // Must be null when the menu is inactive. Only used for making a sound when the selected button changes.
    private GameObject selectedButton;

    // Update is called once per frame
    void Update()
    {         
        if (eventSystem.currentSelectedGameObject != selectedButton) {
            if(selectedButton != null){
                audioSource.PlaySound(ESRPGSound.Move);
            }
            selectedButton = eventSystem.currentSelectedGameObject;
        }
    }
}
