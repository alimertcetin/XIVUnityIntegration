using UnityEngine;
using UnityEngine.UI;
using XIV.Core.TweenSystem.Drivers;
using XIV.Core.XIVMath;
using XIVUnityEngineIntegration.Extensions;

namespace XIV.Core.TweenSystem.ImageTweens
{
    internal sealed class ImageColorCurveTween : CurveTweenDriver<Color, Image>
    {
        protected override void OnUpdate(float easedTime)
        {
            var color = BezierMath4D.GetPoint(startValue[0].ToVec4(), startValue[1].ToVec4(), startValue[2].ToVec4(), startValue[3].ToVec4(), easedTime);
            component.color = color.ToVector4();
        }
    }
}