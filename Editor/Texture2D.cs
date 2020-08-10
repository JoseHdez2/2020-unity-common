using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextureToTiles : EditorWindow
{
    Texture2D texture;

    [MenuItem("Tools/Texture To Tiles")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TextureToTiles));
    }

    private void OnGUI()
    {
        texture = (Texture2D)EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), false);
        if (GUILayout.Button("Slice Image"))
        {
            // tileSprites = SliceImage(numColumns, numRows, sourceSprite);
        }
    }
}
