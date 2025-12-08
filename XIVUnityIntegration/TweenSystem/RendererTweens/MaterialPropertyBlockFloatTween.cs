using XIV.Core.XIVMath;

namespace XIV.Core.TweenSystem.RendererTweens
{
    internal sealed class MaterialPropertyBlockFloatTween : MaterialPropertyBlockTween<float>
    {
        protected override void OnUpdate(float easedTime)
        {
            component.GetPropertyBlock(mpb);
            mpb.SetFloat(propId, XIVMathf.Lerp(startValue, endValue, easedTime));
            component.SetPropertyBlock(mpb);
        }
    }
}