using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimToolBox.DebugTool {
    public static class DebugExtension {

        #region GizmosDraw
        
        public static void GizmoDrawLabel(Vector3 position, string text, Color color, float fontSize = 12) {
#if UNITY_EDITOR
            var style = new GUIStyle(GUI.skin.box);
            style.normal.textColor = color;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.UpperLeft;
            UnityEditor.Handles.Label(position, text, style);
#endif
        }

        #endregion

        public static void DrawPlane(Vector3 origin, Vector3 normal, Color color, float sideLength = 1,float duration = 0)
        {
#if UNITY_EDITOR
            if (normal == Vector3.zero) return;
            DrawArrow(origin, normal, color, duration);
            var rotation = Quaternion.LookRotation(normal, Vector3.up);
            DrawSquare(origin, rotation, sideLength, color);
#endif
        }

        /// <summary>  Draw an arrow using Debug.Draw</summary>
        public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float duration = 0, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
#if UNITY_EDITOR
            if (direction == Vector3.zero) return;
            Debug.DrawRay(pos, direction, color, duration);

            var length = direction.magnitude;

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Debug.DrawRay(pos + direction, arrowHeadLength * length * right, color, duration);
            Debug.DrawRay(pos + direction, arrowHeadLength * length * left, color, duration);
#endif
        }
        
        public static void DrawCircle(Vector3 origin, Quaternion rotation, float radius, Color color,
                                      int segments = 24, float duration = 0.3f) {
            Vector3 previousPoint = origin + rotation * (Vector3.forward * radius);
            float angleStep = 360f / segments;

            for (int i = 1; i <= segments + 1; i++) {
                Vector3 direction = Quaternion.Euler(0f, angleStep * i, 0f) * Vector3.forward;
                Vector3 currentPoint = origin + rotation * (direction * radius);
                Debug.DrawLine(previousPoint, currentPoint, color, duration);
                previousPoint = currentPoint;
            }
        }

        public static void DrawSphere(Vector3 origin, Quaternion rotation, float radius, Color color,
                                      int segments = 24, float duration = 0f) {
            // Draw circles in 3 orthogonal planes (XY, XZ, YZ)
            DrawCircle(origin, rotation, radius, color, segments, duration); // XY plane
            DrawCircle(origin, rotation * Quaternion.Euler(90, 0, 0), radius, color, segments, duration); // XZ plane
            DrawCircle(origin, rotation * Quaternion.Euler(0, 0, 90), radius, color, segments, duration); // YZ plane
        }

        public static void DrawEllipse(Vector3 origin, Quaternion rotation, float majorAxis, float minorAxis,
                                       Color color, int segments = 24, float duration = 0.3f) {
            Vector3 previousPoint = origin + rotation * (new Vector3(majorAxis, 0, 0));
            float angleStep = 360f / segments;

            for (int i = 1; i <= segments + 1; i++) {
                float currentAngle = angleStep * i;
                Vector3 pointOffset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * currentAngle) * majorAxis, 0,
                    Mathf.Sin(Mathf.Deg2Rad * currentAngle) * minorAxis);
                Vector3 currentPoint = origin + rotation * pointOffset;
                Debug.DrawLine(previousPoint, currentPoint, color, duration);
                previousPoint = currentPoint;
            }
        }

        public static void DrawArc(Vector3 origin, Quaternion rotation, float radius, float angle, Color color, int segments = 24, float duration = 0f)
        {
            float angleStep = angle / segments;
            Vector3 previousPoint = origin + rotation * Quaternion.Euler(0, -angle / 2, 0) * Vector3.forward * radius;

            for (int i = 1; i <= segments + 1; i++)
            {
                Quaternion segmentRotation = Quaternion.Euler(0, angleStep * i - angle / 2, 0);
                Vector3 currentPoint = origin + rotation * segmentRotation * Vector3.forward * radius;
                Debug.DrawLine(previousPoint, currentPoint, color, duration);
                previousPoint = currentPoint;
            }
        }

        public static void DrawRectangle(Vector3 origin, Quaternion rotation, Vector2 size, Color color,
                                         float duration = 0.3f) {
            Vector3 forward = rotation * Vector3.forward * size.y;
            Vector3 right = rotation * Vector3.right * size.x;
            Vector3 backLeft = origin - forward / 2 - right / 2;
            Vector3 backRight = origin - forward / 2 + right / 2;
            Vector3 frontRight = origin + forward / 2 + right / 2;
            Vector3 frontLeft = origin + forward / 2 - right / 2;

            Debug.DrawLine(backLeft, backRight, color, duration);
            Debug.DrawLine(backRight, frontRight, color, duration);
            Debug.DrawLine(frontRight, frontLeft, color, duration);
            Debug.DrawLine(frontLeft, backLeft, color, duration);
        }

        // Assuming the view cone is aligned along the forward direction of the provided rotation
        public static void DrawViewCone(Vector3 origin, Quaternion rotation, float viewDistance, float viewAngle,
                                        Color color, int segments = 24, float duration = 0.3f) {
            DrawArc(origin, rotation, viewDistance, viewAngle, color, segments, duration);

            Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * rotation * Vector3.forward * viewDistance;
            Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * rotation * Vector3.forward * viewDistance;

            Debug.DrawLine(origin, origin + leftBoundary, color, duration);
            Debug.DrawLine(origin, origin + rightBoundary, color, duration);
        }
        public static void DrawSquare(Vector3 origin, Quaternion rotation, float sideLength, Color color, float duration = 0.3f)
        {
            // Calculate half side length to position the corners relative to the origin
            float halfSide = sideLength / 2;

            // Calculate the corner points
            Vector3 up = rotation * Vector3.up * halfSide;
            Vector3 right = rotation * Vector3.right * halfSide;
            Vector3 corner1 = origin - up - right;
            Vector3 corner2 = origin - up + right;
            Vector3 corner3 = origin + up + right;
            Vector3 corner4 = origin + up - right;

            // Draw the square
            Debug.DrawLine(corner1, corner2, color, duration);
            Debug.DrawLine(corner2, corner3, color, duration);
            Debug.DrawLine(corner3, corner4, color, duration);
            Debug.DrawLine(corner4, corner1, color, duration);
        }
        
        public static void DrawBox(Vector3 origin, Quaternion rotation, Vector3 size, Color color, float duration = 0.3f)
        {
            float halfWidth = size.x / 2;
            float halfHeight = size.y / 2;
            float halfDepth = size.z / 2;

            // Calculate local positions of the corners of the box
            Vector3[] points = new Vector3[8]
            {
                new Vector3(-halfWidth, -halfHeight, -halfDepth),
                new Vector3(halfWidth, -halfHeight, -halfDepth),
                new Vector3(halfWidth, -halfHeight, halfDepth),
                new Vector3(-halfWidth, -halfHeight, halfDepth),
                new Vector3(-halfWidth, halfHeight, -halfDepth),
                new Vector3(halfWidth, halfHeight, -halfDepth),
                new Vector3(halfWidth, halfHeight, halfDepth),
                new Vector3(-halfWidth, halfHeight, halfDepth)
            };

            // Transform points to world space
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = origin + rotation * points[i];
            }

            // Draw bottom square
            Debug.DrawLine(points[0], points[1], color, duration);
            Debug.DrawLine(points[1], points[2], color, duration);
            Debug.DrawLine(points[2], points[3], color, duration);
            Debug.DrawLine(points[3], points[0], color, duration);

            // Draw top square
            Debug.DrawLine(points[4], points[5], color, duration);
            Debug.DrawLine(points[5], points[6], color, duration);
            Debug.DrawLine(points[6], points[7], color, duration);
            Debug.DrawLine(points[7], points[4], color, duration);

            // Draw vertical lines
            Debug.DrawLine(points[0], points[4], color, duration);
            Debug.DrawLine(points[1], points[5], color, duration);
            Debug.DrawLine(points[2], points[6], color, duration);
            Debug.DrawLine(points[3], points[7], color, duration);
        }
    }
}