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
        
        public override void OnActionStart() {
            base.OnActionStart();
            animator.CrossFadeInFixedTime(AnimationHash,transitionDuration);
        }
        public override ActionStatus OnActionUpdate() {
            if (!isLoop && SelfStopTimer.IsFinished) {
                return ActionStatus.Succeeded;
            } 
            return base.OnActionUpdate();
        }
        public override void OnActionFixedUpdate() {
            base.OnActionFixedUpdate();
        }

        public override void OnExitState() {
            base.OnExitState();
            Debug.Log($"{GetType()} OnExit");
        }
    }
}