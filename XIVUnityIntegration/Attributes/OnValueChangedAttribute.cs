using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XIV.Core.Extensions;

namespace XIV.UnityEngineIntegration
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class OnValueChangedAttribute : XIVAttribute
    {
        class ValueTracker<T>
        {
            T currentValue;
            T newValue;
        
            public bool isChanged
            {
                get
                {
                    var res = currentValue.Equals(newValue) == false;
                    if (res) currentValue = newValue;
                    return res;
                }
            }

            public ValueTracker(T currentValue)
            {
                this.currentValue = Clone(currentValue);
                this.newValue = Clone(currentValue);
            }

            public void Assign(T val)
            {
                newValue = Clone(val);
            }

            static T Clone(T obj)
            {
                if (obj == null) return default(T);

                var type = obj.GetType();
                // Value types or strings are immutable and safe to use as-is
                if (type.IsValueType || type == typeof(string)) return obj;

                // Try ICloneable
                if (obj is ICloneable cloneable)
                    return (T)cloneable.Clone();

                // Try to use a parameterless constructor and copy properties
                var clone = Activator.CreateInstance(type);

                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (prop.CanRead && prop.CanWrite)
                    {
                        var value = prop.GetValue(obj);
                        prop.SetValue(clone, value);
                    }
                }

                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    var value = field.GetValue(obj);
                    field.SetValue(clone, value);
                }

                return (T)clone;
            }
        }

        static Dictionary<string, ValueTracker<object>> trackers = new();
        string functionToCall;
        readonly bool playModeOnly;
        
        public OnValueChangedAttribute(string functionToCall, bool playModeOnly = false) : base()
        {
            this.functionToCall = functionToCall;
            this.playModeOnly = playModeOnly;
        }

        public override void StartDraw(object sender, string memberName)
        {
            if (trackers.ContainsKey(memberName) == false)
            {
                var val = sender.GetType().XIVGetFieldOrPropertyValue(memberName, sender);
                trackers.Add(memberName, new ValueTracker<object>(val));
            }
        }

        public override void Draw(object sender, string memberName)
        {
            var val = sender.GetType().XIVGetFieldOrPropertyValue(memberName, sender);
            var tracker = trackers[memberName];
            tracker.Assign(val);
            if (tracker.isChanged)
            {
                if (this.playModeOnly && Application.isPlaying == false)
                {
                    Debug.LogWarning( $"{nameof(OnValueChangedAttribute)}: \"{functionToCall}\" is not allowed in editor mode");
                    return;
                }
                sender.GetType().XIVGetMethods().XIVFirstOrDefault(p => p.Name == functionToCall).Invoke(sender, Array.Empty<object>());
            }
        }

        public override void EndDraw(object sender, string memberName)
        {
        }
    }
}