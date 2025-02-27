using System;
using System.Collections.Generic;
using UnityEngine;

namespace TimToolBox.DesignPattern.StateMachine
{
    /// <summary>
    /// A State Machine that uses the Type of the State as the Key
    /// </summary>
    public class TypeStateMachine : StateMachine<Type>
    {
        /// <summary>
        /// Add a state to the state machine with its Type as the key
        /// </summary>
        public bool AddState<TState>(TState state) where TState : IState
        {
            return base.AddState(typeof(TState), state);
        }

        /// <summary>
        /// Remove a state from the state machine by its Type
        /// </summary>
        public bool RemoveState<TState>() where TState : IState
        {
            return base.RemoveState(typeof(TState));
        }

        /// <summary>
        /// Get a state from the state machine by its Type
        /// </summary>
        public TState GetState<TState>() where TState : IState
        {
            return (TState)base.GetState(typeof(TState));
        }

        /// <summary>
        /// Change to a state by its Type
        /// </summary>
        public void ChangeToState<TState>() where TState : IState
        {
            base.ChangeToState(typeof(TState));
        }
        /// <summary>
        /// Change to a state by state
        /// </summary>
        public void ChangeToState(IState state)
        {
            if (state == null)
            {
                Debug.LogError("Cannot change to a null state object");
                return;
            }

            var stateType = state.GetType();
            base.ChangeToState(stateType);
        }
        /// <summary>
        /// Add a transition between two states using their Types
        /// </summary>
        public bool AddTransition<TFromState, TToState>(IPredicate condition)
            where TFromState : IState
            where TToState : IState
        {
            return base.AddTransition(typeof(TFromState), typeof(TToState), condition);
        }

        /// <summary>
        /// Add an "Any State" transition to a specific state Type
        /// </summary>
        public void AddAnyTransition<TToState>(IPredicate condition) where TToState : IState
        {
            base.AddAnyTransition(typeof(TToState), condition);
        }
    }
}