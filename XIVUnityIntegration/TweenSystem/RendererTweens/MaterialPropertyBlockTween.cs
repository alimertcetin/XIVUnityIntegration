using UnityEngine;
using XIV.Core.TweenSystem.Drivers;
using XIV.Core.Utils;

namespace XIV.Core.TweenSystem.RendererTweens
{
    internal abstract class MaterialPropertyBlockTween<TValueType> : TweenDriver<TValueType, Renderer>
    {
        protected MaterialPropertyBlock mpb;
        protected int propId;
        
        public MaterialPropertyBlockTween<TValueType> Set(MaterialPropertyBlock mpb, int propId, Renderer component, TValueType startValue, TValueType endValue, float duration, EasingFunction.Function easingFunction, bool isPingPong = false, int loopCount = 0)
        {
            this.mpb = mpb;
            this.propId = propId;
            base.Set(component, startValue, endValue, duration, easingFunction, isPingPong, loopCount);
            return this;
        }

    }
}