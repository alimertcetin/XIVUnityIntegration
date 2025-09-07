using System;
using UnityEditorInternal;
using XIV.Core.Utils;

namespace XIV.UnityEngineIntegration.XIVEditor.CodeGeneration
{
    public static class PhysicsConstantsGenerator
    {
        const string CLASS_NAME = "PhysicsConstants";

        public static string GetClassString()
        {
            ClassGenerator generator = new ClassGenerator(CLASS_NAME, classModifier: "static");
            generator.AddUsing(nameof(UnityEngine));

            var layers = InternalEditorUtility.layers;
            var layersLength = layers.Length;
            for (int i = 0; i < layersLength; i++)
            {
                var fieldName = CleanFieldName(layers[i]);
                var fieldValue = FormatStringFieldValue(layers[i]);
                generator.AddField(fieldName, fieldValue, "string", "const");
                generator.AddField(fieldName + "Layer", ToLayerMask(fieldValue), "int", "static readonly");

                layers[i] = fieldName; // we are using this later, reuse the same array
            }
            generator.AddMemberArray("All", layers, "string[]", "static readonly");
            return generator.EndClass();
        }

        static string FormatStringFieldValue(string value) => $"\"{value}\"";
        static string ToLayerMask(string field) => $"LayerMask.NameToLayer({field})";

        static string CleanFieldName(string fieldName)
        {
            // Replace spaces and other characters with underscores
            return fieldName.Replace(" ", "_").Replace("-", "_");
        }
    }
}