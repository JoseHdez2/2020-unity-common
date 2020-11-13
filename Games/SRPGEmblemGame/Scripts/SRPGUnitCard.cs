using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SrpgUnitCard : MonoBehaviour
{
    public ImageWipe cardBg;
    public TMP_Text unitNameText;
    public TMP_Text unitTypeText;
    public TMP_Text unitHpText;

    private List<TMP_Text> texts = new List<TMP_Text>();

    // Start is called before the first frame update
    void Start()
    {
        texts = new List<TMP_Text>();
        texts.AddRange(new TMP_Text[]{unitNameText, unitTypeText, unitHpText});
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO if overlapping cursor, move out of the way.
    }

    public void SetUnit(SrpgUnit unit){
        unitNameText.text = unit.name;
        unitTypeText.text = unit.typeId;
        unitHpText.text = $"HP: {unit.hp} / {unit.maxHp}";
    }

    public void Open(){
        texts.ForEach(t => t.gameObject.SetActive(true));
        cardBg.ToggleWipe(true);
    }

    public void Close(){
        texts.ForEach(t => t.gameObject.SetActive(false));
        cardBg.ToggleWipe(false);
    }
}
