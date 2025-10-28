using UnityEngine;
using XIV.Core.TweenSystem.Drivers;

namespace XIV.Core.TweenSystem.RendererTweens
{
    internal sealed class SpriteRendererColorTween : TweenDriver<Color, SpriteRenderer>
    {
        protected override void OnUpdate(float easedTime)
        {
            component.color = Color.Lerp(startValue, endValue, easedTime);
        }
    }
}