using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UniExtension
{
    [InitializeOnLoad]
    public class AnimatorParametersGenerator : AssetModificationProcessor
    {
        private const string k_FileName = "AnimatorParameters";
        private const string k_FilePath = "Assets/Scripts/Generated/AnimatorParameters.cs";

        static AnimatorParametersGenerator()
        {
            GenerateIfNeeded();
        }

        static string[] OnWillSaveAssets(string[] paths)
        {
            foreach (var path in paths)
            {
                if (path.EndsWith(".controller"))
                {
                    GenerateIfNeeded();
                    break;
                }
            }

            return paths;
        }

        static void GenerateIfNeeded()
        {
            string[] guids = AssetDatabase.FindAssets("t:AnimatorController");
            Dictionary<string, List<string>> parameters = new();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);

                parameters[controller.name] = new();

                foreach (var param in controller.parameters)
                    parameters[controller.name].Add(param.name);
            }

            string newContent = GenerateContent(parameters);

            if (File.Exists(k_FilePath))
            {
                string oldContent = File.ReadAllText(k_FilePath);

                if (oldContent == newContent)
                    return;
            }

            File.WriteAllText(k_FilePath, newContent);
            AssetDatabase.Refresh();

            Debug.Log("Animator parameters regenerated");
        }

        static string GenerateContent(Dictionary<string, List<string>> parameters)
        {
            StringBuilder sb = new();

            sb.AppendLine("using UnityEngine;");
            sb.AppendLine();
            sb.AppendLine("public static class AnimatorParameters");
            sb.AppendLine("{");

            foreach (var controller in parameters)
            {
                foreach (var param in controller.Value)
                {
                    string name = ToCamelCase(controller.Key) + ToCamelCase(param);
                    int hash = Animator.StringToHash(param);

                    sb.AppendLine($"    public static readonly int {name} = {hash};");
                }
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        static string ToCamelCase(string value)
        {
            var parts = value
                .Split(new[] { ' ', '_', '-' }, System.StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new();

            foreach (var part in parts)
            {
                sb.Append(char.ToUpper(part[0]));
                if (part.Length > 1)
                    sb.Append(part.Substring(1));
            }

            return sb.ToString();
        }
    }
}
