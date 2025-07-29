using UnityEngine;
using XIV.Core.TweenSystem.Drivers;
using XIV.Core.XIVMath;
using XIVUnityEngineIntegration.Extensions;

namespace XIV.Core.TweenSystem.RendererTweens
{
    internal sealed class RendererColorCurveTween : CurveTweenDriver<Color, Renderer>
    {
        protected override void OnUpdate(float easedTime)
        {
            var color = BezierMath4D.GetPoint(startValue[0].ToVec4(), startValue[1].ToVec4(), startValue[2].ToVec4(), startValue[3].ToVec4(), easedTime);
            component.material.color = color.ToVector4();
        }
    }
}