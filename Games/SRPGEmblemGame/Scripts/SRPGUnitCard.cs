using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SRPGUnitCard : MonoBehaviour
{
    public ImageWipe cardBg;
    public TMP_Text unitNameText;
    public TMP_Text unitTypeText;

    // Start is called before the first frame update
    void Start()
    {
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO if overlapping cursor, move out of the way.
    }

    public void SetUnit(SRPGUnit unit){
        unitNameText.text = unit.name;
        unitTypeText.text = unit.typeId;
    }

    public void Open(){
        unitNameText.gameObject.SetActive(true);
        unitTypeText.gameObject.SetActive(true);
        cardBg.ToggleWipe(true);
    }

    public void Close(){
        unitNameText.gameObject.SetActive(false);
        unitTypeText.gameObject.SetActive(false);
        cardBg.ToggleWipe(false);
    }
}
