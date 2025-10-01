using System;
using UnityEngine;
using XIV.Core.Extensions;

namespace XIV.UnityEngineIntegration
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : XIVAttribute
    {
        public string label;
        public bool playModeOnly;

        public ButtonAttribute() : this("")
        {
            
        }

        public ButtonAttribute(bool playModeOnly) : this("", playModeOnly)
        {
            
        }

        public ButtonAttribute(string label, bool playModeOnly = false)
        {
            this.label = label;
            this.playModeOnly = playModeOnly;
        }

        public override void StartDraw(object sender, string memberName)
        {
        }

        public override void Draw(object sender, string memberName)
        {
            var method = sender.GetType().XIVGetMethodByName(memberName);
            var buttonText = string.IsNullOrWhiteSpace(this.label) ? method.Name : this.label;
            if (GUILayout.Button(buttonText))
            {
                if (this.playModeOnly && Application.isPlaying == false)
                {
                    Debug.LogWarning( $"\"{buttonText}\" is not allowed in editor mode");
                    return;
                }
                method.Invoke(sender, Array.Empty<object>());
            }
        }

        public override void EndDraw(object sender, string memberName)
        {
        }
    }
}