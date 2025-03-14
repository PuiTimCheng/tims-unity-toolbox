using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimToolBox.Extensions
{
    public static class RayCastExtensions
    {
        public static bool CameraMainRaycastFirstHitPos(Vector2 screenPos, out Vector3 worldPos, int ignoreLayers = -1)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreLayers))
            {
                worldPos = hit.point;
                return true;
            }

            worldPos = Vector3.zero;
            return false;
        }
        
        public static bool RaycastToPlane(this Ray ray, Plane plane, out Vector3 worldPos)
        {
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 collisionPoint = ray.GetPoint(distance);
                worldPos = collisionPoint;
                return true;
            }
            worldPos = Vector3.zero;
            return false;
        }
    }
}