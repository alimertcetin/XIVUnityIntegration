using UnityEditor;
using XIV.Core.Utils;
using XIVUnityEngineIntegration.Extensions;

namespace XIV.UnityEngineIntegration
{
    using UnityEngine;
    using System.Collections.Generic;
    using XIV.Core.Collections;
    using XIV.Core.DataStructures;
    using XIV.Core.XIVMath;

    public static class XIVDebug
    {
        static IXIVDebugger debugger = new XIVUnityDebugger();

        // Line
        public static void DrawLine(Vec3 from, Vec3 to, float duration = 0f)
        {
            debugger.DrawLine(from, to, XIVColor.white, true, duration);
        }

        public static void DrawLine(Vec3 from, Vec3 to, XIVColor xivColor, float duration = 0f)
        {
            debugger.DrawLine(from, to, xivColor, true, duration);
        }

        public static void DrawLine(Vec3 from, Vec3 to, XIVColor xivColor, bool depthTest, float duration = 0f)
        {
            debugger.DrawLine(from, to, xivColor, depthTest, duration);
        }

        // Bezier
        public static void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, XIVColor xivColor, int detail, float duration = 0f)
        {
            debugger.DrawBezier(p0, p1, p2, p3, xivColor, detail, duration);
        }

        public static void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, XIVColor xivColor, float duration = 0f)
        {
            debugger.DrawBezier(p0, p1, p2, p3, xivColor, 10, duration);
        }

        public static void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, float duration = 0f)
        {
            debugger.DrawBezier(p0, p1, p2, p3, XIVColor.white, 10, duration);
        }

        public static void DrawBezierWithT(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, float t, float duration)
        {
            debugger.DrawBezierWithT(p0, p1, p2, p3, t, XIVColor.white, 10, duration);
        }

        public static void DrawBezier(Vec3[] curve, XIVColor xivColor, int detail, float duration = 0f)
        {
            debugger.DrawBezier(curve[0], curve[1], curve[2], curve[3], xivColor, detail, duration);
        }

        public static void DrawBezier(Vec3[] curve, XIVColor xivColor, float duration = 0f)
        {
            debugger.DrawBezier(curve[0], curve[1], curve[2], curve[3], xivColor, 10, duration);
        }

        public static void DrawBezier(XIVMemory<Vec3> curve, XIVColor xivColor, int detail, float duration = 0f)
        {
            debugger.DrawBezier(curve, xivColor, detail, duration);
        }

        public static void DrawBezier(XIVMemory<Vec3> curve, XIVColor xivColor, float duration = 0f)
        {
            debugger.DrawBezier(curve[0], curve[1], curve[2], curve[3], xivColor, 10, duration);
        }

        public static void DrawBezier(Vec3[] curve, float duration = 0f)
        {
            debugger.DrawBezier(curve[0], curve[1], curve[2], curve[3], XIVColor.white, 10, duration);
        }

        public static void DrawBezier(XIVMemory<Vec3> curve, float duration = 0f)
        {
            debugger.DrawBezier(curve[0], curve[1], curve[2], curve[3], XIVColor.white, 10, duration);
        }

        public static void DrawBezier(Vec3[] curve, float t, float duration)
        {
#if UNITY_EDITOR
            DrawBezierWithT(curve[0], curve[1], curve[2], curve[3], t, duration);
#endif
        }

        public static void DrawBezier(XIVMemory<Vec3> curve, float t, float duration)
        {
#if UNITY_EDITOR
            DrawBezierWithT(curve[0], curve[1], curve[2], curve[3], t, duration);
#endif
        }

        public static void DrawBezierDetailed(XIVMemory<Vec3> curve, int detail = 100, float duration = 0f)
        {
            debugger.DrawBezierDetailed(curve, detail, duration);
        }

        // Spline
        public static void DrawSpline(IList<Vec3> points, XIVColor xivColor, int detail, float duration = 0f)
        {
#if UNITY_EDITOR
            UnityEngine.Color c = xivColor.ToUnityColor();
            var p1 = points[0].ToVector3();
            for (int i = 1; i <= detail; i++)
            {
                float t = i / (float)detail;
                var p2 = SplineMath.GetPoint(points, t).ToVector3();
                Debug.DrawLine(p1, p2, c, duration);
                p1 = p2;
            }
#endif
        }

        public static void DrawSpline(IList<Vec3> points, float t, XIVColor XIVColor, int detail, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawSpline(points, XIVColor, detail, duration);
            var current = SplineMath.GetPoint(points, t);
            DrawSphere(current, 0.2f, XIVColor.red, duration);
#endif
        }

        // Sphere
        public static void DrawSphere(in Vec3 position, in float radius, in XIVColor xivColor, in int detail, in int circleDetail, in float duration = 0)
        {
#if UNITY_EDITOR
            for (int i = 0; i < detail; i++)
            {
                var angle = i * (XIVMathf.TAU / detail);
                var axis = Vector3.RotateTowards(Vector3.forward, Vector3.back, angle, 180f);
                DrawCircle(position, radius, axis.ToVec3(), xivColor, circleDetail, duration);
            }
#endif
        }

        static readonly XIVColor DefaultCircleXIVColor = XIVColor.cyan;
        const int DEFAULT_SPHERE_DETAIL = 5;
        const int DEFAULT_CIRCLE_DETAIL = 8;
        
        public static void DrawSphere(Vec3 position, float radius, float duration = 0)
        {
#if UNITY_EDITOR
            debugger.DrawSphere(position, radius, XIVColor.white, DEFAULT_SPHERE_DETAIL, DEFAULT_CIRCLE_DETAIL, duration);
#endif
        }

        public static void DrawSphere(Vec3 position, float radius, XIVColor xivColor, float duration = 0)
        {
#if UNITY_EDITOR
            DrawSphere(position, radius, xivColor, DEFAULT_SPHERE_DETAIL, DEFAULT_CIRCLE_DETAIL, duration);
#endif
        }

        // Circle
        public static void DrawCircle(Vec3 position, float radius, Vec3 axis, XIVColor XIVColor, int detail, float duration = 0)
        {
#if UNITY_EDITOR
            UnityEngine.Color c = XIVColor.ToUnityColor();
            var pos = position;
            var rotation = XIVQuaternion.FromToRotation(Vec3.forward, axis);
            var startPoint = (pos + rotation * Vec3.right * radius);
            var p1 = startPoint;
            for (int i = 1; i <= detail; i++)
            {
                float angle = i * (360f / detail);
                var p2 = pos + rotation * XIVQuaternion.AngleAxis(angle, Vec3.forward) * Vec3.right * radius;
                Debug.DrawLine(p1.ToVector3(), p2.ToVector3(), c, duration);
                p1 = p2;
            }
#endif
        }

        public static void DrawCircle(Vec3 position, float radius, float duration = 0)
        {
#if UNITY_EDITOR
            DrawCircle(position, radius, Vec3.forward, DefaultCircleXIVColor, DEFAULT_CIRCLE_DETAIL, duration);
#endif
        }

        public static void DrawCircle(Vec3 position, float radius, Vec3 axis, float duration = 0)
        {
#if UNITY_EDITOR
            DrawCircle(position, radius, axis, DefaultCircleXIVColor, DEFAULT_CIRCLE_DETAIL, duration);
#endif
        }

        public static void DrawCircle(Vec3 position, float radius, Vec3 axis, XIVColor XIVColor, float duration = 0)
        {
#if UNITY_EDITOR
            DrawCircle(position, radius, axis, XIVColor, DEFAULT_CIRCLE_DETAIL, duration);
#endif
        }

        public static void DrawCircle(Vec3 position, float radius, XIVColor XIVColor, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawCircle(position, radius, Vec3.forward, XIVColor, duration);
#endif
        }

        // Bounds
        public static void DrawBounds(Bounds bounds, float duration = 0f)
        {
#if UNITY_EDITOR
            // bottom
            var p1 = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
            var p2 = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            var p3 = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            var p4 = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);

            Debug.DrawLine(p1, p2, Color.blue, duration);
            Debug.DrawLine(p2, p3, Color.red, duration);
            Debug.DrawLine(p3, p4, Color.yellow, duration);
            Debug.DrawLine(p4, p1, Color.magenta, duration);

            // top
            var p5 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            var p6 = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            var p7 = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
            var p8 = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);

            Debug.DrawLine(p5, p6, Color.blue, duration);
            Debug.DrawLine(p6, p7, Color.red, duration);
            Debug.DrawLine(p7, p8, Color.yellow, duration);
            Debug.DrawLine(p8, p5, Color.magenta, duration);

            // sides
            Debug.DrawLine(p1, p5, Color.white, duration);
            Debug.DrawLine(p2, p6, Color.gray, duration);
            Debug.DrawLine(p3, p7, Color.green, duration);
            Debug.DrawLine(p4, p8, Color.cyan, duration);
#endif
        }

        // Rectangle
        public static void DrawRectangle(Vector3 center, Vector3 halfExtents, Quaternion orientation, float duration = 0f)
        {
#if UNITY_EDITOR
            halfExtents.z = 0f; // We are working in 2D plane

            // Define local corners around origin (center-relative)
            Vector3 localBL = new Vector3(-halfExtents.x, -halfExtents.y, 0f);
            Vector3 localBR = new Vector3(halfExtents.x, -halfExtents.y, 0f);
            Vector3 localTR = new Vector3(halfExtents.x, halfExtents.y, 0f);
            Vector3 localTL = new Vector3(-halfExtents.x, halfExtents.y, 0f);

            // Rotate and translate to world space
            Vector3 worldBL = center + orientation * localBL;
            Vector3 worldBR = center + orientation * localBR;
            Vector3 worldTR = center + orientation * localTR;
            Vector3 worldTL = center + orientation * localTL;

            // Draw rectangle edges
            Debug.DrawLine(worldBL, worldBR, Color.red, duration);
            Debug.DrawLine(worldBR, worldTR, Color.green, duration);
            Debug.DrawLine(worldTR, worldTL, Color.red, duration);
            Debug.DrawLine(worldTL, worldBL, Color.green, duration);
#endif
        }

        public static void DrawRectangle(Vec3 center, Vec3 halfExtends, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawRectangle(center.ToVector3(), halfExtends.ToVector3(), Quaternion.identity, duration);
#endif
        }

#if UNITY_EDITOR
        // Text
        class TextHelper : MonoBehaviour
        {
            public struct TextData
            {
                public Vector3 position;
                public string text;
                public int size;
                public Color color;
                public Timer timer;
            }

            public DynamicArray<TextData> textDatas = new DynamicArray<TextData>(8);

            static TextHelper instance;
            public static TextHelper Instance => instance == null ? instance = new GameObject("XIVDebug - TextHelper").AddComponent<TextHelper>() : instance;

            void OnDrawGizmos()
            {
                for (int i = textDatas.Count - 1; i >= 0; i--)
                {
                    ref var textData = ref textDatas[i];
                    var style = new GUIStyle();
                    style.fontSize = textData.size;
                    style.normal.textColor = textData.color;
                    Handles.Label(textData.position, textData.text, style);
                    if (textData.timer.Update(Time.deltaTime))
                    {
                        textDatas.RemoveAt(i);
                    }
                }
            }

            void OnDestroy()
            {
                instance = null;
            }
        }
#endif


        public static void DrawText(Vec3 position, string text, int size, XIVColor xivColor, float duration = 0f)
        {
#if UNITY_EDITOR
            // Do not create TextHelper if not in play mode
            if (Application.isPlaying == false)
            {
                var style = new GUIStyle();
                style.fontSize = size;
                style.normal.textColor = xivColor.ToUnityColor();
                Handles.Label(position.ToVector3(), text, style);
                return;
            }
            TextHelper.Instance.textDatas.Add() = new TextHelper.TextData
            {
                position = position.ToVector3(),
                text = text,
                color = xivColor.ToUnityColor(),
                size = size,
                timer = new Timer(duration),
            };
#endif
        }

        public static void DrawText(Vec3 position, string text, int size, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawText(position, text, size, XIVColor.black, duration);
#endif
        }

        public static void DrawText(Vec3 position, string text, float duration = 0f)
        {
#if UNITY_EDOTOR
            var size = (int)HandleUtility.GetHandleSize(position);
            DrawText(position, text, size, XIVColor.black, duration);
#endif
        }

        public static void DrawTextOnLine(Vec3 from, Vec3 to, string text, int size, XIVColor XIVColor, float t, float duration)
        {
#if UNITY_EDITOR
            var position = from + (to - from) * t;
            DrawText(position, text, size, XIVColor, duration);
#endif
        }

        public static void DrawTextOnLine(Vec3 from, Vec3 to, string text, int size, XIVColor XIVColor, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawTextOnLine(from, to, text, size, XIVColor, 0.5f, duration);
#endif
        }

        public static void DrawTextOnLine(Vec3 from, Vec3 to, string text, int size, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawTextOnLine(from, to, text, size, XIVColor.black, 0.5f, duration);
#endif
        }

        public static void DrawTextOnLine(Vec3 from, Vec3 to, string text, float duration = 0f)
        {
#if UNITY_EDITOR
            var position = (from + (to - from) * 0.5f);
            var size = (int)HandleUtility.GetHandleSize(position.ToVector3());
            DrawText(position, text, size, XIVColor.black, duration);
#endif
        }

    }

}