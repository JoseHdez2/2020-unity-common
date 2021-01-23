using UnityEngine;
using System.Collections;

public class ButtonSell : Selectable
{
    private MetroGameTerminalTrading terminal;
    protected override void Awake()
    {
        base.Awake();
        terminal = GetComponentInParent<MetroGameTerminalTrading>();
    }

    protected override void OnClickDown()
    {
        terminal.Sell();
    }
}
