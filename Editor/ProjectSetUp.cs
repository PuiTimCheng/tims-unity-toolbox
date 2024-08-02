using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

using static System.Environment;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

public static class ProjectSetup {
    [MenuItem("Tim'sToolBox/Setup/Import Essential Assets")]
    public static void ImportEssentials() {
        //Assets.ImportAsset("Odin Inspector and Serializer.unitypackage", "Sirenix/Editor ExtensionsSystem");
        //Assets.ImportAsset("Odin Validator.unitypackage", "Sirenix/Editor ExtensionsUtilities");
        //Assets.ImportAsset("Editor Console Pro.unitypackage", "FlyingWorm/Editor ExtensionsSystem");
        // and so on...
        var folderPath = "Packages/com.ptcheng.toolbox/DownloadedPackage/EssentialPackages";
        // Get all files with a .unitypackage extension in the specified folder
        string[] packageFiles = Directory.GetFiles(folderPath, "*.unitypackage");
        // Display the package names
        foreach (var packageFileName in packageFiles)
        {
            Debug.Log("Importing Unity Package Name: " + GetFileNameWithoutExtension(packageFileName));
            Assets.ImportAssetFromPath(packageFileName);
        }
    }

    [MenuItem("Tim'sToolBox/Setup/Install Essential Packages")]
    public static void InstallPackages() {
        Packages.InstallPackages(new[] {
            //"com.unity.2d.animation",
            //"git+https://github.com/adammyhre/Unity-Utils.git",
            //"git+https://github.com/adammyhre/Unity-Improved-Timers.git",
            "com.unity.addressables",
            "com.unity.cinemachine",
            "com.unity.ai.navigation",
            // If necessary, import new Input System last as it requires a Unity Editor restart
            "com.unity.inputsystem",
        });
    }

    [MenuItem("Tim'sToolBox/Setup/Create Folders")]
    public static void CreateFolders() {
        Folders.Create("_Project", "Animation", "Art", "Materials", "Prefabs", "Scripts/Tests", "Scripts/Tests/Editor", "Scripts/Tests/Runtime");
        Refresh();
        Folders.Move("_Project", "Scenes");
        Folders.Move("_Project", "Settings");
        Folders.Delete("TutorialInfo");
        Refresh();

        MoveAsset("Assets/InputSystem_Actions.inputactions", "Assets/_Project/Settings/InputSystem_Actions.inputactions");
        DeleteAsset("Assets/Readme.asset");
        Refresh();
    }

    static class Assets {
        public static void ImportAsset(string asset, string folder) {
            var basePath = GetFolderPath(SpecialFolder.ApplicationData);
            var assetsFolder = Combine(basePath, "Unity/Asset Store-5.x");
            asset = asset.EndsWith(".unitypackage") ? asset : asset + ".unitypackage";
            ImportPackage(Combine(assetsFolder, folder, asset), false);
        }
        public static void ImportAssetFromPath(string path) {
            var rootFolder = GetDirectoryName(Application.dataPath);
            string absolute =   Path.GetFullPath(path);
            var combine = Combine(rootFolder, "", path);
            ImportPackage(combine, false);
        }
    }

    static class Packages {
        static AddRequest request;
        static Queue<string> packagesToInstall = new Queue<string>();

        public static void InstallPackages(string[] packages) {
            foreach (var package in packages) {
                packagesToInstall.Enqueue(package);
            }

            if (packagesToInstall.Count > 0) {
                StartNextPackageInstallation();
            }
        }

        static async void StartNextPackageInstallation() {
            request = Client.Add(packagesToInstall.Dequeue());
            
            while (!request.IsCompleted) await Task.Delay(10);
            
            if (request.Status == StatusCode.Success) Debug.Log("Installed: " + request.Result.packageId);
            else if (request.Status >= StatusCode.Failure) Debug.LogError(request.Error.message);

            if (packagesToInstall.Count > 0) {
                await Task.Delay(1000);
                StartNextPackageInstallation();
            }
        }
    }

    static class Folders {
        public static void Create(string root, params string[] folders) {
            var fullpath = Combine(Application.dataPath, root);
            if (!Directory.Exists(fullpath)) {
                Directory.CreateDirectory(fullpath);
            }

            foreach (var folder in folders) {
                CreateSubFolders(fullpath, folder);
            }
        }
        
        static void CreateSubFolders(string rootPath, string folderHierarchy) {
            var folders = folderHierarchy.Split('/');
            var currentPath = rootPath;

            foreach (var folder in folders) {
                currentPath = Combine(currentPath, folder);
                if (!Directory.Exists(currentPath)) {
                    Directory.CreateDirectory(currentPath);
                }
            }
        }
        
        public static void Move(string newParent, string folderName) {
            var sourcePath = $"Assets/{folderName}";
            if (IsValidFolder(sourcePath)) {
                var destinationPath = $"Assets/{newParent}/{folderName}";
                var error = MoveAsset(sourcePath, destinationPath);

                if (!string.IsNullOrEmpty(error)) {
                    Debug.LogError($"Failed to move {folderName}: {error}");
                }
            }
        }
        
        public static void Delete(string folderName) {
            var pathToDelete = $"Assets/{folderName}";

            if (IsValidFolder(pathToDelete)) {
                DeleteAsset(pathToDelete);
            }
        }
    }
}