using UnityEngine;
using XIV.Core.TweenSystem.Drivers;

namespace XIV.Core.TweenSystem.ImageTweens
{
    internal sealed class CanvasGroupAlphaTween : TweenDriver<float, CanvasGroup>
    {
        protected override void OnUpdate(float easedTime)
        {
            component.alpha = Mathf.Lerp(startValue, endValue, easedTime);
        }
    }
}