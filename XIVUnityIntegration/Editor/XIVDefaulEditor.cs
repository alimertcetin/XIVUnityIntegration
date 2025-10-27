using System;
using System.Collections.Generic;
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
        
        static Queue<Dictionary<object, List<AttributeData>>> pool;

        struct AttributeData
        {
            public MemberInfo owner;
            public XIVAttribute attribute;
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
                foreach (var attributeData in attributeDataList)
                {
                    var memberName = attributeData.owner.Name;
                    var attribute = attributeData.attribute;
                    attribute.StartDraw(sender, memberName);
                    attribute.Draw(sender, memberName);
                    attribute.EndDraw(sender, memberName);
                }
            }
        }

        static void FillAttributes(object sender, XIVMemory<MemberInfo> members, Dictionary<object, List<AttributeData>> attributes)
        {
            static AttributeData CreateAttributeData(MemberInfo member, XIVAttribute xivAttribute)
            {
                return new AttributeData
                {
                    owner = member,
                    attribute = xivAttribute,
                };
            }
            
            int length = members.Length;
            for (var i = 0; i < length; i++)
            {
                var member = members[i];
                foreach (var customAttribute in member.GetCustomAttributes<XIVAttribute>())
                {
                    if (attributes.TryGetValue(sender, out var list))
                    {
                        list.Add(CreateAttributeData(member, customAttribute));
                        continue;
                    }

                    attributes.Add(sender, new List<AttributeData>() { CreateAttributeData(member, customAttribute) });
                }

                object otherSender;
                // rigidbody property throws exception.
                try
                {
                    otherSender = sender.GetType().XIVGetFieldOrPropertyValue(member.Name, sender);
                }
                catch (Exception e)
                {
                    continue;
                }

                if (otherSender == null) continue;
                var others = member.MemberType == MemberTypes.Field ? ((FieldInfo)member).FieldType.XIVGetMembersHasAttribute<XIVAttribute>() : member.MemberType == MemberTypes.Property ? ((PropertyInfo)member).PropertyType.XIVGetMembersHasAttribute<XIVAttribute>() : default;
                FillAttributes(otherSender, others, attributes);
            }
        }
    }
}