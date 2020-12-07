using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;
using TMPro;

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
    [SerializeField] private TMP_Text textName;

    // Start is called before the first frame update
    void Start(){
        FpsProcDatabase db = FindObjectOfType<FpsProcDatabase>();
        data.fullName = db.GetRandomFullName();
        name = data.fullName;
        textName.text = data.fullName;
        // faceRenderer.material = new Material(faceShader);
        data.faceIndex = db.GetRandomFaceIndex();
        faceRenderer.material.mainTexture = FpsProcDatabase.faces[data.faceIndex];
        faceRenderer.material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickedOn(){
        Debug.Log($"You clicked on {data.fullName}.");
    }
}
