namespace TimToolBox.DesignPattern.StateMachine {
    public class StateTransition : IStateTransition {
        public IState To { get; }
        public IPredicate Condition { get; }

        public StateTransition(IState to, IPredicate condition) {
            To = to;
            Condition = condition;
        }
    }
}