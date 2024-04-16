using TimToolBox.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace TimToolBox.ToolClasses.ActionSystem {
    public class AnimatedAction : UnitAction {
        
        public Animator animator;
        public string AnimationName;
        public bool isLoop;
        public float transitionDuration;
        
        protected int AnimationHash;

        public override void Init() {
            base.Init();
            AnimationHash = Animator.StringToHash(AnimationName);
            ActionDuration = animator.GetAnimationLength(AnimationHash);
            SelfStopTimer.Duration = ActionDuration;
        }
        
        public override void OnEnterState() {
            base.OnEnterState();
            SelfStopTimer.Start();
            animator.CrossFadeInFixedTime(AnimationHash,transitionDuration);
        }
        public override void OnUpdateState() {
            base.OnUpdateState();
        }
        public override void OnFixedUpdateState() {
            base.OnFixedUpdateState();
        }

        protected override void UpdateActionStatus() {
            if (!isLoop && SelfStopTimer.IsFinished) {
                Status = ActionState.Stopped;
            } 
        }

        public override void OnExitState() {
            base.OnExitState();
            Debug.Log($"{GetType()} OnExit");
        }
    }
}