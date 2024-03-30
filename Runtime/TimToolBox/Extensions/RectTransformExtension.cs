using System.Collections.Generic;
using UnityEngine;

namespace MatchThreeRoguelike {
    public static class RectTransformExtension {
        
        public static void SetSize(this RectTransform rectTransform, Vector2 size) {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
    }
}