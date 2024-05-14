using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RigidBodyLocomotion : MonoBehaviour {
    public Rigidbody rb;

    public Vector3 currentDirection;
    public float speed;
    public bool enableRotateTowardsDirection;
    public float rotationSpeed = 1440f;
    private Vector3 _targetRotationDirection;

    public void MoveTowardDirection(Vector3 direction, float inputSpeed) {
        currentDirection = direction;
        this.speed = inputSpeed;
    }

    private void Start() {
        _targetRotationDirection = rb.transform.forward;
    }

    private void FixedUpdate() {
        PositionFixedUpdate();
        RotationFixedUpdate();
    }

    private void PositionFixedUpdate() {
        /*// Multiply the direction by our desired speed to get a velocity
        Vector3 desiredVelocity = speed * currentDirection.normalized;
        // The velocity change we want to apply
        var applyVelocityChange = desiredVelocity - rb.velocity;
        rb.AddForce(applyVelocityChange, ForceMode.VelocityChange);*/
        
        // Calculate desired velocity
        Vector3 desiredVelocity = speed * currentDirection.normalized;
        // Calculate the difference between desired velocity and current velocity
        Vector3 velocityChange = desiredVelocity - rb.velocity;
        // Calculate the drag force
        Vector3 counterDragForce = rb.drag * rb.velocity;
        // Calculate the force needed to reach the desired velocity considering the drag
        Vector3 finalVelChange = velocityChange + counterDragForce * Time.fixedDeltaTime;
        // Apply the force
        rb.AddForce(finalVelChange, ForceMode.VelocityChange);
    }
    private void RotationFixedUpdate() {
        if (enableRotateTowardsDirection && currentDirection != Vector3.zero) _targetRotationDirection = currentDirection;
        // Rotate towards the target
        if (enableRotateTowardsDirection && _targetRotationDirection != Vector3.zero) {
            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(_targetRotationDirection);
            // Smoothly interpolate between the current rotation and the target rotation
            Quaternion smoothedRotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            // Apply the smoothed rotation
            rb.MoveRotation(smoothedRotation);
            
            
            /*Quaternion targetRotation = Quaternion.LookRotation(_targetRotationDirection);
            Quaternion currentRotation = rb.rotation;
            Quaternion rotationDelta = targetRotation * Quaternion.Inverse(currentRotation);
            float angle;
            Vector3 axis;
            rotationDelta.ToAngleAxis(out angle, out axis);
            // Ensure the angle is between -180 and 180 degrees
            if (angle > 180) angle -= 360;
            // Clamp the angle to avoid excessive rotation per physics update
            angle = Mathf.Clamp(angle, -rotationSpeed * Time.fixedDeltaTime, rotationSpeed * Time.fixedDeltaTime);
            // Calculate the desired angular velocity to achieve this angle change
            Vector3 angularVelocity = angle * Mathf.Deg2Rad * axis.normalized / Time.fixedDeltaTime;
            // Apply the torque
            rb.AddTorque(angularVelocity - rb.angularVelocity, ForceMode.VelocityChange);*/
        }
    }

}