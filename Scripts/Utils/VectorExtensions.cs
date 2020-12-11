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

        public static Vector3 WithY(this Vector3 v, float newY) => new Vector3(v.x, newY, v.z);

        public static BoundsInt WithSize(this BoundsInt b, int width, int height) => new BoundsInt(b.position, new Vector3Int(width, height, 1));

        public static Bounds WithSize(this BoundsInt b, Vector3 size) => new Bounds(b.position, size);
        
        public static BoundsInt TopWall(this BoundsInt b) => new BoundsInt(){xMin=0, yMin=0}.WithSize(width:b.size.x, height:1);
        public static BoundsInt BottomWall(this BoundsInt b) => new BoundsInt(){xMin=0, yMin=b.yMax-1}.WithSize(width:b.size.x, height:1);
        public static BoundsInt LeftWall(this BoundsInt b) => new BoundsInt(){xMin=0, yMin=0}.WithSize(width:1, height:b.size.y);
        public static BoundsInt RightWall(this BoundsInt b) => new BoundsInt(){xMin=b.xMax-1, yMin=0}.WithSize(width:1, height:b.size.y); // FIXME
    }
}