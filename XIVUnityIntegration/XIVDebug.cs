using UnityEditor;
using XIV.Core.Utils;

namespace XIV.UnityEngineIntegration
{
    using UnityEngine;
    using System.Collections.Generic;
    using XIV.Core.Collections;
    using XIV.Core.DataStructures;
    using XIV.Core.XIVMath;
    
    public static class XIVDebug
    {
        const float TAU = 6.283185307179586f;
        
        static readonly XIVColor DefaultBezierXIVColor = new XIVColor(1f, 1f, 1f, 1f); // Same as XIVColor.white
        const int DEFAULT_BEZIER_DETAIL = 20;
        
        static readonly XIVColor DefaultCircleXIVColor = new XIVColor(0f, 0f, 1f, 1f); // Same as XIVColor.blue
        const int DEFAULT_CIRCLE_DETAIL = 10;

        static readonly XIVColor DefaultSphereXIVColor = new XIVColor(1f, 0f, 0f, 1f); // Same as XIVColor.red
        const int DEFAULT_SPHERE_DETAIL = 20;
        
        // Line
        public static void DrawLine(Vec3 from, Vec3 to, float duration = 0f)
        {
            Debug.DrawLine(from, to, XIVColor.white, duration);
        }
        
        public static void DrawLine(Vec3 from, Vec3 to, XIVColor XIVColor, float duration = 0f)
        {
            Debug.DrawLine(from, to, XIVColor, duration);
        }
        
        public static void DrawLine(Vec3 from, Vec3 to, XIVColor XIVColor, bool depthTest, float duration = 0f)
        {
            Debug.DrawLine(from, to, XIVColor, duration, depthTest);
        }

        // Bezier
        public static void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, XIVColor XIVColor, int detail, float duration = 0f)
        {
            var point1 = p0;
            for (int i = 1; i <= detail; i++)
            {
                float t = i / (float)detail;
                var point2 = BezierMath.GetPoint(p0, p1, p2, p3, t);
                Debug.DrawLine(point1, point2, XIVColor, duration);
                point1 = point2;
            }
        }
        
        public static void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, XIVColor XIVColor, float duration = 0f)
        {
            DrawBezier(p0, p1, p2, p3, XIVColor, DEFAULT_BEZIER_DETAIL, duration);
        }
        
        public static void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, float duration = 0f)
        {
            DrawBezier(p0, p1, p2, p3, DefaultBezierXIVColor, DEFAULT_BEZIER_DETAIL, duration);
        }
        
        public static void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, float t, float duration)
        {
            DrawBezier(p0, p1, p2, p3, DefaultBezierXIVColor, DEFAULT_BEZIER_DETAIL, duration);
            var current = BezierMath.GetPoint(p0, p1, p2, p3, t);
            DrawSphere(current, 0.2f, XIVColor.red, duration);
        }
        
        public static void DrawBezier(Vec3[] curve, XIVColor XIVColor, int detail, float duration = 0f)
        {
            DrawBezier(curve[0], curve[1], curve[2], curve[3], XIVColor, detail, duration);
        }
        
        public static void DrawBezier(XIVMemory<Vec3> curve, XIVColor XIVColor, int detail, float duration = 0f)
        {
            DrawBezier(curve[0], curve[1], curve[2], curve[3], XIVColor, detail, duration);
        }
        
        public static void DrawBezier(Vec3[] curve, XIVColor XIVColor, float duration = 0f)
        {
            DrawBezier(curve[0], curve[1], curve[2], curve[3], XIVColor, duration);
        }
        
        public static void DrawBezier(XIVMemory<Vec3> curve, XIVColor XIVColor, float duration = 0f)
        {
            DrawBezier(curve[0], curve[1], curve[2], curve[3], XIVColor, duration);
        }
        
        public static void DrawBezier(Vec3[] curve, float duration = 0f)
        {
            DrawBezier(curve[0], curve[1], curve[2], curve[3], duration);
        }
        
        public static void DrawBezier(XIVMemory<Vec3> curve, float duration = 0f)
        {
            DrawBezier(curve[0], curve[1], curve[2], curve[3], duration);
        }
        
        public static void DrawBezier(Vec3[] curve, float t, float duration)
        {
            DrawBezier(curve[0], curve[1], curve[2], curve[3], t, duration);
        }
        
        public static void DrawBezier(XIVMemory<Vec3> curve, float t, float duration)
        {
            DrawBezier(curve[0], curve[1], curve[2], curve[3], t, duration);
        }

        public static void DrawBezierDetailed(XIVMemory<Vec3> curve, int detail = 100, float duration = 0f)
        {
            var p0 = BezierMath.GetCurveData(curve, 0f);
            for (int i = 1; i <= detail; i++)
            {
                float t = (float)i / detail;
                var p1 = BezierMath.GetCurveData(curve, t);
                DrawLine(p0.point, p1.point, XIVColor.magenta, duration);
                DrawLine(p0.point, p0.point + (p0.right * 0.25f), XIVColor.red, duration);
                DrawLine(p0.point, p0.point + (p0.forward * 0.25f), XIVColor.blue, duration);
                DrawLine(p0.point, p0.point + (p0.normal * 0.25f), XIVColor.yellow, duration);
                DrawSphere(p0.point, 0.01f, XIVColor.red, duration);
                p0 = p1;
            }

            var curveLen = SplineMath.GetLength(curve);
            if (Application.isPlaying)
            {
                DrawTextOnLine(curve[1], curve[^2], curveLen.ToString(), 100, XIVColor.blue, duration);
            }
        }
        
        // Spline
        public static void DrawSpline(IList<Vec3> points, XIVColor XIVColor, int detail, float duration = 0f)
        {
            var p1 = points[0];
            for (int i = 1; i <= detail; i++)
            {
                float t = i / (float)detail;
                var p2 = SplineMath.GetPoint(points, t);
                Debug.DrawLine(p1, p2, XIVColor, duration);
                p1 = p2;
            }
        }

        public static void DrawSpline(IList<Vec3> points, float t, XIVColor XIVColor, int detail, float duration = 0f)
        {
            DrawSpline(points, XIVColor, detail, duration);
            var current = SplineMath.GetPoint(points, t);
            DrawSphere(current, 0.2f, XIVColor.red, duration);
        }
        
        // Sphere
        public static void DrawSphere(Vec3 position, float radius, XIVColor XIVColor, int detail, int circleDetail, float duration = 0)
        {
            for (int i = 0; i < detail; i++)
            {
                var angle = i * (TAU / detail);
                var axis = Vector3.RotateTowards(Vector3.forward, Vector3.back, angle, 180f);
                DrawCircle(position, radius, axis, XIVColor, circleDetail, duration);
            }
        }
        
        public static void DrawSphere(Vec3 position, float radius, float duration = 0)
        {
            DrawSphere(position, radius, DefaultSphereXIVColor, DEFAULT_SPHERE_DETAIL, DEFAULT_CIRCLE_DETAIL, duration);
        }
        
        public static void DrawSphere(Vec3 position, float radius, XIVColor XIVColor, float duration = 0)
        {
            DrawSphere(position, radius, XIVColor, DEFAULT_SPHERE_DETAIL, DEFAULT_CIRCLE_DETAIL, duration);
        }
        
        // Circle
        public static void DrawCircle(Vec3 position, float radius, Vec3 axis, XIVColor XIVColor, int detail, float duration = 0)
        {
            var rotation = Quaternion.FromToRotation(Vec3.forward, axis);
            var startPoint = (Vector3)position + rotation * Vec3.right * radius;
            var p1 = startPoint;
            for (int i = 1; i <= detail; i++)
            {
                float angle = i * (360f / detail);
                var p2 = (Vector3)position + rotation * Quaternion.AngleAxis(angle, Vec3.forward) * Vec3.right * radius;
                Debug.DrawLine(p1, p2, XIVColor, duration);
                p1 = p2;
            }
        }
        
        public static void DrawCircle(Vec3 position, float radius, float duration = 0)
        {
            DrawCircle(position, radius, Vec3.forward, DefaultCircleXIVColor, DEFAULT_CIRCLE_DETAIL, duration);
        }

        public static void DrawCircle(Vec3 position, float radius, Vec3 axis, float duration = 0)
        {
            DrawCircle(position, radius, axis, DefaultCircleXIVColor, DEFAULT_CIRCLE_DETAIL, duration);
        }

        public static void DrawCircle(Vec3 position, float radius, Vec3 axis, XIVColor XIVColor, float duration = 0)
        {
            DrawCircle(position, radius, axis, XIVColor, DEFAULT_CIRCLE_DETAIL, duration);
        }

        public static void DrawCircle(Vec3 position, float radius, XIVColor XIVColor, float duration = 0f)
        {
            DrawCircle(position, radius, Vec3.forward, XIVColor, duration);
        }
        
        // Bounds
        public static void DrawBounds(Bounds bounds, float duration = 0f)
        {
            // bottom
            var p1 = new Vec3(bounds.min.x, bounds.min.y, bounds.min.z);
            var p2 = new Vec3(bounds.max.x, bounds.min.y, bounds.min.z);
            var p3 = new Vec3(bounds.max.x, bounds.min.y, bounds.max.z);
            var p4 = new Vec3(bounds.min.x, bounds.min.y, bounds.max.z);

            Debug.DrawLine(p1, p2, XIVColor.blue, duration);
            Debug.DrawLine(p2, p3, XIVColor.red, duration);
            Debug.DrawLine(p3, p4, XIVColor.yellow, duration);
            Debug.DrawLine(p4, p1, XIVColor.magenta, duration);

            // top
            var p5 = new Vec3(bounds.min.x, bounds.max.y, bounds.min.z);
            var p6 = new Vec3(bounds.max.x, bounds.max.y, bounds.min.z);
            var p7 = new Vec3(bounds.max.x, bounds.max.y, bounds.max.z);
            var p8 = new Vec3(bounds.min.x, bounds.max.y, bounds.max.z);

            Debug.DrawLine(p5, p6, XIVColor.blue, duration);
            Debug.DrawLine(p6, p7, XIVColor.red, duration);
            Debug.DrawLine(p7, p8, XIVColor.yellow, duration);
            Debug.DrawLine(p8, p5, XIVColor.magenta, duration);

            // sides
            Debug.DrawLine(p1, p5, XIVColor.white, duration);
            Debug.DrawLine(p2, p6, XIVColor.gray, duration);
            Debug.DrawLine(p3, p7, XIVColor.green, duration);
            Debug.DrawLine(p4, p8, XIVColor.cyan, duration);
        }

        // Rectangle
        // public static void DrawRectangle(Vector3 center, Vector3 halfExtends, Quaternion orientation, float duration = 0f)
        // {
        //     halfExtends.z = 0f;
        //     var bottomLeft = center - halfExtends;
        //     var topRight = center + halfExtends;
        //     var topLeft = new Vector3(bottomLeft.x, topRight.y);
        //     var bottomRight = new Vector3(topRight.x, bottomLeft.y);
        //     bottomLeft = center + orientation * (bottomLeft - center);
        //     topRight = center + orientation * (topRight - center);
        //     topLeft = center + orientation * (topLeft - center);
        //     bottomRight = center + orientation * (bottomRight - center);
        //     
        //     Debug.DrawLine(bottomLeft, bottomRight, XIVColor.red, duration);
        //     Debug.DrawLine(bottomRight, topRight, XIVColor.green, duration);
        //     Debug.DrawLine(topRight, topLeft, XIVColor.red, duration);
        //     Debug.DrawLine(topLeft, bottomLeft, XIVColor.green, duration);
        // }
        
        public static void DrawRectangle(Vector3 center, Vector3 halfExtents, Quaternion orientation, float duration = 0f)
        {
            halfExtents.z = 0f; // We are working in 2D plane

            // Define local corners around origin (center-relative)
            Vector3 localBL = new Vector3(-halfExtents.x, -halfExtents.y, 0f);
            Vector3 localBR = new Vector3( halfExtents.x, -halfExtents.y, 0f);
            Vector3 localTR = new Vector3( halfExtents.x,  halfExtents.y, 0f);
            Vector3 localTL = new Vector3(-halfExtents.x,  halfExtents.y, 0f);

            // Rotate and translate to world space
            Vector3 worldBL = center + orientation * localBL;
            Vector3 worldBR = center + orientation * localBR;
            Vector3 worldTR = center + orientation * localTR;
            Vector3 worldTL = center + orientation * localTL;

            // Draw rectangle edges
            Debug.DrawLine(worldBL, worldBR, XIVColor.red, duration);
            Debug.DrawLine(worldBR, worldTR, XIVColor.green, duration);
            Debug.DrawLine(worldTR, worldTL, XIVColor.red, duration);
            Debug.DrawLine(worldTL, worldBL, XIVColor.green, duration);
        }

        public static void DrawRectangle(Vec3 center, Vec3 halfExtends, float duration = 0f)
        {
            DrawRectangle(center, halfExtends, Quaternion.identity, duration);
        }
        
        // Text
        class TextHelper : MonoBehaviour
        {
            public struct TextData
            {
                public Vec3 position;
                public string text;
                public int size;
                public XIVColor XIVColor;
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
                    style.normal.textColor = textData.XIVColor;
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

        
        public static void DrawText(Vec3 position, string text, int size, XIVColor XIVColor, float duration = 0f)
        {
            // Do not create TextHelper if not in play mode
            if (Application.isPlaying == false)
            {
                var style = new GUIStyle();
                style.fontSize = size;
                style.normal.textColor = XIVColor;
                Handles.Label(position, text, style);
                return;
            }
            TextHelper.Instance.textDatas.Add() = new TextHelper.TextData
            {
                position = position, 
                text = text,
                XIVColor = XIVColor,
                size = size,
                timer = new Timer(duration),
            };
        }
        
        public static void DrawText(Vec3 position, string text, int size, float duration = 0f)
        {
            DrawText(position, text, size, XIVColor.black, duration);
        }
        
        public static void DrawText(Vec3 position, string text, float duration = 0f)
        {
            var size = (int)HandleUtility.GetHandleSize(position);
            DrawText(position, text, size, XIVColor.black, duration);
        }

        public static void DrawTextOnLine(Vec3 from, Vec3 to, string text, int size, XIVColor XIVColor, float t, float duration)
        {
            var position = from + (to - from) * t;
            DrawText(position, text, size, XIVColor, duration);
        }
        
        public static void DrawTextOnLine(Vec3 from, Vec3 to, string text, int size, XIVColor XIVColor, float duration = 0f)
        {
            DrawTextOnLine(from, to, text, size, XIVColor, 0.5f, duration);
        }
        
        public static void DrawTextOnLine(Vec3 from, Vec3 to, string text, int size, float duration = 0f)
        {
            DrawTextOnLine(from, to, text, size, XIVColor.black, 0.5f, duration);
        }
        
        public static void DrawTextOnLine(Vec3 from, Vec3 to, string text, float duration = 0f)
        {
            var position = from + (to - from) * 0.5f;
            var size = (int)HandleUtility.GetHandleSize(position);
            DrawText(position, text, size, XIVColor.black, duration);
        }

    }
}