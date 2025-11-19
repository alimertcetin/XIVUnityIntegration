using System;
using UnityEngine;
using XIV.Core.DataStructures;
using XIV.Core.Extensions;

namespace XIV.UnityEngineIntegration
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : XIVAttribute
    {
        static GUIStyle guiStyle;
        readonly bool playModeOnly;
        string label;

        public ButtonAttribute() : this("")
        {
            
        }

        public ButtonAttribute(bool playModeOnly) : this("", playModeOnly)
        {
            
        }

        public ButtonAttribute(string label, bool playModeOnly = false) : base()
        {
            this.label = label;
            this.playModeOnly = playModeOnly;
        }

        public override void StartDraw(object sender, string memberName)
        {
            guiStyle ??= new GUIStyle(GUI.skin.button) { richText = true };
        }

        public override void Draw(object sender, string memberName)
        {
            var type = sender.GetType();
            var method = type.XIVGetMethodByName(memberName);
            var buttonText = string.IsNullOrWhiteSpace(this.label) ? method.Name : this.label;
            
            if (GUILayout.Button(type.Name.XIVToColor(XIVColor.cyan) + ": " + buttonText, guiStyle))
            {
                if (this.playModeOnly && Application.isPlaying == false)
                {
                    Debug.LogWarning($"\"{buttonText}\" is not allowed in editor mode");
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