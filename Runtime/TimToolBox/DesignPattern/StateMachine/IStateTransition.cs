namespace TimToolBox.DesignPattern.StateMachine {
    public interface IStateTransition {
        IState To { get; }
        IPredicate Condition { get; }
    }
}