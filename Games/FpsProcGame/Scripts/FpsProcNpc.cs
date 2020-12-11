﻿using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;
using TMPro;

[System.Serializable]
public class FpsProcNpcData {
    public string uuid, fullName, greeting;
    public int faceIndex;
}

public class FpsProcNpc : MonoBehaviour
{
    public FpsProcNpcData data = new FpsProcNpcData();
    public Texture2D face;
    public MeshRenderer faceRenderer;
    [SerializeField] private TMP_Text textName;
    private static List<string> greetings = new List<string>(){"Hello.", "Hi!", "What's up?", "Yes?"};

    // Start is called before the first frame update
    void Start(){
        FpsProcDatabase db = FindObjectOfType<FpsProcDatabase>();
        data.fullName = db.GetRandomFullName();
        name = data.fullName;
        textName.text = data.fullName;
        // faceRenderer.material = new Material(faceShader);
        data.faceIndex = db.GetRandomFaceIndex();
        data.greeting = greetings.RandomItem();
        faceRenderer.material.mainTexture = FpsProcDatabase.faces[data.faceIndex];
        faceRenderer.material.color = Color.white;
    }

    public void ClickedOn(){
        FindObjectOfType<FpsProcGameMgr>().ClickNpc(this, data.greeting);
    }
}
