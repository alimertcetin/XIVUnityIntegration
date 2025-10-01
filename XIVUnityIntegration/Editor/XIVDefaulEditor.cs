using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XIV.Core.Extensions;
using XIV.Core.Utils;
using Object = UnityEngine.Object;

namespace XIV.UnityEngineIntegration.XIVEditor
{
    [CustomEditor(typeof(Object), true, isFallback = true), CanEditMultipleObjects]
    public class XIVDefaulEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // otherwise base.OnInspectorGUI throws NullReferenceException
            if (target == false) return;

            base.OnInspectorGUI();
            
            var members = target.GetType().XIVGetMembersHasAttribute<XIVAttribute>();
            int length = members.Length;
            for (var i = 0; i < length; i++)
            {
                var member = members[i];
                foreach (var customAttribute in member.GetCustomAttributes<XIVAttribute>())
                {
                    customAttribute.StartDraw(target, member.Name);
                    customAttribute.Draw(target, member.Name);
                    customAttribute.EndDraw(target, member.Name);
                }
            }
        }
    }
}