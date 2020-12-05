using UnityEngine;
using System.Collections;


namespace ExtensionMethods
{
    public static class MathUtils
    {
        public static int ClampToPositive(this int val){
            return val < 0 ? 0 : val;
        }

        public static int Clamp(this int val, int min, int max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        // for wrapping the menu cursor around.
        public static int InverseClamp(this int val, int min, int max)
        {
            if (val < min) return max;
            if (val > max) return min;
            return val;
        }

        public static bool IsBetween(this int i, int min, int max) => (i >= min && i <= max);
    }
}
