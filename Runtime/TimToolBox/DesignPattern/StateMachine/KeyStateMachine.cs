using System.Collections.Generic;
using UnityEngine;

namespace TimToolBox.DesignPattern.StateMachine
{
    /// <summary>
    /// A State Machine That is operated based on Key
    /// </summary>
    public class KeyStateMachine<TKey>
    {
        private KeyStateNode<TKey> _currentNode;
        private Dictionary<TKey, KeyStateNode<TKey>> _nodes = new();
        HashSet<IKeyStateTransition<TKey>> _anyTransitions = new();

        public void Update() {
            var transition = GetTransition();
            if (transition != null) ChangeState(transition.ToKey);

            _currentNode?.State?.OnUpdateState();
        }
        
        public bool AddState(TKey key, IState state)
        {
            var node = _nodes.GetValueOrDefault(key);
            if (node == null) {
                node = new KeyStateNode<TKey>(key,state);
                _nodes.Add(key, node);
                return true;
            }
            else
            {
                Debug.Log($"Key:{key} was previous added");
                return false;
            }
        }

        public IState GetState(TKey key)
        {
            var node = _nodes.GetValueOrDefault(key);
            return node?.State;
        }

        public IState CurrentState => _currentNode?.State;
        
        public bool RemoveState(TKey key)
        {
            var node = _nodes.GetValueOrDefault(key);
            if(node != null)
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

        public void AddAnyTransition(TKey toStateKey, IPredicate condition)
        {
            _anyTransitions.Add(new KeyStateTransition<TKey>(toStateKey, condition));
        }
        
        public void ChangeState(TKey toStateKey) {
            var to = _nodes.GetValueOrDefault(toStateKey);
            if(to == null) Debug.LogWarning($"State By Key: {toStateKey} not found");
            ChangeStateWithNode(to);
        }
        private void ChangeStateWithNode(KeyStateNode<TKey> toNode)
        {
            _currentNode?.State.OnExitState();
            toNode?.State.OnEnterState();
            _currentNode = toNode;
        }
        
        IKeyStateTransition<TKey> GetTransition() {
            foreach (var transition in _anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;

            foreach (var transition in _currentNode.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;

            return null;
        }
    }


    public class KeyStateNode<TKey>
    {
        public TKey Key { get; }
        public IState State { get; }
        
        public HashSet<IKeyStateTransition<TKey>> Transitions { get; }
        
        public KeyStateNode(TKey key,IState state)
        {
            Key = key;
            State = state;
            Transitions = new HashSet<IKeyStateTransition<TKey>>();
        }
        
        public void AddTransition(TKey toKey, IPredicate condition) {
            Transitions.Add(new KeyStateTransition<TKey>(toKey, condition));
        }
    }
    public interface IKeyStateTransition<TKey>
    {
        TKey ToKey { get; }
        IPredicate Condition { get; }
    }

    public class KeyStateTransition<TKey> : IKeyStateTransition<TKey>
    {
        public TKey ToKey { get; }
        public IPredicate Condition { get; }

        public KeyStateTransition(TKey toKey, IPredicate condition)
        {
            ToKey = toKey;
            Condition = condition;
        }
    }
}