using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    [TextArea(2, 4)]
    public string[] credits;
    private int index = 0;
    public TMP_Text text;

    // Start is called before the first frame update

    
    void Start()
    {
        InputMasterSingleton.Get().Menu.Confirm.performed += _ => SetCreditsIndex(index + 1);
    }

    void OnEnable()
    {
        FindObjectOfType<TitleScreenMenuController>(includeInactive: true).enabled = false;
        SetCreditsIndex(0);
        text.text = credits[index];
    }

    void SetCreditsIndex(int index)
    {
        this.index = index;
        if (index >= credits.Length)
        {
            FindObjectOfType<TitleScreenMenuController>(includeInactive: true).enabled = true;
        } else {
            text.text = credits[this.index];
        }
    }
}
