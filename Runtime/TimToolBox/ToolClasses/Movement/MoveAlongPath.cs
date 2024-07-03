using System.Collections.Generic;
using TimToolBox.Extensions;
using UnityEngine;

public class MoveAlongPath : MonoBehaviour
{
    public Rigidbody rb;
    public RigidBodyLocomotion locomotion;
    public List<Vector3> wayPoints;

    [Header("Parameters")] 
    public float moveSpeed = 5;
    public float reachedWayPointDistance;
    public float endPointSlowDistance;
    public bool reachedLastWayPointIndex;
    public bool reachedDestination;
    
    private int _currentWaypointIndex = -1;
   
    public void SetPath(List<Vector3> path, bool startFollowPath = true)
    {
        wayPoints = path;
        if (startFollowPath)
        {
            StartFollowPath();
        }
    }
    
    public void StartFollowPath()
    {
        _currentWaypointIndex = 0;
        reachedDestination = false;
        reachedLastWayPointIndex = false;
    }

    private void FixedUpdate()
    {
        // We have no path to follow yet, so don't do anything
        if (wayPoints == null || _currentWaypointIndex == -1) {
            /*if(rb.velocity != Vector3.zero) {
                rb.AddForce(-rb.velocity, ForceMode.VelocityChange);
            }*/
            return;
        }
        
        if (reachedLastWayPointIndex)
        {
            var dis = Vector3.Distance(transform.position, wayPoints[_currentWaypointIndex]);
            if (dis < reachedWayPointDistance)
            {
                locomotion.MoveTowardDirection(Vector3.zero, 0);
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
            distanceToWaypoint = Vector3.Distance(transform.position, wayPoints[_currentWaypointIndex]);
            if (distanceToWaypoint < reachedWayPointDistance) {
                // Check if there is another waypoint or if we have reached the end of the path
                if (_currentWaypointIndex + 1 < wayPoints.Count) {
                    _currentWaypointIndex++;
                }
                else {
                    break;
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
        
        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (wayPoints[_currentWaypointIndex] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 desiredVelocity = speed * dir;
        
        locomotion.MoveTowardDirection(dir, speed);
        
        /*// The velocity change we want to apply
        var applyVelocityChange = desiredVelocity - rb.velocity;
        rb.AddForce(applyVelocityChange, ForceMode.VelocityChange);
        
        // Rotate towards the target
        if (desiredVelocity != Vector3.zero) {
            var rotation = Quaternion.LookRotation(desiredVelocity);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
        }*/
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
