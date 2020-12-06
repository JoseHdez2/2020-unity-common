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
        
        public static int ManhattanDistanceInt(this Vector2 v, Vector2 other)
        {
            return (int)Math.Round(v.ManhattanDistance(other), MidpointRounding.AwayFromZero);
        }

        public static float ManhattanDistance(this Vector3 v, Vector3 other)
        {
            return Math.Abs(v.x - other.x) + Math.Abs(v.y - other.y) + Math.Abs(v.z - other.z);
        }

        public static Vector2Int RandomPos(this BoundsInt bounds) {
            return new Vector2Int(UnityEngine.Random.Range(bounds.xMin, bounds.xMax), UnityEngine.Random.Range(bounds.yMin, bounds.yMax));
        }
        public static Vector3 RandomPos(this Bounds bounds) {
            return new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
        }

        public static BoundsInt WithSize(this BoundsInt b, int width, int height) {
            return new BoundsInt(){xMin=b.xMin, xMax=b.xMin+width, yMin=b.yMin, yMax=b.yMin+height};
        }
    }
}