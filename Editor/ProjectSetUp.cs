using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

namespace TimToolBox {
    public static class ProjectSetUp {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders() {
            Folders.CreateDefault("_Project", "Animation", "Art", "Materials", "Prefabs", "Scripts/ScriptableObjects", "Scripts/UI");
            Refresh();
        }
        
        [MenuItem("Tools/Setup/Import Must Have Folders")]
        public static void ImportMustHavePackages() {
            try {
                var folderPath = "F:/GameDev/UnityPlugins/_mustHaveInProject";
                // Get all files with a .unitypackage extension in the specified folder
                string[] packageFiles = Directory.GetFiles(folderPath, "*.unitypackage");
                // Display the package names
                foreach (var packageFileName in packageFiles)
                {
                    Debug.Log("Unity Package Name: " + GetFileNameWithoutExtension(packageFileName));
                    Assets.ImportAsset(packageFileName, "",folderPath, false);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error retrieving package names: " + e.Message);
            }
        }
        
        [MenuItem("Tools/Setup/Import Basic Unity Packages")]
        public static void ImportBasicUnityPackages() {
            Packages.InstallPackages(new[] {
                "com.unity.addressables",
                "com.unity.inputsystem",
                "com.unity.cinemachine",
            });
        }
        
        //[MenuItem("Tools/Setup/Import My Favorite Assets")]
        public static void ImportMyFavoriteAssets() {
            Assets.ImportAsset("DOTween HOTween v2.unitypackage", "Demigiant/ScriptingAnimation");
        }

        //[MenuItem("Tools/Setup/Install Netcode for GameObjects")]
        public static void InstallNetcodeForGameObjects() {
            Packages.InstallPackages(new[] {
                "com.unity.multiplayer.tools",
                "com.unity.netcode.gameobjects"
            });
        }

        //[MenuItem("Tools/Setup/Install Unity AI Navigation")]
        public static void InstallUnityAINavigation() {
            Packages.InstallPackages(new[] {
                "com.unity.ai.navigation"
            });
        }

        //[MenuItem("Tools/Setup/Install My Favorite Open Source")]
        public static void InstallOpenSource() {
            Packages.InstallPackages(new[] {
                "git+https://github.com/KyleBanks/scene-ref-attribute",
                "git+https://github.com/starikcetin/Eflatun.SceneReference.git#3.1.1"
            });
        }

        static class Folders {
            public static void CreateDefault(string root, params string[] folders) {
                var fullpath = Path.Combine(Application.dataPath, root);
                if (!Directory.Exists(fullpath)) {
                    Directory.CreateDirectory(fullpath);
                }
                foreach (var folder in folders) {
                    CreateSubFolders(fullpath, folder);
                }
            }
    
            private static void CreateSubFolders(string rootPath, string folderHierarchy) {
                var folders = folderHierarchy.Split('/');
                var currentPath = rootPath;
                foreach (var folder in folders) {
                    currentPath = Path.Combine(currentPath, folder);
                    if (!Directory.Exists(currentPath)) {
                        Directory.CreateDirectory(currentPath);
                    }
                }
            }
        }

        static class Packages {
            static AddRequest Request;
            static Queue<string> PackagesToInstall = new();

            public static void InstallPackages(string[] packages) {
                foreach (var package in packages) {
                    PackagesToInstall.Enqueue(package);
                }

                // Start the installation of the first package
                if (PackagesToInstall.Count > 0) {
                    Request = Client.Add(PackagesToInstall.Dequeue());
                    EditorApplication.update += Progress;
                }
            }

            static async void Progress() {
                if (Request.IsCompleted) {
                    if (Request.Status == StatusCode.Success)
                        Debug.Log("Installed: " + Request.Result.packageId);
                    else if (Request.Status >= StatusCode.Failure)
                        Debug.Log(Request.Error.message);

                    EditorApplication.update -= Progress;

                    // If there are more packages to install, start the next one
                    if (PackagesToInstall.Count > 0) {
                        // Add delay before next package install
                        await Task.Delay(1000);
                        Request = Client.Add(PackagesToInstall.Dequeue());
                        EditorApplication.update += Progress;
                    }
                }
            }
        }

        static class Assets {
            public static void ImportAsset(string asset, string subfolder,
                string rootFolder = "F:/GameDev/UnityPlugins/_mustHaveInProject", bool interactive = false) {
                ImportPackage(Combine(rootFolder, subfolder, asset), interactive);
            }
        }
    }
}