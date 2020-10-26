using UnityEngine;

namespace ExtensionMethods
{
    public static class VectorExtensions
    {
        public static Vector3 Vector3FromAngle(float angleInDegrees)
        {
            float angleRad = angleInDegrees * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float ToAngle(this Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            return n;
        }
    }
}