using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static int WordCount(this string str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }

        // https://answers.unity.com/questions/585035/lookat-2d-equivalent-.html
        public static void LookAt2D(this Transform t, Transform target)
        {
            t.right = target.position - t.position;
        }

        // https://answers.unity.com/questions/650460/rotating-a-2d-sprite-to-face-a-target-on-a-single.html
        public static void TurnTowardsTarget(this Transform t, Transform target, float turnSpeed)
        {
            Vector3 vectorToTarget = target.position - t.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            t.rotation = Quaternion.Slerp(t.rotation, q, Time.deltaTime * turnSpeed);
        }


        public static void SnapToGrid(this Transform transform)
        {
            transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)
            );
        }

        public static Vector3 upperLeft(this Vector3 pos) { return pos + Vector3.up + Vector3.left; }
        public static Vector3 upperRight(this Vector3 pos) { return pos + Vector3.up + Vector3.right; }
        public static Vector3 lowerLeft(this Vector3 pos) { return pos + Vector3.down + Vector3.left; }
        public static Vector3 lowerRight(this Vector3 pos) { return pos + Vector3.down + Vector3.right; }

        public static T Pop<T>(this List<T> list)
        {
            T val = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return val;
        }

        // https://stackoverflow.com/a/18867218/3399416
        public static bool IsEmpty<T>(this IEnumerable<T> list) => !list.Any();

        public static string Color(this string str, string color) => $"<color=\"{color}\">{str}</color>";

        public static string Wavy(this string str, float speed, float intensity, float letterOffset) {
            float[] offsets = new float[str.Length];
            string letters = str.ToList().Select((c,i) =>
            {
                float offset = Mathf.Sin((Time.time + i * letterOffset) * speed) * intensity;
                if(Math.Abs(offset) < 0.001f) { offset = 0; }
                return $"<voffset={offset:0.000}em>{c}";
            }).Aggregate("", (a, b) => a + b);
            return letters + "</voffset>";
        }

        public static void DeleteAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }

        public static bool IsTouchingALot(this Collider collider, Collider otherCollider)
        {
            // TODO checks if the center of one is inside another. improve this
            return collider.bounds.Contains(otherCollider.bounds.ClosestPoint(collider.bounds.center));

            return collider.bounds.Contains(otherCollider.bounds.center)
                || otherCollider.bounds.Contains(collider.bounds.center);
        }

        public static bool IsTouchingALot(this GameObject collider, GameObject otherCollider)
        {
            return IsTouchingALot(collider.GetComponent<Collider>(), otherCollider.GetComponent<Collider>());
        }
    }
}