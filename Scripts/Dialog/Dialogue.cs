using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DialogPos { Left, Right, Center }

[Serializable]
public class DialogBubble {
    public string name;
    public DialogConfig config;
    [TextArea(3, 10)]
    public string text;
    public int spriteIndex = -1;
    public DialogPos pos;
}

[Serializable]
[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialog")]
public class Dialogue : ScriptableObject {
    [SerializeField]
    public List<DialogBubble> dialogBubbles;
    public List<Sprite> sprites;
}
