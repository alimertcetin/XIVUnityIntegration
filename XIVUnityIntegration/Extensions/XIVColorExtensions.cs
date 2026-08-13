using UnityEngine;
using XIV.Core.DataStructures;

namespace XIVUnityEngineIntegration.Extensions
{
    public static class XIVColorExtensions
    {
        public static XIVColor AsXIVColor(this Vec3 vec3)
        {
            return new XIVColor(vec3.x, vec3.y, vec3.z);
        }

        public static Color ToUnityColor(this XIVColor xivColor)
        {
            return new Color(xivColor.r, xivColor.g, xivColor.b, xivColor.a);
        }

        public static XIVColor ToXIVColor(this Color color)
        {
            return new XIVColor(color.r, color.g, color.b, color.a);
        }
    }
}