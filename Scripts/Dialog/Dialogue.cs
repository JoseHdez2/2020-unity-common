using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogBubble {
    public string name;
    public DialogConfig config;
    [TextArea(3, 10)]
    public string text;
}

[Serializable]
[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialog")]
public class Dialogue : ScriptableObject {
    [SerializeField]
    public List<DialogBubble> dialogBubbles;
}
