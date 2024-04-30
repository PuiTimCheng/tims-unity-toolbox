using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TimToolBox.DebugTool;
using TimToolBox.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class MoveAlongPath : MonoBehaviour
{
    public Rigidbody rb;
    public List<Vector3> wayPoints;

    [Header("Parameters")] 
    public float moveSpeed;
    public float reachedWayPointDistance;
    public float endPointSlowDistance;
    
  
    [ReadOnly] public bool reachedLastWayPointIndex;
    [ReadOnly] public bool reachedDestination;
    
    private int _currentWaypointIndex = -1;
    private Vector3 _lastAppliedVelChange;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RayCastExtensions.CameraMainRaycastFirstHitPos(Input.mousePosition, out var worldPos))
            {
                SetPath(new List<Vector3>() { transform.position, worldPos });
                //GetNewPath(worldPos);
            }
        }
    }

    /*public void GetNewPath(Vector3 destination)
    {
        // There must be an AstarPath instance in the scene
        if (AstarPath.active == null) return;
        // We can calculate multiple paths asynchronously
        for (int i = 0; i < 10; i++) {
            var p = ABPath.Construct(transform.position, destination, (p) =>
            {
                SetPath(p.vectorPath);
            });
            // Calculate the path by using the AstarPath component directly
            AstarPath.StartPath (p);
        }
    }*/
    public void SetPath(List<Vector3> path)
    {
        wayPoints = path;
        StartFollowPath();
    }
    
    [Button]
    public void StartFollowPath()
    {
        _currentWaypointIndex = 0;
        _lastAppliedVelChange = Vector3.zero;
        reachedDestination = false;
        reachedLastWayPointIndex = false;
    }

    private void FixedUpdate()
    {
        // We have no path to follow yet, so don't do anything
        if (wayPoints == null || _currentWaypointIndex == -1) {
            if(rb.velocity != Vector3.zero)
                rb.velocity = Vector3.zero;
            return;
        }
        
        if (reachedLastWayPointIndex)
        {
            var dis = Vector3.Distance(transform.position, wayPoints[_currentWaypointIndex]);
            if (dis < reachedWayPointDistance)
            {
                reachedDestination = true;
                _currentWaypointIndex = -1;
                return;
            }
        }
        
        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedLastWayPointIndex = _currentWaypointIndex == wayPoints.Count - 1;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true) {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, wayPoints[_currentWaypointIndex]);
            if (distanceToWaypoint < reachedWayPointDistance) {
                // Check if there is another waypoint or if we have reached the end of the path
                if (_currentWaypointIndex + 1 < wayPoints.Count) {
                    _currentWaypointIndex++;
                }
            } else {
                break;
            }
        }

        // find the slow down effect limit
        var slowDownSpeedLimit = reachedLastWayPointIndex ? 
            Mathf.Clamp01(distanceToWaypoint/ endPointSlowDistance) * moveSpeed : 
            Mathf.Clamp01(distanceToWaypoint/ reachedWayPointDistance) * moveSpeed;
        // find the distance limit
        var distanceSpeedLimit = distanceToWaypoint / Time.fixedDeltaTime;
        var speed = moveSpeed.AtMost(slowDownSpeedLimit).AtMost(distanceSpeedLimit);
        Debug.Log($"slowDownSpeedLimit:{slowDownSpeedLimit}, distanceSpeedLimit:{distanceSpeedLimit}, speed:{speed}");
        
        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (wayPoints[_currentWaypointIndex] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 desiredVelocity = speed * dir;
        // The velocity change we want to apply
        var applyVelocityChange = desiredVelocity - rb.velocity;
        rb.AddForce(applyVelocityChange, ForceMode.VelocityChange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (wayPoints != null)
        {
            foreach (var point in wayPoints)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}
