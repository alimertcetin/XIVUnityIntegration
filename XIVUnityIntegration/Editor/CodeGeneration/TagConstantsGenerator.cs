using UnityEditorInternal;
using XIV.Core.Utils;

namespace XIV.UnityEngineIntegration.XIVEditor.CodeGeneration
{
    public static class TagConstantsGenerator
    {
        const string CLASS_NAME = "TagConstants";

        public static string GetClassString()
        {
            ClassGenerator generator = new ClassGenerator(CLASS_NAME, classModifier: "static");

            var tags = InternalEditorUtility.tags;

            for (int i = 0; i < tags.Length; i++)
            {
                var fieldName = CleanFieldName(tags[i]);
                var fieldValue = FormatStringFieldValue(tags[i]);
                generator.AddField(fieldName, fieldValue, "string", "const");
            }
            generator.AddMemberArray("All", tags, "string[]", "static readonly");

            return generator.EndClass();
        }

        static string FormatStringFieldValue(string value) => $"\"{value}\"";

        static string CleanFieldName(string fieldName)
        {
            // Replace spaces and other characters with underscores
            return fieldName.Replace(" ", "_").Replace("-", "_");
        }
    }
}