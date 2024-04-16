using System;
using System.Collections;
using System.Collections.Generic;
using TimToolBox.DesignPattern.StateMachine;
using UnityEngine;

namespace TimToolBox.ToolClasses.ActionSystem
{
    public class UnitActionController : MonoBehaviour
    {
        private StateMachine _stateMachine;
        
        public void InitController()
        {
            _stateMachine = new StateMachine();
        }
        public void AddAction(UnitAction unitAction)
        {
            _stateMachine.AddStateNode(unitAction);
        }
        public void AddTransition<T1,T2>(IPredicate condition) where T1 : IState where T2 : IState
        {
            _stateMachine.AddTransition(_stateMachine.GetState<T1>(), _stateMachine.GetState<T2>(), 
                condition);
        }
        public void StartAtDefaultAction(UnitAction unitAction)
        {
            _stateMachine.SetState(unitAction);
        }
        public UnitAction CurrentAction => (UnitAction)_stateMachine.CurrentState;
        private void Update()
        {
            _stateMachine.Update();
        }
    }

    public class UnitAction : MonoBehaviour, IState
    {
        public ActionState Status { get; private set; }
        public void OnEnterState() { }

        public void OnUpdateState() { }

        public void OnExitState() { }
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
