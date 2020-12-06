using UnityEngine;
using System.Collections;

public class ButtonGo : Selectable
{
    private MetroGameStation terminal;
    protected override void Awake()
    {
        base.Awake();
        terminal = GetComponentInParent<MetroGameStation>();
    }

    protected override void OnClickDown()
    {
        terminal.Go();
    }
}
