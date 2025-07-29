using UnityEngine;
using UnityEngine.UI;
using XIV.Core.Extensions;
using XIV.Core.TweenSystem.Drivers;
using XIVUnityEngineIntegration.Extensions;

namespace XIV.Core.TweenSystem.ImageTweens
{
    internal sealed class ImageAlphaTween : TweenDriver<float, Image>
    {
        protected override void OnUpdate(float easedTime)
        {
            component.color = component.color.SetA(Mathf.Lerp(startValue, endValue, easedTime));
        }
    }
}