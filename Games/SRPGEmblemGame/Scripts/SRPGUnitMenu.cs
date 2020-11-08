using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRPGUnitMenu : MonoBehaviour
{
    [SerializeField] private ImageWipe buttonContainer;

    private SRPGAudioSource audioSource;
    private SRPGFieldCursor fieldCursor;
    private ButtonMenuBase menu;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = FindObjectOfType<SRPGAudioSource>();
        fieldCursor = FindObjectOfType<SRPGFieldCursor>();
        menu = FindObjectOfType<ButtonMenuBase>();
        buttonContainer.ToggleWipe(false);
    }

    public void Open(){
        buttonContainer.ToggleWipe(true);
        fieldCursor.gameObject.SetActive(false);
        audioSource.PlaySound(ESRPGSound.SelectUnit);
        menu.ResetCursor();
    }

    public void Close(){
        buttonContainer.ToggleWipe(false);
        fieldCursor.gameObject.SetActive(true);
        audioSource.PlaySound(ESRPGSound.Cancel);
    }    

    // Update is called once per frame
    void Update()
    {
        
    }
}
