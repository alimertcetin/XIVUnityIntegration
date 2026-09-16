using System;

namespace XIV.UnityIntegration
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XIVDeepInspectionAttribute : Attribute
    {
    }
}