using UnityEngine;
using XIV.Core.DataStructures;
using XIV.Core.TweenSystem.Drivers;
using XIV.Core.XIVMath;

namespace XIV.Core.TweenSystem.TransformTweens
{
    internal sealed class FollowCurveAndRotationVec3Tween : CurveTweenDriver<Vec3, Transform>
    {
        protected override void OnUpdate(float easedTime)
        {
            // var point = BezierMath.GetPoint(startValue[0], startValue[1], startValue[2], startValue[3], easedTime);
            // component.position = point.ToVector3();
            var curveData = BezierMath.GetCurveData(startValue, easedTime);
            component.position = curveData.point;
            component.rotation = Quaternion.LookRotation(curveData.forward, curveData.normal);
        }
    }
}