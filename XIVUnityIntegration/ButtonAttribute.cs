using System;

namespace XIV.UnityEngineIntegration
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute
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
    }
}