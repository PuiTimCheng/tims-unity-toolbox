using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class TemplateScriptUtil
{
    public static void GenerateScriptFromTemplate(string templateScriptPath, string generatedScriptPath,
        Dictionary<string, string> keywordsToContext)
    {
        try
        {
            // Read the template script
            string templateContent = File.ReadAllText(templateScriptPath);

            // Substitute keywords with their context
            foreach (var pair in keywordsToContext)
            {
                templateContent = templateContent.Replace(pair.Key, pair.Value);
            }

            // Write the modified content to the new script file
            File.WriteAllText(generatedScriptPath, templateContent);
            Debug.Log("Script generated successfully at " + generatedScriptPath);
            AssetDatabase.Refresh();
        }
        catch (Exception ex)
        {
            Debug.Log("An error occurred: " + ex.Message);
        }
    }

    [MenuItem("Assets/Create/ScriptableObject", isValidateFunction: false, priority: 80)]
    public static void CreateNewScriptableObject()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
            "Packages/com.ptcheng.toolbox/ScriptTemplates/ScriptableObjectTemplate.txt", "NewSO.cs");
    }
}