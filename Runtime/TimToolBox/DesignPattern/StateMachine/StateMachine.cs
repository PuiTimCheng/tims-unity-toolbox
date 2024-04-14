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
                ChangeState(transition.To);

            _current.State?.OnUpdate();
        }

        public void FixedUpdate() {
            _current.State?.OnFixedUpdate();
        }
        
        public IState GetState<T>() where T : IState {
            return _nodes.GetValueOrDefault(typeof(T))?.State;
        }

        public void SetState(IState state) {
            _current = _nodes[state.GetType()];
            _current.State?.OnEnter();
        }

        public void ChangeState<T>() where T : IState {
            var state = _nodes.GetValueOrDefault(typeof(T))?.State;
            if (state != default) ChangeState(state);
        }

        public void ChangeState(IState state) {
            var previousState = _current.State;
            var nextState = _nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();
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
        public void AddNode(IState state) {
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