using UnityEngine;
using XIV.Core.DataStructures;
using XIV.Core.XIVMath;

namespace XIV.UnityEngineIntegration
{
    public static class CamFrustumUtils
    {
        public static Vec3 GetFrustum(float distance)
        {
            var cam = Camera.main;
            return FrustumMath.GetFrustum(distance, cam.fieldOfView, cam.aspect);
        }

        public static Vec3 GetFrustum(Camera cam, float distance)
        {
            return FrustumMath.GetFrustum(distance, cam.fieldOfView, cam.aspect);
        }
    }
}