using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TimToolBox.DesignPattern.StateMachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace TimToolBox.ToolClasses.ActionSystem
{
    public class UnitActionController : MonoBehaviour
    {
        private KeyStateMachine<ActionID> _stateMachine;
        public List<UnitAction> UnitActions;
        
        private void Start() {
            InitController();
            foreach (var ea in UnitActions) {
                AddAction(ea.ActionID, ea);
                ea.Init();
                ea.InstantSwitchStateEvent += InstantSwitchState;
            }
            _stateMachine.ChangeState(UnitActions[0].ActionID);
        }

        public void Update() {
            if(_stateMachine.CurrentState is UnitAction {Status: ActionState.Stopped}) 
                BackToDefaultState();
            _stateMachine.Update();
        }
        
        public void FixedUpdate() {
            ((UnitAction) _stateMachine.CurrentState)?.OnFixedUpdateState();
        }
        
        public void BackToDefaultState() {
            _stateMachine.ChangeState(UnitActions[0].ActionID);
        }
        public void InitController()
        {
            _stateMachine = new KeyStateMachine<ActionID>();
        }
        public void AddAction(ActionID actionID, UnitAction unitAction)
        {
            _stateMachine.AddState(actionID, unitAction);
        }
        public void AddTransition(ActionID fromActionID, ActionID toActionID,IPredicate condition)
        {
            _stateMachine.AddTransition(fromActionID, toActionID, condition);
        }
        private void InstantSwitchState(ActionID targetActionID) {
            _stateMachine.ChangeState(targetActionID);
        }
        
        [OnInspectorGUI]
        public void DrawButtons() {
            //finish this method
            if (UnitActions == null) return;
            GUILayout.Space(10);
            GUILayout.Label("Actions:", EditorStyles.boldLabel);
            foreach (var action in UnitActions) {
                if (GUILayout.Button(action.ActionID.ToString())) {
                    _stateMachine.ChangeState(action.ActionID);
                }
            }
        }
    }
}
