using UnityEngine;
using XIV.Core.TweenSystem.Drivers;

namespace XIV.Core.TweenSystem.RectTransformTweens
{
    internal sealed class RectTransformLocalMoveTween : TweenDriver<Vector3, RectTransform>
    {
        protected override void OnUpdate(float normalizedEasedTime)
        {
            component.localPosition = Vector3.Lerp(startValue, endValue, normalizedEasedTime);
        }
    }
}