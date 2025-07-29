using UnityEngine;
using XIV.Core.DataStructures;

namespace XIVUnityEngineIntegration.Extensions
{
    public static class Vec3Extension
    {
        public static Vector3 ToVector3(this Vec3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vec3 ToVec3(this Vector3 v)
        {
            return new Vec3(v.x, v.y, v.z);
        }
    }
}