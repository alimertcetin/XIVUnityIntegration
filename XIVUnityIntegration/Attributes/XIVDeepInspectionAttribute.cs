using System;

namespace XIV.UnityEngineIntegration
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XIVDeepInspectionAttribute : Attribute
    {
    }
}