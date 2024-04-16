using UnityEngine;

namespace TimToolBox.Extensions {
    public static class AnimationExtensions {
        public static float GetAnimationLength(this Animator animator, int clipHash) {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
                if (Animator.StringToHash(clip.name) == clipHash) {
                    return clip.length;
                }
            }

            return -1f;
        }

        public static float GetAnimationLength(this Animator animator, string stateName) {
            // Get the Animator Controller from the Animator
            var controller = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;

            // Ensure the controller is correctly cast and not null
            if (controller == null) {
                Debug.LogError("Animator Controller is not of type AnimatorController or is null.");
                return -1f;
            }

            // Access the layers, here we're accessing only the first layer
            var layer = controller.layers[0];

            // Iterate over all states in the state machine of the first layer
            foreach (var state in layer.stateMachine.states) {
                if (state.state.name == stateName) {
                    // Return the length of the animation clip associated with the state
                    if (state.state.motion is AnimationClip clip) {
                        return clip.length;
                    }
                    else {
                        Debug.LogWarning($"No AnimationClip found for state {stateName}.");
                        return -1f;
                    }
                }
            }

            // If no state with the given name was found
            Debug.LogWarning($"No state with the name {stateName} found.");
            return -1f;
        }
    }
}