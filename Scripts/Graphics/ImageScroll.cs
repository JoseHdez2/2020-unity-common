using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Works for both a 3d object with a renderer as well as a UI Image.
public class ImageScroll : MonoBehaviour
{
    public float scrollX = 0.5f;
    public float scrollY = 0.5f;

    private Renderer myRenderer;
    private Image image;
    void Start(){
        myRenderer = GetComponent<Renderer>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myRenderer){
            myRenderer.material.mainTextureOffset = new Vector2(scrollX * Time.time, scrollY * Time.time);
        }
        if(image){
            // TODO
            image.material.mainTextureOffset = new Vector2(scrollX * Time.time, scrollY * Time.time);
        }
    }
}
