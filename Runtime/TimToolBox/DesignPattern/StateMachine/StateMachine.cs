using System;
using System.Collections.Generic;

namespace TimToolBox.DesignPattern.StateMachine {
    public class StateMachine {
        StateNode _current;
        Dictionary<Type, StateNode> _nodes = new();
        HashSet<IStateTransition> _anyTransitions = new();
        public IState CurrentState => _current.State;

        public void Update() {
            var transition = GetTransition();
            if (transition != null)
                ChangeStateTo(transition.To);

            _current.State?.OnUpdateState();
        }
        
        public IState GetState<T>() where T : IState {
            return _nodes.GetValueOrDefault(typeof(T))?.State;
        }
        public bool AddState(IState state) {
            return GetOrAddNode(state) == null;
        }
        public void ChangeStateTo<T>() where T : IState {
            var state = _nodes.GetValueOrDefault(typeof(T))?.State;
            if (state != default) ChangeStateTo(state);
        }
        public void ChangeStateTo(IState state) {
            var previousState = _current?.State;
            var nextState = _nodes[state.GetType()].State;

            previousState?.OnExitState();
            nextState?.OnEnterState();
            _current = _nodes[state.GetType()];
        }

        IStateTransition GetTransition() {
            foreach (var transition in _anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;

            foreach (var transition in _current.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;

            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition) {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }
        public void AddAnyTransition(IState to, IPredicate condition) {
            _anyTransitions.Add(new StateTransition(GetOrAddNode(to).State, condition));
        }
        public void AddStateNode(IState state) {
            var node = _nodes.GetValueOrDefault(state.GetType());
            if (node == null) {
                node = new StateNode(state);
                _nodes.Add(state.GetType(), node);
            }
        }
        StateNode GetOrAddNode(IState state) {
            var node = _nodes.GetValueOrDefault(state.GetType());

            if (node == null) {
                node = new StateNode(state);
                _nodes.Add(state.GetType(), node);
            }

            return node;
        }
        class StateNode {
            public IState State { get; }
            public HashSet<IStateTransition> Transitions { get; }

            public StateNode(IState state) {
                State = state;
                Transitions = new HashSet<IStateTransition>();
            }

            public void AddTransition(IState to, IPredicate condition) {
                Transitions.Add(new StateTransition(to, condition));
            }
        }
    }
}