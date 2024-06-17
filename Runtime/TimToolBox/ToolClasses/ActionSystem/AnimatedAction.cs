using TimToolBox.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace TimToolBox.ToolClasses.ActionSystem {
    public class AnimatedAction : UnitAction {
        
        public Animator animator;
        public string AnimationName;
        public bool isLoop;
        public float transitionDuration;
        public float ActionDuration;
        
        protected int AnimationHash;

        public override void Init() {
            base.Init();
            AnimationHash = Animator.StringToHash(AnimationName);
            ActionDuration = animator.GetAnimationLength(AnimationHash);
        }
        
        public override void OnActionStart() {
            base.OnActionStart();
            animator.CrossFadeInFixedTime(AnimationHash,transitionDuration);
        }
        public override ActionStatus OnActionUpdate() {
            return base.OnActionUpdate();
        }
        public override void OnActionFixedUpdate() {
            base.OnActionFixedUpdate();
        }
    }
}