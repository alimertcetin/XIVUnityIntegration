using UnityEngine;
using XIV.Core.TweenSystem.Drivers;
using XIV.Core.XIVMath;
using XIV.UnityIntegration.Extensions;

namespace XIV.Core.TweenSystem.TransformTweens
{
    internal sealed class ScaleCurveTween : CurveTweenDriver<Vector3, Transform>
    {
        protected override void OnUpdate(float easedTime)
        {
            var scale = BezierMath.GetPoint(startValue[0].ToVec3(), startValue[1].ToVec3(), startValue[2].ToVec3(), startValue[3].ToVec3(), easedTime);
            component.transform.localScale = scale.ToVector3();
        }
    }
}