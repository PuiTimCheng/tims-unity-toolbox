using System;
using Sirenix.OdinInspector;
using TimToolBox.DesignPattern.StateMachine;
using UnityEditor;
using UnityEngine;

namespace TimToolBox.ToolClasses.ActionSystem {
    public class UnitAction : MonoBehaviour {
        [SerializeField] private bool _enableDebug = false;
        public ActionStatus Status { get; set; }
        public virtual void Init() {
            Status = ActionStatus.Invalid;
        }
        public virtual void OnActionStart() {
            if(_enableDebug) Debug.Log($"[{gameObject.name}@{GetType().Name}] : OnActionStart");
        }
        public virtual ActionStatus OnActionUpdate() {
            return ActionStatus.Running;
        }
        public virtual void OnActionFixedUpdate() {
        }
        public virtual void OnActionStop() {
            if(_enableDebug) Debug.Log($"[{gameObject.name}@{GetType().Name}] : OnActionStop");
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