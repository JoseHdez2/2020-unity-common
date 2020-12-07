using UnityEngine;
using System.Collections;

public class ButtonBuy : Selectable
{
    private MetroGameTerminalTrading terminal;

    protected override void Awake()
    {
        base.Awake();
        terminal = GetComponentInParent<MetroGameTerminalTrading>();
    }

    protected override void OnClickDown()
    {
        terminal.Buy();
    }
}
