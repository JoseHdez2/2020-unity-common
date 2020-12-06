using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

[System.Serializable]
public class FpsProcNpcData {
    public string fullName;
    public int faceIndex;
}

public class FpsProcNpc : MonoBehaviour
{
    public FpsProcNpcData data = new FpsProcNpcData();
    public Texture2D face;
    public MeshRenderer faceRenderer;
    public Shader faceShader;

    // Start is called before the first frame update
    void Start(){
        FpsProcDatabase db = FindObjectOfType<FpsProcDatabase>();
        data.fullName = db.GetRandomFullName();
        name = data.fullName;
        // faceRenderer.material = new Material(faceShader);
        data.faceIndex = db.GetRandomFaceIndex();
        faceRenderer.material.mainTexture = FpsProcDatabase.faces[data.faceIndex];
        faceRenderer.material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
