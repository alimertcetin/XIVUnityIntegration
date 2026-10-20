using UnityEngine;
using XIV.Core.TweenSystem.Drivers;

namespace XIV.Core.TweenSystem.RectTransformTweens
{
    internal sealed class RectTransformAnchoredPosition3D : TweenDriver<Vector3, RectTransform>
    {
        protected override void OnUpdate(float normalizedEasedTime)
        {
            component.anchoredPosition3D = Vector3.Lerp(startValue, endValue, normalizedEasedTime);
        }
    }
}