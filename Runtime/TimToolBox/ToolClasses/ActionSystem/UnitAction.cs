using System;
using Sirenix.OdinInspector;
using TimToolBox.DesignPattern.StateMachine;
using UnityEditor;
using UnityEngine;

namespace TimToolBox.ToolClasses.ActionSystem {
    public class UnitAction : MonoBehaviour, IState
    {
        public ActionStatus Status { get; set; }
        public float ActionDuration;
        
        protected Timer SelfStopTimer;
        
        public virtual void Init() {
            Status = ActionStatus.Invalid;
            SelfStopTimer = new Timer(ActionDuration);
        }

        public virtual void OnActionStart() {
            Debug.Log($"OnStartAction: {GetType().Name}");
            SelfStopTimer.Start();
        }

        public virtual ActionStatus OnActionUpdate() {
            return ActionStatus.Running;
        }

        public virtual void OnActionFixedUpdate() {
            
        }
        
        public virtual void OnActionStop() {
            Debug.Log($"OnExitState: {GetType().Name}");
        }
        
        public void OnEnterState() {
            // noop
        }
        public void OnUpdateState() {
            // noop
        }
        public virtual void OnExitState(){
            // noop
        }
        
        [OnInspectorGUI]
        public void DrawDebug() {
            GUILayout.Label($"Status: {Status}", EditorStyles.boldLabel);
        }
    }
    
    public enum ActionStatus
    {
        Invalid,
        Running,
        Failed,
        Succeeded,
    }
    
    public enum ActionEndReason
    {
        Completed,
        Interrupted,
    }
}