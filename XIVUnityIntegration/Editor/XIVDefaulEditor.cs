using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using XIV.Core.DataStructures;
using XIV.Core.Extensions;

namespace XIV.UnityEngineIntegration.XIVEditor
{
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = true), CanEditMultipleObjects]
    public class XIVDefaulEditor : Editor
    {
        Dictionary<object, List<AttributeData>> attributes;
        Dictionary<string, bool> foldoutStates = new();
        
        static Queue<Dictionary<object, List<AttributeData>>> pool;

        struct AttributeData
        {
            public MemberInfo owner;
            public XIVAttribute attribute;
            public string fieldPath;
        }
            
        void OnEnable()
        {
            if (pool == null) pool = new Queue<Dictionary<object, List<AttributeData>>>();
            if (pool.TryDequeue(out attributes) == false) attributes = new();
            CacheAttributes();
        }

        public override void OnInspectorGUI()
        {
            // otherwise base.OnInspectorGUI throws NullReferenceException
            if (target == false) return;

            base.OnInspectorGUI();
            DrawAttributes();
        }

        void OnDisable()
        {
            pool.Enqueue(attributes);
        }

        void CacheAttributes()
        {
            attributes.Clear();
            FillAttributes(target, target.GetType().XIVGetMembers(), attributes);
        }

        void DrawAttributes()
        {
            foreach (var kvp in attributes)
            {
                var sender = kvp.Key;
                var attributeDataList = kvp.Value;

                // Group attributes by fieldPath
                var grouped = attributeDataList.GroupBy(a => a.fieldPath.Split('.')[0]);

                foreach (var group in grouped)
                {
                    var fieldPath = group.Key;

                    // Ensure we have a foldout state entry
                    if (!foldoutStates.TryGetValue(fieldPath, out bool isOpen))
                        foldoutStates[fieldPath] = isOpen = false;

                    // Draw foldout header
                    foldoutStates[fieldPath] = EditorGUILayout.Foldout(isOpen, fieldPath, true);

                    if (foldoutStates[fieldPath])
                    {
                        EditorGUI.indentLevel++;
                        foreach (var attributeData in group)
                        {
                            var memberName = attributeData.owner.Name;
                            var attribute = attributeData.attribute;

                            attribute.StartDraw(sender, memberName);
                            attribute.Draw(sender, memberName);
                            attribute.EndDraw(sender, memberName);
                        }
                        EditorGUI.indentLevel--;
                        EditorGUILayout.Space();
                    }
                }
            }
        }


        static void FillAttributes(object sender, XIVMemory<MemberInfo> members, Dictionary<object, List<AttributeData>> attributes, string parentPath = null)
        {
            static AttributeData CreateAttributeData(MemberInfo member, XIVAttribute xivAttribute, string fieldPath)
            {
                return new AttributeData
                {
                    owner = member,
                    attribute = xivAttribute,
                    fieldPath = fieldPath,
                };
            }
            
            int length = members.Length;
            for (var i = 0; i < length; i++)
            {
                var member = members[i];
                // rigidbody property throws exception.
                // MeshFilter.mesh logs error - Not allowed to access MeshFilter.mesh on Prefab object. Use MeshFilter.sharedMesh instead
                // So we will just skip any UnityEngine types
                if (member.DeclaringType.Assembly.GetName().Name.Contains(nameof(UnityEngine))) continue;
                
                var fieldPath = string.IsNullOrEmpty(parentPath)
                    ? member.Name
                    : $"{parentPath}.{member.Name}";
                
                foreach (var customAttribute in member.GetCustomAttributes<XIVAttribute>())
                {
                    if (attributes.TryGetValue(sender, out var list))
                    {
                        list.Add(CreateAttributeData(member, customAttribute, fieldPath));
                        continue;
                    }

                    attributes.Add(sender, new List<AttributeData>() { CreateAttributeData(member, customAttribute, fieldPath) });
                }

                var otherSender = sender.GetType().XIVGetFieldOrPropertyValue(member.Name, sender);

                if (otherSender == null) continue;
                var others = member.MemberType == MemberTypes.Field ? ((FieldInfo)member).FieldType.XIVGetMembersHasAttribute<XIVAttribute>() : member.MemberType == MemberTypes.Property ? ((PropertyInfo)member).PropertyType.XIVGetMembersHasAttribute<XIVAttribute>() : default;
                FillAttributes(otherSender, others, attributes, fieldPath);
            }
        }
    }
}