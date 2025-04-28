using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;


    /// <summary>
    /// This editor script converts txt word lists to JSON format.
    /// </summary>
    public class JSONDictionaryCreator : EditorWindow
    {
        #region Fields

        private TextAsset sourceFile;

        #endregion

        #region Unity Methods

        private void OnGUI()
        {
            GUILayout.Label("Source Text File (.txt)", EditorStyles.boldLabel);
            sourceFile = (TextAsset) EditorGUILayout.ObjectField("Select File", sourceFile, typeof(TextAsset), false);

            if (GUILayout.Button("Create JSON"))
            {
                CreateJsonFile();
            }
        }

        #endregion

        #region Private Methods

        [MenuItem("Tools/Create JSON Dictionary")]
        private static void ShowWindow()
        {
            var window = GetWindow<JSONDictionaryCreator>();
            window.titleContent = new GUIContent("JSON Dictionary Creator");
            window.minSize = new Vector2(300, 100);
            window.Show();
        }

        private void CreateJsonFile()
        {
            if (sourceFile == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select a source text file.", "OK");
                return;
            }

            string sourceText = sourceFile.text;
            List<string> words = sourceText.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.Trim()).Where(word => !string.IsNullOrEmpty(word)).ToList();

            if (words.Count == 0)
            {
                EditorUtility.DisplayDialog("Warning", "The source file is empty or contains no valid words.", "OK");
                return;
            }

            var dictionaryData = new Dictionary<string, List<string>> {{"words", words}};

            string jsonContent = JsonConvert.SerializeObject(dictionaryData, Formatting.Indented);

            string savePath = Path.Combine(Application.dataPath, "dictionary.json");

            try
            {
                File.WriteAllText(savePath, jsonContent);
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Success", $"JSON dictionary created successfully at: {savePath}", "OK");
                Debug.Log($"JSON dictionary created at: {savePath}");
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Error", $"Failed to create JSON file: {e.Message}", "OK");
                Debug.LogError($"Failed to create JSON file: {e.Message}");
            }
        }

        #endregion
    }

