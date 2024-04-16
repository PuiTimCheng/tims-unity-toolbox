using System;
using System.Collections;
using System.Collections.Generic;
using TimToolBox.DesignPattern.StateMachine;
using UnityEngine;

namespace TimToolBox.ToolClasses.ActionSystem
{
    public class UnitActionController : MonoBehaviour
    {
        private KeyStateMachine<ActionID> _stateMachine;
        
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
    }

    public class UnitAction : MonoBehaviour, IState
    {
        public ActionState Status { get; private set; }
        public void OnEnterState() { }

        public void OnUpdateState() { }

        public void OnExitState() { }
    }

    public enum ActionID
    {
        Idle,
        Run,
        Attack,
    }
    
    public enum ActionState
    {
        Running,
        Stopped,
    }
    
    public enum ActionEndReason
    {
        Completed,
        Interrupted,
    }
}
