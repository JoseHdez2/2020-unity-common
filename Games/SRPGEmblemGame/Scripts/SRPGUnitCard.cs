using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class SrpgUnitCard : MonoBehaviour
{
    public ImageWipe cardBg;
    public TMP_Text unitNameText;
    public TMP_Text unitTypeText;
    public TMP_Text unitHpText;

    private List<TMP_Text> texts = new List<TMP_Text>();

    private SrpgController srpgController;

    void Awake(){
        texts = new List<TMP_Text>();
        texts.AddRange(new TMP_Text[]{unitNameText, unitTypeText, unitHpText});
        srpgController = FindObjectOfType<SrpgController>();
    }

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

    public void SetUnit(SrpgUnit unit){
        unitNameText.text = unit.name;
        unitTypeText.text = unit.typeId;
        unitHpText.text = $"HP: {unit.hp} / {unit.maxHp}";
        Color c = (unit.teamId == "good guys") ? srpgController.colorGood : srpgController.colorBad;
        cardBg.GetComponent<Image>().color = new Color(c.r, c.g, c.b, cardBg.GetComponent<Image>().color.a);
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
