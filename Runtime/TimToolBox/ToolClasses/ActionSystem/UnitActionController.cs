using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TimToolBox.DesignPattern.StateMachine;
using TimToolBox.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace TimToolBox.ToolClasses.ActionSystem
{
    public class UnitActionController : MonoBehaviour
    {
        private StateMachine _stateMachine;
        public List<UnitAction> unitActions;
        
        public UnitAction CurrentAction => (UnitAction) _stateMachine?.CurrentState;
        
        private void Start() {
            _stateMachine = new StateMachine();
            if (unitActions == null) {
                unitActions = new List<UnitAction>();
                ReadChildActions();
            }
            foreach (var unitAction in unitActions) {
                AddAction(unitAction);
                unitAction.Init();
            }
            _stateMachine.ChangeStateTo(unitActions[0]);
        }
        
        public void Update() {
            var curAction = CurrentAction;
            var actionStatus = curAction.OnActionUpdate();
            curAction.Status = actionStatus;
            if(actionStatus != ActionStatus.Running) {
                BackToDefaultState();
            }
        }
        
        public void FixedUpdate() {
            (_stateMachine.CurrentState as UnitAction)?.OnActionFixedUpdate();
        }
        
        public void BackToDefaultState() {
            _stateMachine.ChangeStateTo(unitActions[0]);
        }
        public void AddAction(UnitAction unitAction)
        {
            _stateMachine.AddState(unitAction);
        }
        public UnitAction GetAction<T>() where T : UnitAction
        {
            return (UnitAction) _stateMachine.GetState<T>();
        }
        public void StartAction<T>() where T : UnitAction
        {
            _stateMachine.ChangeStateTo<T>();
        }

        [Button]
        public void ReadChildActions() {
            foreach (Transform child in transform) {
                var action = child.GetComponent<UnitAction>();
                if (action != null) {
                    unitActions.Add(action);
                }
            }
        }
        
        
        [OnInspectorGUI]
        public void DrawDebug() {
            if(CurrentAction) GUILayout.Label($"Current State: {CurrentAction.OrNull()}", EditorStyles.boldLabel);
        }
        [OnInspectorGUI]
        public void DrawButtons() {
            //finish this method
            if (unitActions == null) return;
            GUILayout.Space(10);
            GUILayout.Label("Actions:", EditorStyles.boldLabel);
            foreach (var action in unitActions) {
                if (GUILayout.Button(action.ToString())) {
                    _stateMachine.ChangeStateTo(action);
                }
            }
        }
    }
}
