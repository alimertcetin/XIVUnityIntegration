#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditorInternal;

namespace XIV.UnityEngineIntegration.XIVEditor
{
    [InitializeOnLoad]
    public static class ProfilerDefines
    {
        public const string PROFILER_ENABLED = "PROFILER_ENABLED";
        public const string DEEP_PROFILING_ENABLED = "DEEP_PROFILING_ENABLED";

        static bool enableProfilerSymbols
        {
            get => EditorPrefs.GetBool(nameof(enableProfilerSymbols));
            set => EditorPrefs.SetBool(nameof(enableProfilerSymbols), value);
        }
        
        static ProfilerDefines()
        {
            if (enableProfilerSymbols)
            {
                EditorApplication.update += OnEditorUpdate;
            }
        }
        
        public static void EnableProfilerSymbols()
        {
            enableProfilerSymbols = true;
            EditorApplication.update += OnEditorUpdate;
        }
        
        public static void DisableProfilerSymbols()
        {
            enableProfilerSymbols = false;
            EditorApplication.update -= OnEditorUpdate;
            RemoveDefines(DEEP_PROFILING_ENABLED, PROFILER_ENABLED);
        }
        
        static void OnEditorUpdate()
        {
            BuildTargetGroup currentTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (currentTarget == BuildTargetGroup.Unknown)
            {
                return;
            }

            bool isProfilerWindowOpen = EditorWindow.HasOpenInstances<ProfilerWindow>();
            if (isProfilerWindowOpen == false)
            {
                RemoveDefines(DEEP_PROFILING_ENABLED, PROFILER_ENABLED);
                
                return;
            }
            
            if (ProfilerDriver.enabled)
            {
                if (ProfilerDriver.deepProfiling)
                {
                    AddDefines(PROFILER_ENABLED, DEEP_PROFILING_ENABLED);
                    return;
                }

                RemoveDefine(currentTarget, DEEP_PROFILING_ENABLED);
                AddDefine(currentTarget, PROFILER_ENABLED);
            }
            else
            {
                RemoveDefines(DEEP_PROFILING_ENABLED, PROFILER_ENABLED);
            }
        }
        
        public static void AddDefines(params string[] definesToAdd)
        {
            var currentTarget = EditorUserBuildSettings.selectedBuildTargetGroup;

            if (currentTarget == BuildTargetGroup.Unknown)
            {
                return;
            }

            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            string[] defines = definesString.Split(';');

            bool changed = false;

            foreach (var define in definesToAdd)
            {
                if (Contains(defines, define)) continue;
                
                if (definesString.EndsWith(";", StringComparison.InvariantCulture) == false)
                {
                    definesString += ";";
                }

                definesString += define;
                changed = true;
            }

            if (changed)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
            }
        }
        
        public static void RemoveDefines(params string[] definesToRemove)
        {
            var currentTarget = EditorUserBuildSettings.selectedBuildTargetGroup;

            if (currentTarget == BuildTargetGroup.Unknown)
            {
                return;
            }

            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            string[] defines = definesString.Split(';');

            bool changed = false;

            foreach (var defineToRemove in definesToRemove)
            {
                if (Contains(defines, defineToRemove) == false) continue;
                
                definesString = definesString.Replace(defineToRemove, "");
                changed = true;
            }

            if (changed)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
            }
        }

        public static bool AddDefine(BuildTargetGroup currentTarget, string define)
        {
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            var defines = definesString.Split(';');
            
            if (Contains(defines, define)) return false;
                
            if (definesString.EndsWith(";", StringComparison.InvariantCulture) == false)
            {
                definesString += ";";
            }

            definesString += define;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
            
            return true;
        }

        public static bool RemoveDefine(BuildTargetGroup currentTarget, string define)
        {
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            var defines = definesString.Split(';');
            
            if (Contains(defines, define) == false) return false;
                
            if (definesString.EndsWith(";", StringComparison.InvariantCulture) == false)
            {
                definesString += ";";
            }

            definesString = definesString.Replace(define, "");
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
            return true;
        }

        static bool Contains<T>(T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Equals(value)) return true;
            }

            return false;
        }
    }
}
#endif