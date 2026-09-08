using UnityEngine;
using XIV.Core.XIVMath;

namespace XIVUnityEngineIntegration.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 SetX(this Vector2 point, float value)
        {
            return new Vector2(value, point.y);
        }

        public static Vector2 SetY(this Vector2 point, float value)
        {
            return new Vector2(point.x, value);
        }

        public static Vector2 Abs(this Vector2 vec2)
        {
            return new Vector2(XIVMathf.Abs(vec2.x), XIVMathf.Abs(vec2.y));
        }

        public static bool IsNaN(this Vector2 vec2)
        {
            return float.IsNaN(vec2.x) || float.IsNaN(vec2.y);
        }
    }
}