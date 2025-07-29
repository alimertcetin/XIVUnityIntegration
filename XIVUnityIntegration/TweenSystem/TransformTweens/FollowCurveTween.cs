using UnityEngine;
using XIV.Core.TweenSystem.Drivers;
using XIV.Core.XIVMath;
using XIVUnityEngineIntegration.Extensions;

namespace XIV.Core.TweenSystem.TransformTweens
{
    internal sealed class FollowCurveTween : CurveTweenDriver<Vector3, Transform>
    {
        protected override void OnUpdate(float easedTime)
        {
            var point = BezierMath.GetPoint(startValue[0].ToVec3(), startValue[1].ToVec3(), startValue[2].ToVec3(), startValue[3].ToVec3(), easedTime);
            component.position = point.ToVector3();
        }
    }
}