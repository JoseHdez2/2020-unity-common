using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class FpsProcNpc : MonoBehaviour
{
    public static List<string> unisexNames = new List<string>{"Addison", "Adrian", "Alex", "Arden", "Aubrey", "August", "Bailey"};
    public List<Texture2D> faces;
    public Texture2D face;
    public MeshRenderer faceRenderer;
    public Shader faceShader;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = $"{unisexNames.RandomItem()} Smith";
        // faceRenderer.material = new Material(faceShader);
        faceRenderer.material.mainTexture = faces.RandomItem();
        faceRenderer.material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
