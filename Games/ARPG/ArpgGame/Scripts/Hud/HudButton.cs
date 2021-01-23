using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class KeyCodeOverrides
{
    public static Dictionary<KeyCode, string> dict = new Dictionary<KeyCode, string>
    {
        [KeyCode.UpArrow] = "↑",
        [KeyCode.DownArrow] = "↓",
        [KeyCode.LeftArrow] = "←",
        [KeyCode.RightArrow] = "→",
    };
}

[ExecuteInEditMode]
public class HudButton : MonoBehaviour
{
    public KeyCode keyCode;

    public TMP_Text hudBtnText;
    public Image hudBtnPanel;

    private Color colorBtnUp;
    public Color colorBtnDown;

    public void Start()
    {
        colorBtnUp = hudBtnPanel.color;
        hudBtnText.text = KeyCodeOverrides.dict.ContainsKey(keyCode) ? 
            KeyCodeOverrides.dict[keyCode] : keyCode.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            hudBtnPanel.color = colorBtnDown;
        }
        if (Input.GetKeyUp(keyCode))
        {
            hudBtnPanel.color = colorBtnUp;
        }
    }

}
