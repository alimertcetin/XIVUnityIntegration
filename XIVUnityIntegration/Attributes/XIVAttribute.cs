using System;

namespace XIV.UnityEngineIntegration
{
    public abstract class XIVAttribute : Attribute
    {
        public abstract void StartDraw(object sender, string memberName);
        public abstract void Draw(object sender, string memberName);
        public abstract void EndDraw(object sender, string memberName);
    }
}