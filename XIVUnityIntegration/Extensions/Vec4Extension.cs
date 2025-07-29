using UnityEngine;
using XIV.Core.DataStructures;

namespace XIVUnityEngineIntegration.Extensions
{
    public static class Vec4Extension
    {
        public static Vector4 ToVector4(this Vec4 v)
        {
            return new Vector4(v.x, v.y, v.z, v.w);
        }

        public static Color ToColor(this Vec4 v)
        {
            return new Color(v.x, v.y, v.z, v.w);
        }

        public static Vec4 ToVec4(this Vector4 v)
        {
            return new Vec4(v.x, v.y, v.z, v.w);
        }

        public static Vec4 ToVec4(this Color v)
        {
            return new Vec4(v.r, v.g, v.b, v.a);
        }
    }
}