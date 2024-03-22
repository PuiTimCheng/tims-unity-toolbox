namespace TimToolBox.DesignPattern.StateMachine {
    /// <summary>
    /// Predicate is a function that tests a condition and then returns a boolean value - true or false.
    /// </summary>
    public interface IPredicate {
        bool Evaluate();
    }
}