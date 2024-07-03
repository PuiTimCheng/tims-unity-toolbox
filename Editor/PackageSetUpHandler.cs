using System.IO;
using System.Text;
using TimToolBox;
using UnityEditor;
using static System.IO.Path;

public static class PackageSetUpHandler
{
    [InitializeOnLoadMethod]
    private static void OnEditorLoad()
    {
        var settings = PackageSetUpData.GetOrCreateSettings();
        if (!settings.hasPromptedInstallMustHavePackages)
        {
            PromptInstallRunRequiredPackages();
            settings.hasPromptedInstallMustHavePackages = true;
            PackageSetUpData.SaveSettings(settings);
        }
        
        /*if (!EditorPrefs.GetBool(InstalledRequiredPackageKey, false))
        {
            PromptInstallRunRequiredPackages();
            EditorPrefs.SetBool(InstalledRequiredPackageKey, true);
        }*/
    }

    private static void PromptInstallRunRequiredPackages()
    {
        // Your setup code here
        var sb = new StringBuilder();
        sb.AppendLine("Install Required Packages now? Includes:");
        
        var folderPath = "Packages/com.ptcheng.toolbox/unityPackage/MustHavePackages";
        // Get all files with a .unitypackage extension in the specified folder
        string[] packageFiles = Directory.GetFiles(folderPath, "*.unitypackage");
        // Display the package names
        foreach (var packageFileName in packageFiles) {
            sb.AppendLine(GetFileNameWithoutExtension(packageFileName));
        }
        
        var answer = EditorUtility.DisplayDialog("InstallRunRequiredPackages", sb.ToString(), "Yes", "No");
        if (answer) ProjectSetUp.ImportMustHavePackages();
    }
}
