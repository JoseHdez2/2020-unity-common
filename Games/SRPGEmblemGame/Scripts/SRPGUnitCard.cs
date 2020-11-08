using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SRPGUnitCard : MonoBehaviour
{
    public TMP_Text unitNameText;
    public TMP_Text unitTypeText;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
