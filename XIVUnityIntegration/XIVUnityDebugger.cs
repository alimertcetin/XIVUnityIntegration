using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XIV.Core.Collections;
using XIV.Core.DataStructures;
using XIV.Core.Utils;
using XIV.Core.XIVMath;
using XIVUnityEngineIntegration.Extensions;

namespace XIV.UnityEngineIntegration
{
    public class XIVUnityDebugger : IXIVDebugger
    {
#if UNITY_EDITOR
        const float TAU = 6.283185307179586f;

        static readonly XIVColor DefaultBezierXIVColor = new XIVColor(1f, 1f, 1f, 1f); // Same as XIVColor.white
        const int DEFAULT_BEZIER_DETAIL = 20;

        static readonly XIVColor DefaultCircleXIVColor = new XIVColor(0f, 0f, 1f, 1f); // Same as XIVColor.blue
        const int DEFAULT_CIRCLE_DETAIL = 10;

        static readonly XIVColor DefaultSphereXIVColor = new XIVColor(1f, 0f, 0f, 1f); // Same as XIVColor.red
        const int DEFAULT_SPHERE_DETAIL = 20;
#endif

        // Line
        public void DrawLine(Vec3 from, Vec3 to, float duration = 0f)
        {
#if UNITY_EDITOR
            Debug.DrawLine(from.ToVector3(), to.ToVector3(), Color.white, duration);
#endif
        }

        public void DrawLine(Vec3 from, Vec3 to, XIVColor xivColor, float duration = 0f)
        {
#if UNITY_EDITOR
            Debug.DrawLine(from.ToVector3(), to.ToVector3(), xivColor.ToUnityColor(), duration);
#endif
        }

        public void DrawLine(Vec3 from, Vec3 to, XIVColor xivColor, bool depthTest, float duration = 0f)
        {
#if UNITY_EDITOR
            Debug.DrawLine(from.ToVector3(), to.ToVector3(), xivColor.ToUnityColor(), duration, depthTest);
#endif
        }

        // Bezier
        public void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, XIVColor xivColor, int detail, float duration = 0f)
        {
#if UNITY_EDITOR
            var point1 = p0.ToVector3();
            for (int i = 1; i <= detail; i++)
            {
                float t = i / (float)detail;
                var point2 = BezierMath.GetPoint(p0, p1, p2, p3, t).ToVector3();
                Debug.DrawLine(point1, point2, xivColor.ToUnityColor(), duration);
                point1 = point2;
            }
#endif
        }

        public void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, XIVColor xivColor, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawBezier(p0, p1, p2, p3, xivColor, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }

        public void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawBezier(p0, p1, p2, p3, DefaultBezierXIVColor, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }

        public void DrawBezier(Vec3[] curve, XIVColor xivColor, int detail, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawBezier(curve[0], curve[1], curve[2], curve[3], xivColor, detail, duration);
#endif
        }

        public void DrawBezier(Vec3[] curve, XIVColor xivColor, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawBezier(curve[0], curve[1], curve[2], curve[3], xivColor, duration);
#endif
        }

        public void DrawBezier(Vec3[] curve, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawBezier(curve[0], curve[1], curve[2], curve[3], duration);
#endif
        }

        public void DrawBezier(XIVMemory<Vec3> curve, XIVColor xivColor, int detail, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawBezier(curve[0], curve[1], curve[2], curve[3], xivColor, detail, duration);
#endif
        }

        public void DrawBezier(XIVMemory<Vec3> curve, XIVColor xivColor, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawBezier(curve[0], curve[1], curve[2], curve[3], xivColor, duration);
#endif
        }

        public void DrawBezier(XIVMemory<Vec3> curve, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawBezier(curve[0], curve[1], curve[2], curve[3], duration);
#endif
        }
        public void DrawBezierWithT(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, float t, XIVColor xivColor, int detail, float duration)
        {
#if UNITY_EDITOR
            DrawBezier(p0, p1, p2, p3, xivColor, detail, duration);
            var current = BezierMath.GetPoint(p0, p1, p2, p3, t);
            DrawSphere(current, 0.2f, XIVColor.red, duration);
#endif
        }
        public void DrawBezierWithT(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, float t, XIVColor xivColor, float duration)
        {
#if UNITY_EDITOR
            DrawBezierWithT(p0, p1, p2, p3, t, xivColor, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }
        public void DrawBezierWithT(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, float t, float duration)
        {
#if UNITY_EDITOR
            DrawBezierWithT(p0, p1, p2, p3, t, DefaultBezierXIVColor, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }
        
        public void DrawBezierWithT(Vec3[] curve, float t, XIVColor xivColor, int detail, float duration)
        {
#if UNITY_EDITOR
            DrawBezierWithT(curve[0], curve[1], curve[2], curve[3], t, xivColor, detail, duration);
#endif
        }
        public void DrawBezierWithT(Vec3[] curve, float t, XIVColor xivColor, float duration)
        {
#if UNITY_EDITOR
            DrawBezierWithT(curve[0], curve[1], curve[2], curve[3], t, xivColor, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }
        public void DrawBezierWithT(Vec3[] curve, float t, float duration)
        {
#if UNITY_EDITOR
            DrawBezierWithT(curve[0], curve[1], curve[2], curve[3], t, DefaultBezierXIVColor, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }
        
        public void DrawBezierWithT(XIVMemory<Vec3> curve, float t, XIVColor xivColor, int detail, float duration)
        {
#if UNITY_EDITOR
            DrawBezierWithT(curve[0], curve[1], curve[2], curve[3], t, xivColor, detail, duration);
#endif
        }
        
        public void DrawBezierWithT(XIVMemory<Vec3> curve, float t, XIVColor xivColor, float duration)
        {
#if UNITY_EDITOR
            DrawBezierWithT(curve[0], curve[1], curve[2], curve[3], t, xivColor, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }
        
        public void DrawBezierWithT(XIVMemory<Vec3> curve, float t, float duration)
        {
#if UNITY_EDITOR
            DrawBezierWithT(curve[0], curve[1], curve[2], curve[3], t, DefaultBezierXIVColor, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }

        public void DrawBezierDetailed(XIVMemory<Vec3> curve, int detail = 100, float duration = 0f)
        {
#if UNITY_EDITOR
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
#endif
        }
        
        // Spline
        public void DrawSpline(IList<Vec3> points, XIVColor xivColor, int detail, float duration = 0f)
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
        public void DrawSpline(IList<Vec3> points, XIVColor xivColor, float duration)
        {
#if UNITY_EDITOR
            DrawSpline(points, xivColor, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }
        
        public void DrawSpline(IList<Vec3> points, float duration)
        {
#if UNITY_EDITOR
            DrawSpline(points, XIVColor.white, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }
        
        public void DrawSplineWithT(IList<Vec3> points, XIVColor xivColor, float t, int detail, float duration)
        {
#if UNITY_EDITOR
            DrawSpline(points, xivColor, detail, duration);
            var current = SplineMath.GetPoint(points, t);
            DrawSphere(current, 0.2f, XIVColor.red, duration);
#endif
        }
        
        public void DrawSplineWithT(IList<Vec3> points, XIVColor xivColor, float t, float duration)
        {
#if UNITY_EDITOR
            DrawSplineWithT(points, xivColor, t, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }
        
        public void DrawSplineWithT(IList<Vec3> points, float t, float duration)
        {
#if UNITY_EDITOR
            DrawSplineWithT(points, XIVColor.white, t, DEFAULT_BEZIER_DETAIL, duration);
#endif
        }

        // Sphere
        public void DrawSphere(Vec3 position, float radius, XIVColor xivColor, int detail, int circleDetail, float duration = 0)
        {
#if UNITY_EDITOR
            for (int i = 0; i < detail; i++)
            {
                var angle = i * (TAU / detail);
                var axis = Vector3.RotateTowards(Vector3.forward, Vector3.back, angle, 180f);
                DrawCircle(position, radius, axis.ToVec3(), xivColor, circleDetail, duration);
            }
#endif
        }

        public void DrawSphere(Vec3 position, float radius, float duration = 0)
        {
#if UNITY_EDITOR
            DrawSphere(position, radius, DefaultSphereXIVColor, DEFAULT_SPHERE_DETAIL, DEFAULT_CIRCLE_DETAIL, duration);
#endif
        }

        public void DrawSphere(Vec3 position, float radius, XIVColor xivColor, float duration = 0)
        {
#if UNITY_EDITOR
            DrawSphere(position, radius, xivColor, DEFAULT_SPHERE_DETAIL, DEFAULT_CIRCLE_DETAIL, duration);
#endif
        }

        // Circle
        public void DrawCircle(Vec3 position, float radius, Vec3 axis, XIVColor xivColor, int detail, float duration = 0)
        {
#if UNITY_EDITOR
            UnityEngine.Color c = xivColor.ToUnityColor();
            var pos = position.ToVector3();
            var rotation = Quaternion.FromToRotation(Vector3.forward, axis.ToVector3());
            var startPoint = (pos + rotation * Vector3.right * radius);
            var p1 = startPoint;
            for (int i = 1; i <= detail; i++)
            {
                float angle = i * (360f / detail);
                var p2 = pos + rotation * Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right * radius;
                Debug.DrawLine(p1, p2, c, duration);
                p1 = p2;
            }
#endif
        }

        public void DrawCircle(Vec3 position, float radius, float duration = 0)
        {
#if UNITY_EDITOR
            DrawCircle(position, radius, Vec3.forward, DefaultCircleXIVColor, DEFAULT_CIRCLE_DETAIL, duration);
#endif
        }

        public void DrawCircle(Vec3 position, float radius, Vec3 axis, float duration = 0)
        {
#if UNITY_EDITOR
            DrawCircle(position, radius, axis, DefaultCircleXIVColor, DEFAULT_CIRCLE_DETAIL, duration);
#endif
        }

        public void DrawCircle(Vec3 position, float radius, Vec3 axis, XIVColor xivColor, float duration = 0)
        {
#if UNITY_EDITOR
            DrawCircle(position, radius, axis, xivColor, DEFAULT_CIRCLE_DETAIL, duration);
#endif
        }

        public void DrawCircle(Vec3 position, float radius, XIVColor XIVColor, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawCircle(position, radius, Vec3.forward, XIVColor, duration);
#endif
        }

        // Bounds
        public void DrawBounds(Vec3 min, Vec3 max, float duration = 0f)
        {
#if UNITY_EDITOR
            // bottom
            var p1 = new Vector3(min.x, min.y, min.z);
            var p2 = new Vector3(max.x, min.y, min.z);
            var p3 = new Vector3(max.x, min.y, max.z);
            var p4 = new Vector3(min.x, min.y, max.z);

            Debug.DrawLine(p1, p2, Color.blue, duration);
            Debug.DrawLine(p2, p3, Color.red, duration);
            Debug.DrawLine(p3, p4, Color.yellow, duration);
            Debug.DrawLine(p4, p1, Color.magenta, duration);

            // top
            var p5 = new Vector3(min.x, max.y, min.z);
            var p6 = new Vector3(max.x, max.y, min.z);
            var p7 = new Vector3(max.x, max.y, max.z);
            var p8 = new Vector3(min.x, max.y, max.z);

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
        public void DrawRectangle(Vector3 center, Vector3 halfExtents, XIVQuaternion orientation, float duration = 0)
        {
#if UNITY_EDITOR
            DrawRectangle(center, halfExtents, orientation, duration);
#endif
        }

        // Rectangle
        public void DrawRectangle(Vec3 center, Vec3 halfExtents, XIVQuaternion orientation, float duration = 0f)
        {
#if UNITY_EDITOR
            halfExtents.z = 0f; // We are working in 2D plane

            // Define local corners around origin (center-relative)
            Vector3 localBL = new Vector3(-halfExtents.x, -halfExtents.y, 0f);
            Vector3 localBR = new Vector3(halfExtents.x, -halfExtents.y, 0f);
            Vector3 localTR = new Vector3(halfExtents.x, halfExtents.y, 0f);
            Vector3 localTL = new Vector3(-halfExtents.x, halfExtents.y, 0f);

            // Rotate and translate to world space
            var c = center.ToVector3();
            var o = orientation.ToQuaternion();
            Vector3 worldBL = c + o * localBL;
            Vector3 worldBR = c + o * localBR;
            Vector3 worldTR = c + o * localTR;
            Vector3 worldTL = c + o * localTL;

            // Draw rectangle edges
            Debug.DrawLine(worldBL, worldBR, Color.red, duration);
            Debug.DrawLine(worldBR, worldTR, Color.green, duration);
            Debug.DrawLine(worldTR, worldTL, Color.red, duration);
            Debug.DrawLine(worldTL, worldBL, Color.green, duration);
#endif
        }

        public void DrawRectangle(Vec3 center, Vec3 halfExtends, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawRectangle(center, halfExtends, XIVQuaternion.identity, duration);
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


        public void DrawText(Vec3 position, string text, int size, XIVColor xivColor, float duration = 0f)
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

        public void DrawText(Vec3 position, string text, int size, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawText(position, text, size, XIVColor.black, duration);
#endif
        }

        public void DrawText(Vec3 position, string text, float duration = 0f)
        {
#if UNITY_EDOTOR
            var size = (int)HandleUtility.GetHandleSize(position);
            DrawText(position, text, size, XIVColor.black, duration);
#endif
        }

        public void DrawTextOnLine(Vec3 from, Vec3 to, string text, int size, XIVColor xivColor, float t, float duration)
        {
#if UNITY_EDITOR
            var position = from + (to - from) * t;
            DrawText(position, text, size, xivColor, duration);
#endif
        }

        public void DrawTextOnLine(Vec3 from, Vec3 to, string text, int size, XIVColor xivColor, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawTextOnLine(from, to, text, size, xivColor, 0.5f, duration);
#endif
        }

        public void DrawTextOnLine(Vec3 from, Vec3 to, string text, int size, float duration = 0f)
        {
#if UNITY_EDITOR
            DrawTextOnLine(from, to, text, size, XIVColor.black, 0.5f, duration);
#endif
        }

        public void DrawTextOnLine(Vec3 from, Vec3 to, string text, float duration = 0f)
        {
#if UNITY_EDITOR
            var position = (from + (to - from) * 0.5f);
            var size = (int)HandleUtility.GetHandleSize(position.ToVector3());
            DrawText(position, text, size, XIVColor.black, duration);
#endif
        }

    }
}