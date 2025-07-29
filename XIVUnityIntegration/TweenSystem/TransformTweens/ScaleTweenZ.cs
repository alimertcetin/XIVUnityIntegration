using UnityEngine;
using XIV.Core.TweenSystem.Drivers;
using XIVUnityEngineIntegration.Extensions;

namespace XIV.Core.TweenSystem.TransformTweens
{
    internal sealed class ScaleTweenZ : TweenDriver<float, Transform>
    {
        protected override void OnUpdate(float normalizedEasedTime)
        {
            component.localScale = component.localScale.SetZ(Mathf.Lerp(startValue, endValue, normalizedEasedTime));
        }
    }
}