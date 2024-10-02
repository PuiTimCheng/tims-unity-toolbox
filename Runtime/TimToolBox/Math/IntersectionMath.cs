using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimToolBox.Math {
    public class IntersectionMath {
        
        // Method to calculate ray-plane intersection
        public static bool RayIntersectsPlane(Ray ray, Vector3 planePoint, Vector3 planeNormal, out Vector3 intersection)
        {
            intersection = Vector3.zero;
            // Calculate the denominator of the equation: ray direction · plane normal
            float denominator = Vector3.Dot(ray.direction, planeNormal);
        
            // If denominator is close to zero, the ray is parallel to the plane
            if (Mathf.Approximately(denominator, 0.0f))
            {
                intersection = Vector3.zero;
                return false;
            }
            // Calculate the distance along the ray to the intersection point
            float t = Vector3.Dot(planePoint - ray.origin, planeNormal) / (denominator);
            // If t is positive, there is an intersection in front of the ray's origin
            if (t >= 0)
            {
                intersection = ray.origin + t * ray.direction;
                return true;
            }
            return false; // No intersection
        }
    }
}
