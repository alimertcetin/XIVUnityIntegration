using UnityEngine;

namespace XIVUnityEngineIntegration.Extensions
{
    public static class ColorExtensions
    {
        public static Color SetR(this Color color, float r)
        {
            return new Color(r, color.g, color.b, color.a);
        }

        public static Color SetG(this Color color, float g)
        {
            return new Color(color.r, g, color.b, color.a);
        }

        public static Color SetB(this Color color, float b)
        {
            return new Color(color.r, color.g, b, color.a);
        }
        
        public static Color SetA(this Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }
    }
}