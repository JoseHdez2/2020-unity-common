using UnityEngine;
using System.Collections;

public class MathUtils
{
    public static int ClampToPositive(int val){
        return val < 0 ? 0 : val;
    }

    public static int Clamp(int val, int min, int max)
    {
        if (val < min) return min;
        if (val > max) return max;
        return val;
    }

    // for wrapping the menu cursor around.
    public static int InverseClamp(int val, int min, int max)
    {
        if (val < min) return max;
        if (val > max) return min;
        return val;
    }
}
