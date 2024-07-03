using System.Collections.Generic;
using TimToolBox.Extensions;
using UnityEditor;
using UnityEngine;

#if  ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TimToolBox.ToolClasses.ActionSystem
{
    public class UnitActionController : MonoBehaviour
    {
        public List<UnitAction> unitActions;
        public UnitAction defaultAction;
        public UnitAction currentAction;
        
        private void Start() {
            if (unitActions == null) {
                unitActions = new List<UnitAction>();
                ReadChildActions();
            }
            foreach (var unitAction in unitActions) {
                unitAction.Init();
            }
            if (unitActions != null && !defaultAction) defaultAction = unitActions[0];
            StartAction(defaultAction);
        }
        
        public void Update() {
            if (currentAction) {
                var actionStatus = currentAction.OnActionUpdate();
                currentAction.Status = actionStatus;
                if(actionStatus != ActionStatus.Running) {
                    BackToDefaultState();
                }
            }
        }
        public void FixedUpdate() {
            if(currentAction)currentAction.OnActionFixedUpdate();
        }

        public void StartAction(UnitAction nextAction){
            if(currentAction) currentAction.OnActionStop();
            currentAction = nextAction;
            if(currentAction) currentAction.OnActionStart();
        }
        
        public void BackToDefaultState() {
            StartAction(defaultAction);
        }

#if  ODIN_INSPECTOR
        [Button]
#endif
        public void ReadChildActions() {
            unitActions.Clear();
            var actions = transform.GetComponentsInChildren<UnitAction>();
            foreach (var action in actions) {
                if (action != null) {
                    unitActions.Add(action);
                }
            }
        }
        
#if  ODIN_INSPECTOR
        [OnInspectorGUI]
        public void DrawDebug() {
            if(currentAction) GUILayout.Label($"Current State: {currentAction.OrNull()}", EditorStyles.boldLabel);
        }
        [OnInspectorGUI]
        public void DrawButtons() {
            if (unitActions == null) return;
            GUILayout.Space(10);
            GUILayout.Label("Actions:", EditorStyles.boldLabel);
            foreach (var action in unitActions) {
                if (GUILayout.Button(action.ToString())) {
                    StartAction(action);
                }
            }
        }
#endif
    }
}
