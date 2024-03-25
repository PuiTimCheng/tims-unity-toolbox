using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TimToolBox.Extensions;
using UnityEngine;

namespace TimToolBox.DebugTool {
    [ExecuteAlways]
    public class DebugTextInScene : Singleton<DebugTextInScene>
    {
        public class DebugTextInSceneData {
            public string text;
            public Color color;
            public float fontSize;
            public Vector3 position;
            public float duration;
            public float startTime;
        }
        public List<DebugTextInSceneData> datas = new List<DebugTextInSceneData>();

        [Button]
        public void Test() {
            Add("TestText", new Vector3(100, 100, 0), Color.red, 20, 5);
        }
        
        public void Add(string text,Vector3 position, Color color, float fontSize, float duration) {
            datas.Add(new DebugTextInSceneData() {
                text = text,
                position = position,
                color = color,
                fontSize = fontSize,
                duration = duration,
                startTime = Time.time
            });
        }

        private void Update() {
            datas = datas.FindAll(data => Time.time - data.startTime < data.duration);
        }

        private void OnGUI() {
            foreach (var data in datas) {
                // show data text
                GUIStyle guiStyle = new GUIStyle();
                guiStyle.fontSize = (int)data.fontSize;
                guiStyle.normal.textColor = data.color;
                GUI.Label(new Rect(data.position.x, data.position.y, 300, 20), data.text, guiStyle);
            }
        }
    }
}
