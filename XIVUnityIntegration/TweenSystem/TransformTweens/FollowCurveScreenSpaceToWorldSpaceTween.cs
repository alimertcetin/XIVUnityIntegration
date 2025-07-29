using UnityEngine;
using XIV.Core.TweenSystem.Drivers;
using XIV.Core.XIVMath;
using XIVUnityEngineIntegration.Extensions;

namespace XIV.Core.TweenSystem.TransformTweens
{
    internal sealed class FollowCurveScreenSpaceToWorldSpaceTween : CurveTweenDriver<Vector3, Transform>
    {
        public Camera cam;
        
        protected override void OnUpdate(float easedTime)
        {
            var point = cam.ScreenToWorldPoint(BezierMath.GetPoint(startValue[0].ToVec3(), startValue[1].ToVec3(), startValue[2].ToVec3(), startValue[3].ToVec3(), easedTime).ToVector3());
            component.position = point;
        }
    }
}