using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniExtension
{
    /// <summary>
    /// Generate Enum which contains all scenes in scene list
    /// </summary>
    [InitializeOnLoad]
    public class SceneEnumGenerator
    {
        private const string k_OutputPath = "Assets/Scripts/Generated/ScenesEnum.cs";
        private const string k_EnumName = "SceneName";

        static SceneEnumGenerator()
        {
            Debug.Log("Kek");

            var names = new List<string>();

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                names.Add(
                    Path.GetFileNameWithoutExtension(
                        SceneUtility.GetScenePathByBuildIndex(i)
                    )
                );
            }

            if (names.Count == 0)
                return;

            GenerateEnumFile(names);
        }

        private static void GenerateEnumFile(List<string> sceneNames)
        {
            var sb = new StringBuilder();
            sb.AppendLine("// AUTO-GENERATED FILE. DO NOT EDIT.");
            sb.AppendLine();
            sb.AppendLine("public enum " + k_EnumName);
            sb.AppendLine("{");

            foreach (string sceneName in sceneNames)
                sb.AppendLine($"\t{Sanitize(sceneName)},");

            sb.AppendLine("}");

            var newContent = sb.ToString();

            if (File.Exists(k_OutputPath))
            {
                var existingContent = File.ReadAllText(k_OutputPath);

                if (existingContent == newContent)
                {
                    return;
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(k_OutputPath));
            File.WriteAllText(k_OutputPath, sb.ToString(), Encoding.UTF8);

            AssetDatabase.Refresh();
        }

        private static string Sanitize(string name)
        {
            var clean = new string(name
                .Where(char.IsLetterOrDigit)
                .ToArray());

            if (char.IsDigit(clean[0]))
                clean = "_" + clean;

            return clean;
        }
    }
}
