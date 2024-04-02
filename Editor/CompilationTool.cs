using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
 
public static class CompilationTool
{
    [MenuItem("Tim'sToolBox/Compile Tool/Request Script Compilation")]
    private static void RequestScriptCompilation()
    {
        CompilationPipeline.RequestScriptCompilation();
    }
}