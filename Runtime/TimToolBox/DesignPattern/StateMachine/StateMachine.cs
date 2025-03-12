using System.Collections.Generic;
using UnityEngine;

namespace TimToolBox.DesignPattern.StateMachine
{
    /// <summary>
    /// A State Machine That is operated based on Key
    /// </summary>
    public class StateMachine<TKey>
    {
        private StateNode<TKey> _currentNode;
        private Dictionary<TKey, StateNode<TKey>> _nodes = new();
        HashSet<IStateTransition<TKey>> _anyTransitions = new();

        public virtual void Update() {
            var transition = GetTransition();
            if (transition != null) ChangeToState(transition.ToKey);

            _currentNode?.State?.OnUpdateState();
        }
        
        public virtual bool AddState(TKey key, IState state)
        {
            if (_nodes.TryGetValue(key, out var node))
            {
                Debug.Log($"Key:{key} was previous added");
                return false;
            }
            else
            {
                node = new StateNode<TKey>(key,state);
                _nodes.Add(key, node);
                return true;
            }
        }
        public virtual bool RemoveState(TKey key)
        {
            if (_nodes.TryGetValue(key, out var node))
            {
                _nodes.Remove(key);
                return true;
            }
            else
            {
                Debug.Log($"Key:{key} was not found");
                return false;
            }
        }
        
        public virtual IState GetState(TKey key)
        {
            var node = _nodes.GetValueOrDefault(key);
            return node?.State;
        }

        public virtual IState CurrentState => _currentNode?.State;
        
        public bool AddTransition(TKey fromStateKey, TKey toStateKey, IPredicate condition) {
            var from = _nodes.GetValueOrDefault(fromStateKey);
            var to = _nodes.GetValueOrDefault(toStateKey);
            if (from != null && to != null)
            {
                from.AddTransition(toStateKey, condition);
                return true;
            }
            else
            {
                Debug.Log($"Key:{fromStateKey} or {toStateKey} was not found");
                return false;
            }
        }

        public virtual void AddAnyTransition(TKey toStateKey, IPredicate condition)
        {
            _anyTransitions.Add(new StateTransition<TKey>(toStateKey, condition));
        }
        
        public virtual void ChangeToState(TKey toStateKey) {
            var to = _nodes.GetValueOrDefault(toStateKey);
            if(to == null) Debug.LogWarning($"State By Key: {toStateKey} not found");
            ChangeStateWithNode(to);
        }
        private void ChangeStateWithNode(StateNode<TKey> toNode)
        {
            if (toNode == null)
            {
                Debug.LogError("Attempting to change to a null state node");
                return;
            }
            _currentNode?.State.OnExitState();
            toNode?.State.OnEnterState();
            _currentNode = toNode;
        }
        
        IStateTransition<TKey> GetTransition() {
            foreach (var transition in _anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;

            foreach (var transition in _currentNode.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;

            return null;
        }
        public class StateNode<TKey>
        {
            public TKey Key { get; }
            public IState State { get; }
        
            public HashSet<IStateTransition<TKey>> Transitions { get; }
        
            public StateNode(TKey key,IState state)
            {
                Key = key;
                State = state;
                Transitions = new HashSet<IStateTransition<TKey>>();
            }
        
            public void AddTransition(TKey toKey, IPredicate condition) {
                Transitions.Add(new StateTransition<TKey>(toKey, condition));
            }
        }
        public interface IStateTransition<TKey>
        {
            TKey ToKey { get; }
            IPredicate Condition { get; }
        }

        public class StateTransition<TKey> : IStateTransition<TKey>
        {
            public TKey ToKey { get; }
            public IPredicate Condition { get; }

            public StateTransition(TKey toKey, IPredicate condition)
            {
                ToKey = toKey;
                Condition = condition;
            }
        }
    }


}