using UnityEngine;
using XIV.Core.Utils;
using XIV.Core.DataStructures;

namespace XIV.Core.TweenSystem.Drivers
{
    internal abstract class CurveTweenDriver<TValueType, TComponentType> : TweenDriver<XIVMemory<TValueType>, TComponentType> where TComponentType : Component
    {
        internal CurveTweenDriver<TValueType, TComponentType> Set(TComponentType component, XIVMemory<TValueType> values, float duration, EasingFunction.Function easing, bool isPingPong = false, int loopCount = 0)
        {
            base.Set(component, values, values.reversed, duration, easing, isPingPong, loopCount);
            return this;
        }
    }
}