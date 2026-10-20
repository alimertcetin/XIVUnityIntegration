using UnityEngine;
using XIV.Core.TweenSystem.Drivers;

namespace XIV.Core.TweenSystem.RectTransformTweens
{
    internal sealed class RectTransformSizeDelta : TweenDriver<Vector2, RectTransform>
    {
        protected override void OnUpdate(float normalizedEasedTime)
        {
            component.sizeDelta = Vector2.Lerp(startValue, endValue, normalizedEasedTime);
        }
    }
}