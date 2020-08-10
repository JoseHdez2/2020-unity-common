using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteAutoSlicer : EditorWindow
{
    Sprite[,] tileSprites;
    public int numColumns, numRows;
    public Sprite sourceSprite;

    [MenuItem("Tools/Sprite Auto-Slicer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SpriteAutoSlicer));
    }

    private void OnGUI()
    {
        numColumns = EditorGUILayout.IntField("Number of Columns", numColumns);
        numRows = EditorGUILayout.IntField("Number of Rows", numRows);
        sourceSprite = (Sprite)EditorGUILayout.ObjectField("Source Sprite", sourceSprite, typeof(Sprite), false);

        if (GUILayout.Button("Slice Image"))
        {
            // tileSprites = SliceImage(numColumns, numRows, sourceSprite);
        }
    }

    public static Sprite[,] SliceImage(Sprite sourceSprite, int numColumns, int numRows)
    {

        int PixelsToUnits = sourceSprite.texture.height / numRows;
        Sprite[,] tileSprites = new Sprite[numColumns, numRows];
        for (int i = 0; i < numColumns; i++)
        {
            for (int y = 0; y < numRows; y++)
            {
                Rect theArea = new Rect(i * PixelsToUnits, y * PixelsToUnits, PixelsToUnits, PixelsToUnits);
                tileSprites[i, y] = Sprite.Create(sourceSprite.texture, theArea, Vector2.zero, PixelsToUnits);
            }
        }
        return tileSprites;
    }
}
