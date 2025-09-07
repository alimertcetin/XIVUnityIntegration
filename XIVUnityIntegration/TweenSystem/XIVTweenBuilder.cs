using System.Collections.Generic;
using System;
using UnityEngine;
using XIV.PoolSystem;

namespace XIV.Core.TweenSystem
{
    public sealed partial class XIVTweenBuilder
    {
        static readonly XIVTweenBuilder instance = new XIVTweenBuilder();
        static readonly List<TweenTimeline> timelineBuffer = new List<TweenTimeline>(4);

        Component component;
        TweenTimeline currentTimeline;
        int componentInstanceID;
        bool useCurrent;

        public static XIVTweenBuilder GetBuilder(Component component)
        {
            instance.Initialize(component);
            return instance;
        }

        public XIVTweenBuilder AddTween(ITween tween)
        {
            if (useCurrent == false)
            {
                timelineBuffer.Add(currentTimeline);
                currentTimeline = XIVTweenSystem.GetTimeline(tween);
                return this;
            }

            currentTimeline.AddTween(tween);
            useCurrent = false;
            return this;
        }

        public void Start()
        {
            timelineBuffer.Add(currentTimeline);
            int count = timelineBuffer.Count;
            for (int i = 0; i < count; i++)
            {
                XIVTweenSystem.AddTween(componentInstanceID, timelineBuffer[i]);
            }
            Reset();
        }

        public XIVTweenBuilder And()
        {
            useCurrent = true;
            return this;
        }

        public XIVTweenBuilder UseUnscaledDeltaTime()
        {
            currentTimeline.SetDeltaTimeFunc(TweenTimeline.unscaledDeltaTimeFunc);
            return this;
        }

        public XIVTweenBuilder UseCustomDeltaTime(Func<float> customDtFunc)
        {
            currentTimeline.SetDeltaTimeFunc(customDtFunc);
            return this;
        }
        
        /// <summary>
        /// Gets <typeparamref name="T"/> tween from pool
        /// </summary>
        static T Get<T>() where T : ITween
        {
            return XIVPoolSystem.GetItem<T>();
        }

        void Initialize(Component component)
        {
            this.component = component;
            this.componentInstanceID = component.gameObject.GetInstanceID();
            this.currentTimeline = XIVTweenSystem.GetTimeline();
            useCurrent = true;
        }

        void Reset()
        {
            component = default;
            currentTimeline = default;
            timelineBuffer.Clear();
        }

        bool TryGetComponent<T>(out T comp) where T : Component
        {
            comp = component as T;
            if (comp == default) comp = component.GetComponent<T>();
            return comp != default;
        }

        void ThrowCastError(Type type)
        {
            throw new InvalidCastException("Cannot cast " + component.GetType() + " to " + type);
        }
    }
}