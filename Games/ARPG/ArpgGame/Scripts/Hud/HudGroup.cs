using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HudGroup : MonoBehaviour
{
    public List<HudGroup> altGroups;

    private List<HudButton> buttons;
    private void Awake()
    {
        buttons = GetComponentsInChildren<HudButton>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        bool anyDown = buttons.Any(btn => Input.GetKeyDown(btn.keyCode));
        if (anyDown)
        {
            ShowAllButtons(true);
            altGroups.ForEach(grp => grp.ShowAllButtons(false));
        }
    }

    void ShowAllButtons(bool show)
    {
        buttons.ForEach(btn => {
            btn.hudBtnPanel.enabled = show;
            btn.hudBtnText.enabled = show;
        });
    }
}
