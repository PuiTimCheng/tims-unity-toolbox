using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PackageSetUpData : ScriptableObject {
    [FormerlySerializedAs("hasPromptedMustHavePackages")] [FormerlySerializedAs("hasInstalledMustHavePackages")] public bool hasPromptedInstallMustHavePackages;
    
    
    private const string SettingsDataPath = "Assets/TimToolBoxPackageSetUpData.asset";

    public static PackageSetUpData GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<PackageSetUpData>(SettingsDataPath);
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<PackageSetUpData>();
            AssetDatabase.CreateAsset(settings, SettingsDataPath);
            AssetDatabase.SaveAssets();
        }
        return settings;
    }

    public static void SaveSettings(PackageSetUpData settings)
    {
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
    }
}