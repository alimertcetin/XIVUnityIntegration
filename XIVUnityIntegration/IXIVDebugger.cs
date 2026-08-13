using System.Collections.Generic;
using XIV.Core.DataStructures;

namespace XIV.UnityEngineIntegration
{
    public interface IXIVDebugger
    {
        // Line
        public void DrawLine(Vec3 from, Vec3 to, XIVColor XIVColor, bool depthTest, float duration = 0f);

        // Bezier
        public void DrawBezier(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, XIVColor XIVColor, int detail, float duration = 0f);
        public void DrawBezier(Vec3[] curve, XIVColor XIVColor, int detail, float duration = 0f);
        public void DrawBezier(XIVMemory<Vec3> curve, XIVColor XIVColor, int detail, float duration = 0f);

        public void DrawBezierWithT(Vec3 p0, Vec3 p1, Vec3 p2, Vec3 p3, float t, XIVColor XIVColor, int detail, float duration = 0f);
        public void DrawBezierWithT(Vec3[] curve, float t, XIVColor XIVColor, int detail, float duration);
        public void DrawBezierWithT(XIVMemory<Vec3> curve, float t, XIVColor XIVColor, int detail, float duration);
        
        public void DrawBezierDetailed(XIVMemory<Vec3> curve, int detail = 100, float duration = 0f);

        // Spline
        public void DrawSpline(IList<Vec3> points, XIVColor XIVColor, int detail, float duration = 0f);

        public void DrawSplineWithT(IList<Vec3> points, XIVColor XIVColor, float t, int detail, float duration = 0f);

        // Sphere
        public void DrawSphere(Vec3 position, float radius, XIVColor xivColor, int detail, int circleDetail, float duration = 0);

        // Circle
        public void DrawCircle(Vec3 position, float radius, Vec3 axis, XIVColor xivColor, int detail, float duration = 0);

        // Bounds
        public void DrawBounds(Vec3 min, Vec3 max, float duration = 0f);

        // Rectangle
        public void DrawRectangle(Vec3 center, Vec3 halfExtents, XIVQuaternion orientation, float duration = 0f);

        // Text
        public void DrawText(Vec3 position, string text, int size, XIVColor XIVColor, float duration = 0f);

        public void DrawTextOnLine(Vec3 from, Vec3 to, string text, int size, XIVColor xivColor, float t, float duration);
    }
}