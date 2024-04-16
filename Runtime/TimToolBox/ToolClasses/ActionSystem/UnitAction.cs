using System;
using Sirenix.OdinInspector;
using TimToolBox.DesignPattern.StateMachine;
using UnityEditor;
using UnityEngine;

namespace TimToolBox.ToolClasses.ActionSystem {
    public class UnitAction : MonoBehaviour, IState
    {
        public ActionID ActionID;
        public ActionState Status { get; protected set; }
        public float ActionDuration;
        
        protected Timer SelfStopTimer;
        
        public virtual void Init() {
            Status = ActionState.Stopped;
            SelfStopTimer = new Timer(ActionDuration);
        }

        public virtual void OnEnterState() {
            Debug.Log($"{ActionID} OnEnterState");
            Status = ActionState.Running;
            SelfStopTimer.Start();
        }

        public virtual void OnUpdateState() {
            UpdateActionStatus();
        }

        public virtual void OnFixedUpdateState() {
        }

        public virtual void OnExitState(){
            Debug.Log($"{ActionID} OnExitState");
        }

        protected virtual void UpdateActionStatus() {
            if (SelfStopTimer.IsFinished) {
                Status = ActionState.Stopped;
            }
        }

        public event Action<ActionID> InstantSwitchStateEvent = _ => { };
        public void InstantSwitchState(ActionID toActionID) {
            InstantSwitchStateEvent(toActionID);
        }
        [OnInspectorGUI]
        public void DrawDebug() {
            GUILayout.Label($"Status: {Status}", EditorStyles.boldLabel);
        }
    }
    
    public enum ActionID
    {
        Null,
        Idle,
        Walk,
        Attack,
    }
    
    public enum ActionState
    {
        Stopped,
        Running,
    }
    
    public enum ActionEndReason
    {
        Completed,
        Interrupted,
    }
}