using System;
using UnityEngine;
using ExtensionMethods;

public class DebugDraw {
    public static void DrawPos(Vector3 pos, Color color)
    {
        Debug.DrawLine(pos.upperLeft(), pos.lowerRight(), color);
        Debug.DrawLine(pos.lowerLeft(), pos.upperRight(), color);
    }

    internal static void DrawOrientation(Transform t, Color color)
    {
        Vector3 target = t.position + t.right;
        Debug.DrawLine(t.position, target);
    }
}
