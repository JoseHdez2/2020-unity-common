using UnityEngine;
using System.Collections;

public class ButtonToggleRadio : Selectable
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnClickDown()
    {
        FindObjectOfType<MusicController>().toggle();
    }
}
