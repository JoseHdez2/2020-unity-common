using ExtensionMethods;
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
        int maxHp = DungeonCrawlerPlayer.playerMaxHP; // FindObjectOfType<ReDungLevelInterpreter>().curLevelPlayerMaxHp;
        string playerHealthStr = playerHealth > 3 ? playerHealth.ToString() : playerHealth.ToString().Color(Color.red);
        string str = $"<size={textSizePercent}%><smallcaps>Hp:</smallcaps>{playerHealth}<sub> / {maxHp}</sub>";
        GetComponent<TMP_Text>().text = str;
    }
}
