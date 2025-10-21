using UnityEngine;
using XIV.Core.DataStructures;

namespace XIVUnityEngineIntegration.Extensions
{
    public static class Vec2Extensions
    {
        public static Vector2 ToVector2(this Vec2 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vec2 ToVec2(this Vector2 v)
        {
            return new Vec2(v.x, v.y);
        }
    }
}