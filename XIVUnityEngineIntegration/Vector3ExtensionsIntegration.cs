using XIV.Core.DataStructures;
using XIV.Core.XIVMath;

namespace XIV.UnityEngineIntegration
{
    public static class Vector3ExtensionsIntegration
    {
        public static Vec3 RotateAroundAxis(this Vec3 point, Vec3 axis, float angle, Vec3 pivot)
        {
            var translatedPoint = point - pivot;
            var rotation = Quaternion.AngleAxis(angle * XIVMathf.Rad2Deg, axis.normalized);
            var rotatedTranslatedPoint = rotation * translatedPoint;
            var rotatedPoint = rotatedTranslatedPoint + pivot;

            return rotatedPoint;
        }

        public static Vec3 RotateAroundX(this Vec3 point, float angle, Vec3 pivot)
        {
            return RotateAroundAxis(point, Vec3.right, angle, pivot);
        }

        public static Vec3 RotateAroundY(this Vec3 point, float angle, Vec3 pivot)
        {
            return RotateAroundAxis(point, Vec3.up, angle, pivot);
        }

        public static Vec3 RotateAroundZ(this Vec3 point, float angle, Vec3 pivot)
        {
            return RotateAroundAxis(point, Vec3.forward, angle, pivot);
        }
    }
}