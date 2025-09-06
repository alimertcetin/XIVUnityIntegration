using UnityEngine;
using XIV.Core.XIVMath;

namespace XIVUnityEngineIntegration.Extensions
{
    public static class Vector3Extension
    {
        public static Vector3 RotateAroundAxis(this Vector3 point, Vector3 axis, float angleDeg, Vector3 pivot)
        {
            var translatedPoint = point - pivot;
            var rotation = Quaternion.AngleAxis(angleDeg * XIVMathf.Rad2Deg, axis.normalized);
            var rotatedTranslatedPoint = rotation * translatedPoint;
            var rotatedPoint = rotatedTranslatedPoint + pivot;
            return rotatedPoint;
        }

        public static Vector3 RotateAroundX(this Vector3 point, float angleDeg, Vector3 pivot)
        {
            return RotateAroundAxis(point, Vector3.right, angleDeg, pivot);
        }

        public static Vector3 RotateAroundY(this Vector3 point, float angleDeg, Vector3 pivot)
        {
            return RotateAroundAxis(point, Vector3.up, angleDeg, pivot);
        }

        public static Vector3 RotateAroundZ(this Vector3 point, float angleDeg, Vector3 pivot)
        {
            return RotateAroundAxis(point, Vector3.forward, angleDeg, pivot);
        }

        public static Vector3 SetX(this Vector3 point, float value)
        {
            return new Vector3(value, point.y, point.z);
        }

        public static Vector3 SetY(this Vector3 point, float value)
        {
            return new Vector3(point.x, value, point.z);
        }

        public static Vector3 SetZ(this Vector3 point, float value)
        {
            return new Vector3(point.x, point.y, value);
        }

        public static Vector3 Abs(this Vector3 vec3)
        {
            return new Vector3(XIVMathf.Abs(vec3.x), XIVMathf.Abs(vec3.y), XIVMathf.Abs(vec3.z));
        }
    }
}