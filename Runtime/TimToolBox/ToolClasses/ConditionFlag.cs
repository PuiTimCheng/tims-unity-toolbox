using TimToolBox.DesignPattern.StateMachine;

namespace TimToolBox {
    public class ConditionFlag : IPredicate{
        private bool _flag;
        public void Set() => _flag = true;
        public void Reset() => _flag = false;
        public bool Evaluate() {
            return _flag;
        }
    }
}