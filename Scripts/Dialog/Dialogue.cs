using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Dialogue {
    public Dialogue() { }
    public Dialogue(string[] sentences) { this.sentences = sentences.ToList(); }

    public string name;
    public float timePerCharacter = 0.1f;
    public float pitch = 1f;
    public float pitchVariance = 0f;

    [TextArea(3, 10)]
    public List<string> sentences;
}
