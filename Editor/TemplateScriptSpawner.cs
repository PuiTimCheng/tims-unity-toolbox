using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MatchThreeRoguelike
{
    public class TemplateScriptSpawner : MonoBehaviour
    {
 
        [MenuItem("Assets/Create/ScriptableObject", isValidateFunction: false, priority: 80)]
        public static void CreateNewScriptableObject()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile("Packages/com.ptcheng.toolbox/ScriptTemplates/ScriptableObjectTemplate.txt", "NewSO.cs");
        }
    }
}
