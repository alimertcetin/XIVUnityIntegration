using UnityEngine;

namespace XIV.Core.TweenSystem.RendererTweens
{
    internal sealed class MaterialPropertyBlockColorTween : MaterialPropertyBlockTween<Color>
    {
        protected override void OnUpdate(float easedTime)
        {
            component.GetPropertyBlock(mpb);
            mpb.SetColor(propId, Color.Lerp(startValue, endValue, easedTime));
            component.SetPropertyBlock(mpb);
        }
    }
}