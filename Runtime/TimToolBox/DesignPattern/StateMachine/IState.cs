using UnityEngine;

namespace TimToolBox.DesignPattern.StateMachine {
    public interface IState {
        void OnEnter();
        void OnUpdate();
        void OnFixedUpdate();
        void OnExit();
    }
}
