using UnityEngine;

namespace TimToolBox.DesignPattern.StateMachine {
    public interface IState {
        void OnEnterState();
        void OnUpdateState();
        void OnExitState();
    }
}
