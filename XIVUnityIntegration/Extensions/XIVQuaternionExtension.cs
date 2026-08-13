using UnityEngine;
using XIV.Core.DataStructures;

namespace XIVUnityEngineIntegration.Extensions
{
    public static class XIVQuaternionExtension
    {
        public static Quaternion ToQuaternion(this XIVQuaternion q)
        {
            return new Quaternion(q.x, q.y, q.z, q.w);
        }
    }
}