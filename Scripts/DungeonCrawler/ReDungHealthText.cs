using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReDungHealthText : MonoBehaviour
{
    private DungeonCrawlerPlayer player;
    public int textSizePercent = 150;
   
    void Update()
    {
        if(player == null)
        {
            player = FindObjectOfType<DungeonCrawlerPlayer>();
        }
    }

    public void UpdateText(int playerHealth)
    {
        int maxHp = FindObjectOfType<ReDungLevelInterpreter>().curLevelPlayerMaxHp;
        string maxHpStr = maxHp > 3 ? maxHp.ToString() : $"<color=\"red\">{maxHp}</color>";
        string str = $"<size={textSizePercent}%><smallcaps>Hp:</smallcaps> {playerHealth}<sub> / {maxHp}</sub>";
        GetComponent<TMP_Text>().text = str;
    }
}
