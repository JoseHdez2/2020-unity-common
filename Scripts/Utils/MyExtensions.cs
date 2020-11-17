using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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

        public static Vector3 upperLeft(this Vector3 pos)
        {
            return pos + Vector3.up + Vector3.left;
        }

        public static Vector3 upperRight(this Vector3 pos)
        {
            return pos + Vector3.up + Vector3.right;
        }

        public static Vector3 lowerLeft(this Vector3 pos)
        {
            return pos + Vector3.down + Vector3.left;
        }

        public static Vector3 lowerRight(this Vector3 pos)
        {
            return pos + Vector3.down + Vector3.right;
        }

        public static string Color(this string str, Color color)
        {
            return $"<color={color.ToRGBA()}>{str}</color>";
        }

        public static string Wavy(this string str, float speed, float intensity, float letterOffset)
        {
            float[] offsets = new float[str.Length];
            string letters = str.ToList().Select((c, i) => {
                float offset = Mathf.Sin((Time.time + i * letterOffset) * speed) * intensity;
                if (Math.Abs(offset) < 0.001f) { offset = 0; }
                return $"<voffset={offset:0.000}em>{c}";
            }).Aggregate("", (a, b) => a + b);
            return letters + "</voffset>";
        }

        public static void DeleteAllChildren(this Transform transform)
        {
            foreach (Transform child in transform) {
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

        public static string ToRGBA(this Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGBA(color);
        }

        public static UnityEvent NewEvent(UnityAction call)
        {
            UnityEvent e = new UnityEvent();
            e.AddListener(call);
            return e;
        }

        // https://forum.unity.com/threads/pick-random-point-inside-box-collider.541585/ // post #8
        public static Vector3 GetRandomPoint(this Collider collider)
        {
            var point = new Vector3(
                UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                UnityEngine.Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                UnityEngine.Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );

            if (point != collider.ClosestPoint(point)) {
                Debug.Log("Out of the collider! Looking for other point...");
                point = collider.GetRandomPoint();
            }

            return point;
        }

        public static List<T> EnumValues<T>(){
            var list = new List<T>();
            foreach (var itemType in Enum.GetValues(typeof(T))) {
                list.Add((T)itemType);
            }
            return list;
        }

        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate){
            return !source.Any(predicate);
        }

        public static void LogColor(string stringToLog, string color){
            Debug.LogFormat($"<color={color}>{stringToLog}</color>");
        }

        public static void LogRed(string stringToLog){
            LogColor(stringToLog, "red");
        }
    }
}