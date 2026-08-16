using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XIV.Core.Extensions;

namespace XIV.UnityEngineIntegration.XIVEditor
{
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = true), CanEditMultipleObjects]
    public sealed class XIVDefaultEditor : Editor
    {
        static readonly Queue<List<AttributeData>> AttributeListPool = new();
        static readonly Dictionary<string, string> BreadcrumbCache = new();

        List<AttributeData> cachedAttributes;
        readonly Dictionary<string, List<AttributeData>> groupedAttributes = new();
        readonly List<string> groupKeys = new();
        readonly Dictionary<string, bool> foldoutStates = new();

        string searchQuery = string.Empty;

        struct AttributeData
        {
            public object sender;
            public MemberInfo owner;
            public XIVAttribute attribute;
            public string groupName;
        }

        // ---------------------------------------------------------------------
        // Reflection Cache
        // ---------------------------------------------------------------------

        static class ReflectionCache
        {
            public struct MemberMeta
            {
                public MemberInfo member;
                public XIVAttribute[] attributeTemplates;
                public Func<object, object> getter;

                public bool isUnityEngineAssembly;
                public bool isDeepInspection;
                public bool isSerializable;
                public bool canTraverse;
            }

            static readonly Dictionary<Type, MemberMeta[]> TypeCache = new();

            public static MemberMeta[] GetMetadata(Type type)
            {
                if (TypeCache.TryGetValue(type, out var metadata))
                    return metadata;

                var members = type.XIVGetMembers();
                int count = members.Length;
                metadata = new MemberMeta[count];

                for (int i = 0; i < count; i++)
                {
                    MemberInfo member = members[i];
                    Type memberType = GetMemberType(member);

                    bool isUnityEngineAssembly =
                        member.DeclaringType != null &&
                        member.DeclaringType.Assembly.GetName().Name.Contains(nameof(UnityEngine));

                    bool isDeepInspection =
                        member.IsDefined(typeof(XIVDeepInspectionAttribute), inherit: true);

                    bool isSerializable = IsSerializableField(member, memberType);

                    bool canTraverse = CanTraverse(memberType);

                    Func<object, object> getter = null;
                    if (canTraverse)
                    {
                        getter = CreateGetter(member);
                    }

                    var rawAttrs = (XIVAttribute[])member.GetCustomAttributes(typeof(XIVAttribute), inherit: true);

                    metadata[i] = new MemberMeta
                    {
                        member = member,
                        attributeTemplates = rawAttrs,
                        getter = getter,
                        isUnityEngineAssembly = isUnityEngineAssembly,
                        isDeepInspection = isDeepInspection,
                        isSerializable = isSerializable,
                        canTraverse = canTraverse
                    };
                }

                TypeCache[type] = metadata;
                return metadata;
            }

            static Type GetMemberType(MemberInfo member)
            {
                return member.MemberType switch
                {
                    MemberTypes.Field => ((FieldInfo)member).FieldType,
                    MemberTypes.Property => ((PropertyInfo)member).PropertyType,
                    _ => null
                };
            }

            static bool IsSerializableField(MemberInfo member, Type memberType)
            {
                // Only fields carry Unity/System.Serializable semantics for struct/class traversal.
                if (member is not FieldInfo || memberType == null) return false;

                return memberType.IsDefined(typeof(SerializableAttribute), inherit: false);
            }

            static bool CanTraverse(Type type)
            {
                if (type == null) return false;
                if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type.IsPointer) return false;
                if (typeof(Delegate).IsAssignableFrom(type)) return false;
                if (typeof(UnityEngine.Object).IsAssignableFrom(type)) return false;
                if (type.IsArray) return false;

                return true;
            }

            static Func<object, object> CreateGetter(MemberInfo member)
            {
                try
                {
                    var instance = Expression.Parameter(typeof(object), "instance");

                    if (member is FieldInfo field)
                    {
                        if (field.IsStatic)
                        {
                            var access = Expression.Field(null, field);
                            return Expression.Lambda<Func<object, object>>(
                                Expression.Convert(access, typeof(object)), instance).Compile();
                        }

                        var castInstance = Expression.Convert(instance, field.DeclaringType);
                        var fieldAccess = Expression.Field(castInstance, field);
                        return Expression.Lambda<Func<object, object>>(
                            Expression.Convert(fieldAccess, typeof(object)), instance).Compile();
                    }

                    if (member is PropertyInfo property)
                    {
                        if (!property.CanRead || property.GetIndexParameters().Length != 0) return null;
                        MethodInfo getterMethod = property.GetMethod;
                        if (getterMethod == null) return null;

                        if (getterMethod.IsStatic)
                        {
                            var access = Expression.Property(null, property);
                            return Expression.Lambda<Func<object, object>>(
                                Expression.Convert(access, typeof(object)), instance).Compile();
                        }

                        var castInstance = Expression.Convert(instance, property.DeclaringType);
                        var propertyAccess = Expression.Property(castInstance, property);
                        return Expression.Lambda<Func<object, object>>(
                            Expression.Convert(propertyAccess, typeof(object)), instance).Compile();
                    }
                }
                catch
                {
                    // Fallback to standard reflection if Expression compilation fails
                }

                if (member is FieldInfo fallbackField)
                {
                    return fallbackField.IsStatic
                        ? _ => fallbackField.GetValue(null)
                        : inst => fallbackField.GetValue(inst);
                }

                if (member is PropertyInfo fallbackProperty && fallbackProperty.CanRead && fallbackProperty.GetIndexParameters().Length == 0)
                {
                    return inst => fallbackProperty.GetValue(inst, null);
                }

                return null;
            }
        }

        // ---------------------------------------------------------------------
        // Unity Lifecycle
        // ---------------------------------------------------------------------

        void OnEnable()
        {
            if (!AttributeListPool.TryDequeue(out cachedAttributes))
            {
                cachedAttributes = new List<AttributeData>(16);
            }

            CacheAttributes();
        }

        void OnDisable()
        {
            if (cachedAttributes == null) return;

            cachedAttributes.Clear();
            AttributeListPool.Enqueue(cachedAttributes);
            cachedAttributes = null;

            ClearGroupedData();
        }

        // ---------------------------------------------------------------------
        // Inspector GUI
        // ---------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            if (target == null) return;

            base.OnInspectorGUI();

            if (cachedAttributes.Count == 0) return;

            DrawDivider();

            if (cachedAttributes.Count > 5)
            {
                DrawSearchBar();
            }

            DrawAttributes();
        }

        void DrawSearchBar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            searchQuery = EditorGUILayout.TextField(searchQuery, EditorStyles.toolbarSearchField);

            if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), EditorStyles.toolbarButton, GUILayout.Width(25)))
            {
                searchQuery = string.Empty;
                GUI.FocusControl(null);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
        }

        // ---------------------------------------------------------------------
        // Caching
        // ---------------------------------------------------------------------

        void CacheAttributes()
        {
            cachedAttributes.Clear();
            var visited = new HashSet<object>();

            FillAttributes(target, cachedAttributes, visited);
            RebuildGroupedData();
        }

        void RebuildGroupedData()
        {
            ClearGroupedData();

            int count = cachedAttributes.Count;
            for (int i = 0; i < count; i++)
            {
                var data = cachedAttributes[i];
                string groupKey = data.groupName ?? string.Empty;

                if (!groupedAttributes.TryGetValue(groupKey, out var list))
                {
                    list = new List<AttributeData>(4);
                    groupedAttributes.Add(groupKey, list);
                    groupKeys.Add(groupKey);
                }

                list.Add(data);
            }
        }

        void ClearGroupedData()
        {
            foreach (var pair in groupedAttributes)
            {
                pair.Value.Clear();
            }
            groupKeys.Clear();
        }

        // ---------------------------------------------------------------------
        // Recursive Inspection
        // ---------------------------------------------------------------------

        /// <summary>
        /// Inspection Rules:
        /// 1. Every member's direct XIV attributes are collected.
        /// 2. A member's value is entered when:
        ///      - deep inspection is already active, OR
        ///      - the member has [XIVDeepInspection], OR
        ///      - the member's target type is [Serializable].
        /// 3. [Serializable] allows inspection of that object's direct fields ONLY.
        ///    It does NOT propagate deep inspection recursively.
        /// 4. [XIVDeepInspection] activates recursive inspection for the entire descendant branch.
        /// </summary>
        static void FillAttributes(
            object sender,
            List<AttributeData> attributes,
            HashSet<object> visited,
            string parentPath = null,
            bool deepInspectionActive = false)
        {
            if (sender == null) return;

            Type senderType = sender.GetType();

            // Path-local cycle detection
            if (!senderType.IsValueType && !visited.Add(sender)) return;

            try
            {
                var metadata = ReflectionCache.GetMetadata(senderType);
                int count = metadata.Length;

                for (int i = 0; i < count; i++)
                {
                    ref var meta = ref metadata[i];

                    if (meta.isUnityEngineAssembly) continue;

                    // Always inspect XIV attributes on this specific member
                    int attributeCount = meta.attributeTemplates.Length;
                    for (int a = 0; a < attributeCount; a++)
                    {
                        attributes.Add(new AttributeData
                        {
                            sender = sender,
                            owner = meta.member,
                            attribute = meta.attributeTemplates[a],
                            groupName = parentPath
                        });
                    }

                    // Decide whether we enter this member's value
                    bool shouldInspect =
                        deepInspectionActive ||
                        meta.isDeepInspection ||
                        meta.isSerializable;

                    if (!shouldInspect || !meta.canTraverse || meta.getter == null)
                    {
                        continue;
                    }

                    object childSender = meta.getter(sender);
                    if (childSender == null) continue;

                    // [Serializable] opens ONE level.
                    // [XIVDeepInspection] opens the entire descendant branch.
                    bool childDeepInspection =
                        deepInspectionActive ||
                        meta.isDeepInspection;

                    string fieldPath = string.IsNullOrEmpty(parentPath)
                        ? meta.member.Name
                        : $"{parentPath}.{meta.member.Name}";

                    FillAttributes(
                        childSender,
                        attributes,
                        visited,
                        fieldPath,
                        childDeepInspection);
                }
            }
            finally
            {
                if (!senderType.IsValueType)
                {
                    visited.Remove(sender);
                }
            }
        }

        // ---------------------------------------------------------------------
        // Drawing
        // ---------------------------------------------------------------------

        void DrawAttributes()
        {
            bool hasSearch = !string.IsNullOrWhiteSpace(searchQuery);
            int groupCount = groupKeys.Count;

            for (int i = 0; i < groupCount; i++)
            {
                string groupKey = groupKeys[i];
                var list = groupedAttributes[groupKey];
                int totalCount = list.Count;
                int matchingCount = totalCount;

                if (hasSearch)
                {
                    matchingCount = 0;
                    for (int j = 0; j < totalCount; j++)
                    {
                        if (MatchesSearch(list[j], groupKey, searchQuery))
                        {
                            matchingCount++;
                        }
                    }

                    if (matchingCount == 0) continue;
                }

                if (string.IsNullOrEmpty(groupKey))
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    for (int j = 0; j < totalCount; j++)
                    {
                        var data = list[j];
                        if (hasSearch && !MatchesSearch(data, groupKey, searchQuery)) continue;
                        DrawSingleAttribute(data);
                    }
                    EditorGUILayout.EndVertical();
                    continue;
                }

                string displayTitle = FormatBreadcrumbTitleCached(groupKey);

                if (!foldoutStates.TryGetValue(groupKey, out bool isOpen))
                {
                    isOpen = true;
                    foldoutStates[groupKey] = true;
                }

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                string header = $"{displayTitle} ({matchingCount})";

                foldoutStates[groupKey] = EditorGUILayout.BeginFoldoutHeaderGroup(isOpen, header);

                if (foldoutStates[groupKey])
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Space(2);

                    for (int j = 0; j < totalCount; j++)
                    {
                        var data = list[j];
                        if (hasSearch && !MatchesSearch(data, groupKey, searchQuery)) continue;
                        DrawSingleAttribute(data);
                    }

                    EditorGUILayout.Space(2);
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(2);
            }
        }

        static bool MatchesSearch(AttributeData data, string groupKey, string query)
        {
            if (data.owner.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) return true;
            return !string.IsNullOrEmpty(groupKey) && groupKey.Contains(query, StringComparison.OrdinalIgnoreCase);
        }

        static void DrawSingleAttribute(AttributeData data)
        {
            string memberName = data.owner.Name;
            XIVAttribute attribute = data.attribute;

            attribute.StartDraw(data.sender, memberName);
            attribute.Draw(data.sender, memberName);
            attribute.EndDraw(data.sender, memberName);
        }

        // ---------------------------------------------------------------------
        // UI Helpers
        // ---------------------------------------------------------------------

        static string FormatBreadcrumbTitleCached(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            if (BreadcrumbCache.TryGetValue(path, out string cached)) return cached;

            string[] parts = path.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = ObjectNames.NicifyVariableName(parts[i]);
            }

            string formatted = string.Join("  ►  ", parts);
            BreadcrumbCache[path] = formatted;
            return formatted;
        }

        static void DrawDivider()
        {
            EditorGUILayout.Space(8);
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(rect, new Color(0.3f, 0.3f, 0.3f, 0.5f));
            EditorGUILayout.Space(6);
        }
    }
}