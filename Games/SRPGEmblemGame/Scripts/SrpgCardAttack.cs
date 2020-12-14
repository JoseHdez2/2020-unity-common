using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SrpgCardAttack : MonoBehaviour
{
    public ImageWipe cardBg;
    public TMP_Text attackNameText;
    public TMP_Text attackPowerText;
    public TMP_Text attackHitText;

    private List<TMP_Text> texts = new List<TMP_Text>();

    // Start is called before the first frame update
    void Start()
    {
        texts = new List<TMP_Text>();
        texts.AddRange(new TMP_Text[]{attackNameText, attackPowerText, attackHitText});
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO if overlapping cursor, move out of the way.
    }

    public void SetUnit(SrpgUnit unit){
        attackNameText.text = unit.name;
        attackPowerText.text = unit.typeId;
        attackHitText.text = $"HP: {unit.hp} / {unit.maxHp}";
    }

    public void Open(){
        texts.ForEach(t => t.gameObject.SetActive(true));
        cardBg.Toggle(true);
    }

    public void Close(){
        texts.ForEach(t => t.gameObject.SetActive(false));
        cardBg.Toggle(false);
    }
}
