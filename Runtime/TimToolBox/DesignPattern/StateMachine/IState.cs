using UnityEngine;

namespace TimToolBox.DesignPattern.StateMachine {
    public interface IState {
        void OnEnterState();
        void OnExitState();
        void OnUpdateState();
    }
}
