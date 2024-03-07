using System;

namespace TimToolBox.DesignPattern.StateMachine {
    public class FuncStatePredicate : IStatePredicate {
        readonly Func<bool> func;
        
        public FuncStatePredicate(Func<bool> func) {
            this.func = func;
        }
        
        public bool Evaluate() => func.Invoke();
    }
}