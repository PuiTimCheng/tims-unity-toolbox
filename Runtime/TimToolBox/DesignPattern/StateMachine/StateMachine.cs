using System;
using System.Collections.Generic;

namespace TimToolBox.DesignPattern.StateMachine {
    public class StateMachine {
        StateNode current;
        Dictionary<Type, StateNode> nodes = new();
        HashSet<IStateTransition> anyTransitions = new();

        public void Update() {
            var transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);

            current.State?.OnUpdate();
        }

        public void FixedUpdate() {
            current.State?.OnFixedUpdate();
        }

        public void SetState(IState state) {
            current = nodes[state.GetType()];
            current.State?.OnEnter();
        }

        void ChangeState(IState state) {
            if (state == current.State) return;

            var previousState = current.State;
            var nextState = nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();
            current = nodes[state.GetType()];
        }

        IStateTransition GetTransition() {
            foreach (var transition in anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;

            foreach (var transition in current.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;

            return null;
        }

        public void AddTransition(IState from, IState to, IStatePredicate condition) {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IStatePredicate condition) {
            anyTransitions.Add(new StateTransition(GetOrAddNode(to).State, condition));
        }

        StateNode GetOrAddNode(IState state) {
            var node = nodes.GetValueOrDefault(state.GetType());

            if (node == null) {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
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

            public void AddTransition(IState to, IStatePredicate condition) {
                Transitions.Add(new StateTransition(to, condition));
            }
        }
    }
}