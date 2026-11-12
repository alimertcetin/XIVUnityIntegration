using System;

namespace XIV.Core.TweenSystem.OtherTweens
{
    internal sealed class OnFinallyCallbackTween : CallbackTween
    {
        Action action;

        public OnFinallyCallbackTween Set(Action action)
        {
            this.action = action;
            return this;
        }
        
        public override void Complete() => action.Invoke();
        public override void Cancel() => action.Invoke();
    }
    
    internal sealed class OnFinallyCallbackTween<T> : CallbackTween
    {
        Action<T> action;
        T value;
        
        public OnFinallyCallbackTween<T> Set(Action<T> action, T value)
        {
            this.action = action;
            this.value = value;
            return this;
        }
        
        public override void Complete() => action.Invoke(value);
        public override void Cancel() => action.Invoke(value);
    }
}