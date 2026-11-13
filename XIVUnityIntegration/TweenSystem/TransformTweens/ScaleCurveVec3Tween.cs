using UnityEngine;
using XIV.Core.DataStructures;
using XIV.Core.TweenSystem.Drivers;
using XIV.Core.XIVMath;
using XIV.UnityIntegration.Extensions;

namespace XIV.Core.TweenSystem.TransformTweens
{
    internal sealed class ScaleCurveVec3Tween : CurveTweenDriver<Vec3, Transform>
    {
        protected override void OnUpdate(float easedTime)
        {
            var scale = BezierMath.GetPoint(startValue[0], startValue[1], startValue[2], startValue[3], easedTime);
            component.transform.localScale = scale.ToVector3();
        }
    }
}