namespace TimToolBox.DesignPattern.StateMachine {
    public interface IStateTransition {
        IState To { get; }
        IStatePredicate Condition { get; }
    }
}