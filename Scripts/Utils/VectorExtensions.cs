using UnityEngine;
using System;

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

        public static float ManhattanDistance(this Vector2 v, Vector2 other)
        {
            return Math.Abs(v.x - other.x) + Math.Abs(v.y - other.y);
        }

        public static float ManhattanDistance(this Vector3 v, Vector3 other)
        {
            return Math.Abs(v.x - other.x) + Math.Abs(v.y - other.y) + Math.Abs(v.z - other.z);
        }
    }
}